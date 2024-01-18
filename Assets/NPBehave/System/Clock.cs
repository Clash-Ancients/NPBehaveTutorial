using System.Collections.Generic;
using UnityEngine;
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
    
    HashSet<System.Action> m_addObservers = new HashSet<System.Action>();
    HashSet<System.Action> m_removeObservers = new HashSet<System.Action>();
    
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
        Debug.Log("add");
        if (!m_isUpdate)
        {
            if (!m_observer.Contains(observer))
            {
                m_observer.Add(observer);
            }
        }
        else
        {
            if (!m_observer.Contains(observer))
            {
                m_addObservers.Add(observer);
            }

            if (m_removeObservers.Contains(observer))
            {
                m_removeObservers.Remove(observer);
            }
        }
        
    }

    public void OnRemoveUpdateObserver(System.Action observer)
    {
        Debug.Log("remove");
        if (!m_isUpdate)
        {
            if (m_observer.Contains(observer))
            {
                m_observer.Remove(observer);
            }
        }
        else
        {
            if (m_observer.Contains(observer) && !m_removeObservers.Contains(observer))
            {
                m_removeObservers.Add(observer);
            }
            
            if (m_addObservers.Contains(observer))
            {
                m_addObservers.Remove(observer);
            }
        }
       
    }
    
    public void Update(float deltaTime)
    {
        m_elapsedTime += deltaTime;

        m_isUpdate = true;
        
        foreach (var VARIABLE in m_observer)
        {
            if(m_removeObservers.Contains(VARIABLE))
                continue;
            VARIABLE?.Invoke();
        }
        
        foreach (var VARIABLE in m_addObservers)
        {
            m_observer.Add(VARIABLE);
        }

        foreach (var VARIABLE in m_removeObservers)
        {
            m_observer.Remove(VARIABLE);
        }
        
        m_addObservers.Clear();
        m_removeObservers.Clear();
        
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
