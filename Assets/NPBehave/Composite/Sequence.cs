
public class Sequence : Composite
{

    public Sequence(params Node[] _child) : base("Sequence", _child)
    {
    }


    protected override void DoStart()
    {
        m_curIndex = -1;
        
    }
    
    
    protected override void DoChildStopped(Node child)
    {
       
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
}
