public class Decorator : Node
{
   protected Node m_decorator;

   
   public Decorator(string _name, Node _decorator) : base(_name)
   {
      m_decorator = _decorator;
      
   }
}
