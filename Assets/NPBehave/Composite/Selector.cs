
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

    protected override void DoStop()
    {
        m_children[m_curIndex].Stop();
    }
    
    protected override void OnProcessChildren()
    {
        if (++m_curIndex < m_children.Length)
        {
            if (IsStopRequested)
            {
                Stopped(false);
            }
            else
            {
                m_children[m_curIndex].Start();
            }
        }
        else
        {
            Stopped(false);
        }
    }
    public override void StopLowerPriorityChildren(Node child, bool immediateRestart)
    {
        int indexForChild = 0;
        bool found = false;
        foreach (var VARIABLE in m_children)
        {
            if (VARIABLE == child)
            {
                found = true;
            }
            else if (!found)
            {
                indexForChild++;
            }
            else if(found && VARIABLE.IsActive)
            {
                if (immediateRestart)
                {
                    m_curIndex = indexForChild - 1;
                }
                else
                {
                    m_curIndex = m_children.Length;
                }
                VARIABLE.Stop();
                break;
            }
        }
    }

    protected override void DoChildStopped(Node child, bool success)
    {
        if (success)
        {
            Stopped(true);
        }
        else
        {
            OnProcessChildren();
        }
    }
}