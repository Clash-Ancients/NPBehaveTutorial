using System.Collections.Generic;
public class Clock
{

    List<System.Action> m_observer = new List<System.Action>();

    bool m_isUpdate = false;
    
    public int NumTimers
    {
        get
        {
            return m_dicTimers.Count;
        }
    }
    
    public int DebugPoolSize
    {
        get
        {
            return this.m_listTimer.Count;
        }
    }
    
    public int NumUpdateObservers
    {
        get
        {
            return m_observer.Count;
        }
    }
    
    public class Timer
    {
        public float delay = 0f;
        public float elapsedTime = 0f;
        public int repeat = 0;
        public bool used = false;
        public void ScheduleElapsedTime(float _elpasedTime)
        {
            elapsedTime = delay + _elpasedTime;
        }
    }

    float m_elapsedTime = 0f;

    Dictionary<System.Action, Timer> m_dicTimers = new Dictionary<System.Action, Timer>();
    Dictionary<System.Action, Timer> m_dicAddTimers = new Dictionary<System.Action, Timer>();
    HashSet<System.Action> m_removeTimers = new HashSet<System.Action>();
    List<Timer> m_listTimer = new List<Timer>();
    int m_curTimerIndex = 0;
    
    public void AddTimer(float _delay, int _repeat, System.Action _action)
    {
        Timer timer = null;

        if (!m_isUpdate)
        {
            if (!m_dicTimers.ContainsKey(_action))
            {
                timer = getTimerFromPool();
                m_dicTimers.Add(_action, timer);
            }
            else
            {
                timer = m_dicTimers[_action];
            }
        }
        else
        {
            if (!m_dicAddTimers.ContainsKey(_action))
            {
                m_dicAddTimers.Add(_action, getTimerFromPool());
            }
            timer = m_dicAddTimers[_action];

            if (m_removeTimers.Contains(_action))
            {
                m_removeTimers.Remove(_action);
            }


        }
        
        timer.used = true;
        timer.delay = _delay;
        timer.repeat = _repeat;
        timer.ScheduleElapsedTime(m_elapsedTime);
    }

    public void RemoveTimer(System.Action _action)
    {
        if (!m_isUpdate)
        {
            if (m_dicTimers.ContainsKey(_action))
            {
                m_dicTimers[_action].used = false;
                m_dicTimers.Remove(_action);
            }
        }
        else
        {
            if (m_dicTimers.ContainsKey(_action))
            {
                m_removeTimers.Add(_action);
            }

            if (m_dicAddTimers.ContainsKey(_action))
            {
                m_dicAddTimers[_action].used = false;
                m_dicAddTimers.Remove(_action);
            }
            
        }
    }

    Timer getTimerFromPool()
    {
        Timer timer = null;
        
        var i = 0;
        var size = m_listTimer.Count;
        while (i < size)
        {
            var timerIndex = (i + m_curTimerIndex)%size;
            if (!m_listTimer[timerIndex].used)
            {
                m_curTimerIndex = timerIndex;
                timer = m_listTimer[m_curTimerIndex];
                break;
            }
            i++;
        }
        
        if (null == timer)
        {
            timer = new Timer();
            m_curTimerIndex = 0;
            m_listTimer.Add(timer);
        }
        
        return timer;
    }
    
    public void OnAddUpdateObserver(System.Action observer)
    {
        if (!m_observer.Contains(observer))
        {
            m_observer.Add(observer);
        }
    }

    public void OnRemoveUpdateObserver(System.Action observer)
    {
        if (m_observer.Contains(observer))
        {
            m_observer.Remove(observer);
        }
    }
    
    public void Update(float deltaTime)
    {
        m_elapsedTime += deltaTime;

        m_isUpdate = true;
        
        foreach (var VARIABLE in m_observer)
        {
            VARIABLE?.Invoke();
        }
        
        foreach (var action in m_dicTimers.Keys)
        {
            if(m_removeTimers.Contains(action))
                continue;
            
            var timer = m_dicTimers[action];

            if (timer.elapsedTime < m_elapsedTime)
            {
                if (timer.repeat == 0)
                {
                    RemoveTimer(action);
                }
                
                action?.Invoke();
                timer.ScheduleElapsedTime(m_elapsedTime);
            }
        }

        foreach (var VARIABLE in m_dicAddTimers)
        {
            if (m_dicTimers.ContainsKey(VARIABLE.Key))
            {
                m_dicTimers[VARIABLE.Key].used = false;
            }

            m_dicTimers[VARIABLE.Key] = VARIABLE.Value;
        }

        foreach (var VARIABLE in m_removeTimers)
        {
            m_dicTimers[VARIABLE].used = false;
            m_dicTimers.Remove(VARIABLE);
        }
        
        m_dicAddTimers.Clear();
        m_removeTimers.Clear();
        
        m_isUpdate = false;
    }
}
