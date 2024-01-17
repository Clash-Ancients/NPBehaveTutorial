
public abstract class Container : Node
{
    
    private bool collapse = false;
    public bool Collapse
    {
        get
        {
            return collapse;
        }
        set
        {
            collapse = value;
        }
    }
    
    public void ChildStopped(Node child, bool success)
    {
        DoChildStopped(child, success);
    }
   
    protected abstract void DoChildStopped(Node child, bool success);
    
    public Container(string _name) : base(_name)
    {
    }
    
    
#if UNITY_EDITOR
    public abstract Node[] DebugChildren
    {
        get;
    }
#endif
    
}
