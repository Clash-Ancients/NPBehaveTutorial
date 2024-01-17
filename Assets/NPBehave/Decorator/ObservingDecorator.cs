public abstract class ObservingDecorator : Decorator
{
    protected STOPS m_stop;
    bool m_isObserving = false;
    public ObservingDecorator(string _name, Node _decorator) : base(_name, _decorator)
    {
    }
    
    protected override void DoStart()
    {
        if (!m_isObserving)
        {
            StartObserving();
            m_isObserving = true;
        }
        
        if (!IsConditionMet())
        {
            Stopped(false);
        }
        else
        {
            m_decorator.Start();
        }
    }
    
    protected override void DoStop()
    {
        m_decorator.Stop();
    }
    
    protected override void DoChildStopped(Node child, bool success)
    {
        if (m_stop != STOPS.IMMEDIATE_RESTART)
        {
            if (m_isObserving)
            {
                StopObserving();
                m_isObserving = false;
            }
        }
        
        
        Stopped(success);
    }

    protected void Evaluate()
    {
        if (IsActive && !IsConditionMet())
        {
            //
            Stop();
        }
        else if (!IsActive && IsConditionMet())
        {
            
            //如果当前非激活 && 条件满足：
            /*
             * 1 让sequence 停止
             * 2 让blackboard打开
             */

            Container parentNode = ParentNode;
            Node childNode = this;
            while (null != parentNode && !(parentNode is Composite))
            {
                childNode = parentNode;
                parentNode = parentNode.ParentNode;
            }
            
            if (m_stop == STOPS.IMMEDIATE_RESTART)
            {
                if (m_isObserving)
                {
                    StopObserving();
                    m_isObserving = false;
                }
            }

            var composite = (Composite)parentNode;
            
            composite.StopLowerPriorityChildren(childNode, true);

        }
    }
    
    protected abstract bool IsConditionMet();

    protected abstract void StartObserving();

    protected abstract void StopObserving();
}
