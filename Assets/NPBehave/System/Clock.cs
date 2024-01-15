using System;
using System.Collections.Generic;
public class Clock
{

    List<Action> m_observer = new List<Action>();
    
    public void OnAddUpdateObserver(Action observer)
    {
        if (!m_observer.Contains(observer))
        {
            m_observer.Add(observer);
        }
    }

    public void OnRemoveUpdateObserver(Action observer)
    {
        if (m_observer.Contains(observer))
        {
            m_observer.Remove(observer);
        }
    }
    
    public void Update(float deltaTime)
    {
        foreach (var VARIABLE in m_observer)
        {
            VARIABLE?.Invoke();
        }
    }
}
