
public class Action : Task
{

    System.Action m_action;
    public Action(System.Action action) : base("Action")
    {
        m_action = action;
    }

    protected override void DoStart()
    {
        m_action?.Invoke();
        Stopped();
    }
    
    
}
