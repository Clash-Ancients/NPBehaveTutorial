

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
            Stopped();
        }
        else
        {
            m_decorator.Start();
        }
    }

    protected override void DoChildStopped(Node child)
    {
        if (m_isObserving)
        {
            StopObserving();
            m_isObserving = false;
        }
        
        Stopped();
    }

    protected void Evaluate()
    {
        
    }
    
    protected abstract bool IsConditionMet();

    protected abstract void StartObserving();

    protected abstract void StopObserving();

}
