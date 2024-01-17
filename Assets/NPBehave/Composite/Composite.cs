public abstract class Composite : Container
{

    protected Node[] m_children;
    
    protected  int m_curIndex = -1;
    
    public Composite(string _name, params Node[] _child) : base(_name)
    {
        m_children = _child;

        foreach (var VARIABLE in m_children)
        {
            VARIABLE.SetParent(this);
        }
    }

    public override void SetRoot(Root _root)
    {
        base.SetRoot(_root);
        foreach (var VARIABLE in m_children)
        {
            VARIABLE.SetRoot(_root);
        }
    }

    protected abstract void OnProcessChildren();
    
    public abstract void StopLowerPriorityChildren(Node child, bool immediateRestart);
    
#if UNITY_EDITOR
    public override Node[] DebugChildren
    {
        get
        {
            return this.m_children;
        }
    }

    public Node DebugGetActiveChild()
    {
        foreach( Node node in DebugChildren )
        {
            if(node.NodeState == eNodeState.eACTIVE )
            {
                return node;
            }
        }

        return null;
    }
#endif
}
