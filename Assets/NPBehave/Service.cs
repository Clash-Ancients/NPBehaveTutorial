using System;

public class Service : Decorator
{
    Action m_serviceMethod;

    // float m_interval = -1f;
    //
    // float m_randomVariation = 0f;
    
    public Service(Action _serviceMethod, Node _decoratee) : base("Service", _decoratee)
    {
        m_serviceMethod = _serviceMethod;
    }
    
    protected override void DoStart()
    {
        if (null != m_serviceMethod)
        {
            m_serviceMethod?.Invoke();
        }

        if (null != m_decoratee)
        {
            m_decoratee.Start();
        }
    }
}
