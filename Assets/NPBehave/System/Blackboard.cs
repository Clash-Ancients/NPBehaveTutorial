
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
    
    public Blackboard(Clock _clock)
    {
        m_clock = _clock;
    }
    
    Dictionary<string, object> m_dicBlackboard = new Dictionary<string, object>();

    List<Notification> m_listNotify = new List<Notification>();

    bool m_isNotify = false;

    Dictionary<string, List<System.Action>> m_dicObservers = new Dictionary<string, List<System.Action>>();
    Dictionary<string, List<System.Action>> m_dicAddObservers = new Dictionary<string, List<System.Action>>();
    Dictionary<string, List<System.Action>> m_dicRemoveObservers = new Dictionary<string, List<System.Action>>();
    
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
        return null;
    }
    
    void Set(string key, object newvalue)
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

    public void AddObserver(string _key, System.Action _callback)
    {
        if (!m_isNotify)
        {
            if (!m_dicObservers.ContainsKey(_key))
            {
                m_dicObservers[_key] = new List<System.Action>
                {
                    _callback
                };
            }
            else
            {
                m_dicObservers[_key].Add(_callback);
            }
        }
        else
        {
            if (!m_dicAddObservers.ContainsKey(_key))
            {
                m_dicAddObservers[_key] = new List<System.Action>
                {
                    _callback
                };
            }
            else
            {
                m_dicAddObservers[_key].Add(_callback);
            }

            if (m_dicRemoveObservers.ContainsKey(_key))
            {
                var list = m_dicRemoveObservers[_key];
                if (list.Contains(_callback))
                {
                    list.Remove(_callback);
                }
            }
        }
    }

    public void RemoveObserver(string _key)
    {
        if (!m_isNotify)
        {
            
        }
        else
        {
            
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
        
        
        foreach (var VARIABLE in m_dicAddObservers)
        {
            m_dicObservers[VARIABLE.Key].AddRange(VARIABLE.Value);
        }
        
        m_dicAddObservers.Clear();

        m_isNotify = false;
    }
}
