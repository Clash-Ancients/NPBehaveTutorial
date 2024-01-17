using NPBehave;
public class Root : Decorator
{

    protected Node m_mainNode;

    protected Clock m_clock;

    Blackboard m_blackboard;
    public Blackboard Blackboard => m_blackboard;
    
    public Clock GetRootClock()
    {
        return m_clock;
    }
    
    public Root( Node _mainnode) : base("Root", _mainnode)
    {
        m_mainNode = _mainnode;
        m_clock = UnityContext.GetClock();
        m_blackboard = new Blackboard(m_clock);
        SetRoot(this);
    }

    public override void SetRoot(Root _root)
    {
        base.SetRoot(_root);
        m_mainNode.SetRoot(_root);
    }
    
    protected override void DoStart()
    {
        m_blackboard.Enable();
        m_mainNode.Start();
    }

    #if UNITY_EDITOR
    public int TotalNumStartCalls = 0;
    public int TotalNumStopCalls = 0;
    public int TotalNumStoppedCalls = 0;
#endif
    
    protected override void DoStop()
    {
        m_mainNode.Stop();
    }
    
    protected override void DoChildStopped(Node child, bool success)
    {
        m_blackboard.Disable();
       Stopped(success);
    }
}
