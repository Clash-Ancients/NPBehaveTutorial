public abstract class Node
{

   string m_Name;

   protected Root m_Root;

   protected Node m_parentNode;
   
   public Node(string _name)
   {
      m_Name = _name;
   }
   
   protected virtual void DoStart()
   {
      
   }
   
   public virtual void Start()
   {

      DoStart();
   }

   public virtual void SetRoot(Root _root)
   {
      m_Root = _root;
   }

   public void SetParent(Container parent)
   {
      m_parentNode = parent;
   }
   
}
