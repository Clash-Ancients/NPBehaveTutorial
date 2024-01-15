public abstract class Decorator : Container
{
   protected Node m_decorator;
   
   public Decorator(string _name, Node _decorator) : base(_name)
   {
      m_decorator = _decorator;
      m_decorator.SetParent(this);
   }

   public override void SetRoot(Root rootNode)
   {
      base.SetRoot(rootNode);
      m_decorator.SetRoot(rootNode);
   }
   
}
