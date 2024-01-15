public class Decorator : Container
{
    
    protected Node m_decoratee;
    
    public Decorator(string _name, Node _decoratee) : base(_name)
    {
        //TODO:
        //Debug.Assert(null != m_decoratee, "Derorator, input param _decoratee should not be null");
        
        m_decoratee = _decoratee;
        if (null != m_decoratee)
        {
            m_decoratee.SetParent(this);
        }
        
    }
    
    public override void SetRoot(Root _root)
    {
        base.SetRoot(_root);
        if (null != m_decoratee)
            m_decoratee.SetRoot(_root);
    }
    
}
