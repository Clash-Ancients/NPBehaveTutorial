using System;
public class Service : Decorator
{
    Action m_serviceAction;
    
    float m_inverval = -1f;
    
    public Service(Action _service, Node _decorator) : base("Service", _decorator)
    {
        m_serviceAction = _service;
     
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
        
        m_decorator.Start();
    }

    protected override void DoStop()
    {
        m_decorator.Stop();
    }
    
    protected override void DoChildStopped(Node child)
    {
        if (m_inverval <= 0)
        {
            m_rootNode.Clock.OnRemoveUpdateObserver(m_serviceAction);
        }
        
        Stopped();
        
    }
}
