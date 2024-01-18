
using System.Collections.Generic;
public class Blackboard
{
    
    public enum eBBOpeType
    {
        eADD,
        eREMOVE,
        eCHANGE,
    }
    public struct Notification
    {
        public string key;
        public eBBOpeType bbType;
        public object value;
        public Notification(string _key, eBBOpeType _type, object _value)
        {
            key = _key;
            bbType = _type;
            value = _value;
        }
    }
    
    Clock m_clock;
    
    public Clock Clock => m_clock;

    Blackboard m_parentBlackboard;
    
    public Blackboard(Clock _clock)
    {
        m_clock = _clock;
        m_parentBlackboard = null;
    }

    public Blackboard(Blackboard parentbb, Clock _clock)
    {
        m_parentBlackboard = parentbb;
        m_clock = _clock;
    }
    
    Dictionary<string, object> m_dicBlackboard = new Dictionary<string, object>();

    List<Notification> m_listNotify = new List<Notification>();

    bool m_isNotify = false;

    Dictionary<string, List<System.Action>> m_dicObservers = new Dictionary<string, List<System.Action>>();
    Dictionary<string, List<System.Action>> m_dicAddObservers = new Dictionary<string, List<System.Action>>();
    Dictionary<string, List<System.Action>> m_dicRemoveObservers = new Dictionary<string, List<System.Action>>();
    HashSet<Blackboard> children = new HashSet<Blackboard>();
    public object this[string key]
    {
        get
        {
            return m_dicBlackboard[key];
        }
        set
        {
            Set(key, value);
        }
    }

    public void Enable()
    {
        if (this.m_parentBlackboard != null)
        {
            this.m_parentBlackboard.children.Add(this);
        }
    }
    
    public void Disable()
    {
        if (this.m_parentBlackboard != null)
        {
            this.m_parentBlackboard.children.Remove(this);
        }
        if (this.m_clock != null)
        {
            this.m_clock.RemoveTimer(NotifyCB);
        }
    }
    
    public T Get<T>(string key)
    {
        object result = Get(key);
        if (result == null)
        {
            return default(T);
        }
        return (T)result;
    }

    public object Get(string key)
    {
        if (m_dicBlackboard.ContainsKey(key))
        {
            return m_dicBlackboard[key];
        }
        else if (this.m_parentBlackboard != null)
        {
            return this.m_parentBlackboard.Get(key);
        }
        else
        {
            return null;
        }
    }
    
    void Set(string key, object newvalue)
    {
        if (this.m_parentBlackboard != null && this.m_parentBlackboard.Isset(key))
        {
            this.m_parentBlackboard.Set(key, newvalue);
        }
        else
        {
            if (!m_dicBlackboard.ContainsKey(key))
            {
                var notification = new Notification(key, eBBOpeType.eADD, newvalue);
                m_listNotify.Add(notification);

                m_dicBlackboard[key] = newvalue;
                m_clock.AddTimer(0,0, NotifyCB);
            }
            else
            {
                if (m_dicBlackboard[key] != newvalue)
                {
                    var notification = new Notification(key, eBBOpeType.eCHANGE, newvalue);
                    m_listNotify.Add(notification);
                
                    m_dicBlackboard[key] = newvalue;
                    m_clock.AddTimer(0,0, NotifyCB);
                }
            }
        }
        
    }

    public bool Isset(string key)
    {
        return m_dicObservers.ContainsKey(key) || (this.m_parentBlackboard != null && this.m_parentBlackboard.Isset(key));
    }
    
    public void AddObserver(string _key, System.Action _callback)
    {
        var listObserver = GetObserverList(m_dicObservers, _key);
        if (!m_isNotify)
        {
            if (!listObserver.Contains(_callback))
            {
                listObserver.Add(_callback);
            }
        }
        else
        {
            var listAddObserver = GetObserverList(m_dicAddObservers, _key);
            if (!listObserver.Contains(_callback) && !listAddObserver.Contains(_callback))
            {
                listAddObserver.Add(_callback);
            }
            
            var listRemoveObserver = GetObserverList(m_dicRemoveObservers, _key);
            if (listRemoveObserver.Contains(_callback))
            {
                listRemoveObserver.Remove(_callback);
            }
        }
    }

    public void RemoveObserver(string _key, System.Action observer)
    {

        var listObserver = GetObserverList(m_dicObservers, _key);
        
        if (!m_isNotify)
        {
            if (listObserver.Contains(observer))
            {
                listObserver.Remove(observer);
            }
        }
        else
        {
            var listRemoveObserver = GetObserverList(m_dicRemoveObservers, _key);
            
            if (!listRemoveObserver.Contains(observer) && listObserver.Contains(observer))
            {
                listRemoveObserver.Add(observer);
            }
            
            var listAddObserver = GetObserverList(m_dicAddObservers, _key);

            if (listAddObserver.Contains(observer))
            {
                listAddObserver.Remove(observer);
            }
            
            
        }
    }
    
    void NotifyCB()
    {
        m_isNotify = true;
        
        foreach (var notification in m_listNotify)
        {
            if(!m_dicObservers.ContainsKey(notification.key))
                continue;
            
            var list = m_dicObservers[notification.key];
            
            foreach (var action in list)
            {
                if(m_dicRemoveObservers.ContainsKey(notification.key) && m_dicRemoveObservers[notification.key].Contains(action))
                    continue;
                
                action?.Invoke();
            }
        }
        
        
        foreach (var key in m_dicAddObservers.Keys)
        {
            GetObserverList(m_dicObservers, key).AddRange(m_dicAddObservers[key]);
        }

        foreach (var key in m_dicRemoveObservers.Keys)
        {
            foreach (System.Action action in m_dicRemoveObservers[key])
            {
                GetObserverList(m_dicObservers, key).Remove(action);
            }
        }
        
        m_dicAddObservers.Clear();
        m_dicRemoveObservers.Clear();
        m_listNotify.Clear();
        m_isNotify = false;
    }

    List<System.Action> GetObserverList(Dictionary<string, List<System.Action>> _dicObserver, string _key)
    {
        if (!_dicObserver.ContainsKey(_key))
        {
            _dicObserver.Add(_key, new List<System.Action>());
        }

        return _dicObserver[_key];
    }
    
      
#if UNITY_EDITOR
    public List<string> Keys
    {
        get
        {
            if (this.m_parentBlackboard != null)
            {
                List<string> keys = this.m_parentBlackboard.Keys;
                keys.AddRange(m_dicBlackboard.Keys);
                return keys;
            }
            else
            {
                return new List<string>(m_dicBlackboard.Keys);
            }
        }
    }

    public int NumObservers
    {
        get
        {
            int count = 0;
            foreach (string key in m_dicObservers.Keys)
            {
                count += m_dicObservers[key].Count;
            }
            return count;
        }
    }
#endif
    
}
