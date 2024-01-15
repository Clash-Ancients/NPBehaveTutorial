
public abstract class Node
{
   string m_name;

   protected Container m_parentNode;

   protected Root m_rootNode;

   public virtual void SetRoot(Root _root)
   {
      m_rootNode = _root;
   }

   public virtual void SetParent(Container _parent)
   {
      m_parentNode = _parent;
   }
   
   public Clock Clock => m_rootNode.GetRootClock();
   
   public Node(string _name)
   {
      m_name = _name;
   }

   protected virtual void DoStart()
   {
      
   }

   public virtual void Start()
   {

      DoStart();
   }

   public virtual void Stop()
   {
      DoStop();
   }

   protected virtual void DoStop()
   {
      
   }

   protected virtual void Stopped()
   {
      if (null != m_parentNode)
      {
         m_parentNode.ChildStopped(this);
      }  
   }
   
  
  
   
}
