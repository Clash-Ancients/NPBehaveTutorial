public abstract class Composite : Container
{

    protected Node[] m_children;
    
    protected  int m_curIndex = -1;
    
    public Composite(string _name, params Node[] _child) : base(_name)
    {
        m_children = _child;
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

}
