
public abstract class Container : Node
{
    
    public void ChildStopped(Node child, bool success)
    {
        DoChildStopped(child, success);
    }
   
    protected abstract void DoChildStopped(Node child, bool success);
    
    public Container(string _name) : base(_name)
    {
    }
}
