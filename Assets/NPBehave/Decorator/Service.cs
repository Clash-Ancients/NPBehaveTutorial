using System;
public class Service : Decorator
{
    System.Action m_serviceAction;
    
    float m_inverval = -1f;
    
    public Service(System.Action _service, Node _decorator) : base("Service", _decorator)
    {
        m_serviceAction = _service;
    }
    
    public Service(float _interval,  System.Action _service, Node _decorator) : base("Service", _decorator)
    {
        m_serviceAction = _service;
        m_inverval = _interval;
    }
    
    protected override void DoStart()
    {
        if (m_inverval <= 0f)
        {
            if (null != m_serviceAction)
            {
                m_rootNode.Clock.OnAddUpdateObserver(m_serviceAction);
                m_serviceAction?.Invoke();
            }
        }
        else
        {
            InvokeIntervalAction();
        }
        
        m_decorator.Start();
    }

    protected override void DoStop()
    {
        m_decorator.Stop();
    }
    
    protected override void DoChildStopped(Node child, bool success)
    {
        if (m_inverval <= 0)
        {
            m_rootNode.Clock.OnRemoveUpdateObserver(m_serviceAction);
        }
        else
        {
            m_rootNode.Clock.RemoveTimer(InvokeIntervalAction);
        }
        
        Stopped(success);
        
    }

    void InvokeIntervalAction()
    {
        m_serviceAction?.Invoke();
        m_rootNode.Clock.AddTimer(m_inverval, 0, InvokeIntervalAction);
    }
}
