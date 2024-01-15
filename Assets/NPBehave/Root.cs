
public class Root : Decorator
{

    protected Node m_mainNode;
    
    public Root(string _name, Node _mainNode) : base(_name, _mainNode)
    {
        m_mainNode = _mainNode;
        SetRoot(this);
    }
    
    public override void SetRoot(Root _root)
    {
        base.SetRoot(_root);
        m_mainNode.SetRoot(_root);
    }

    protected override void DoStart()
    {
        m_mainNode.Start();
    }
    
}
