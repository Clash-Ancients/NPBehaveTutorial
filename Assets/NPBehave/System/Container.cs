
public abstract class Container : Node
{
    
    public void ChildStopped(Node child)
    {
        DoChildStopped(child);
    }
   
    protected abstract void DoChildStopped(Node child);
    
    public Container(string _name) : base(_name)
    {
    }
}
