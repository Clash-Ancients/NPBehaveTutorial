
public class Selector : Composite
{

   
    public Selector(params Node[] _child) : base("Selector", _child)
    {
    }


    protected override void DoStart()
    {
        m_curIndex = -1;

        OnProcessChildren();
    }

    protected override void OnProcessChildren()
    {
        if (++m_curIndex < m_children.Length)
        {
            if (IsStopRequested)
            {
                Stopped();
            }
            else
            {
                m_children[m_curIndex].Start();
            }
        }
        else
        {
            Stopped();
        }
    }
    
    protected override void DoChildStopped(Node child)
    {
       
    }
}
