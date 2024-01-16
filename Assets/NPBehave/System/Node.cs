
using UnityEngine;
public abstract class Node
{

   public enum eNodeState
   {
      eACTIVE,
      eUNACTIVE,
      eSTOP_REQUESTED,
   }

   protected eNodeState m_NodeState = eNodeState.eUNACTIVE;
   
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

      Debug.AssertFormat(m_NodeState == eNodeState.eUNACTIVE, $"node:{m_name} state should be un active");

      if (m_NodeState != eNodeState.eUNACTIVE)
      {
         return;
      }
      
      m_NodeState = eNodeState.eACTIVE;
      DoStart();
   }

   public virtual void Stop()
   {
      
      Debug.AssertFormat(m_NodeState == eNodeState.eACTIVE, $"node:{m_name}, only active node can stop");

      if (m_NodeState != eNodeState.eACTIVE)
      {
         return;
      }
      
      m_NodeState = eNodeState.eSTOP_REQUESTED;
      DoStop();
   }

   protected virtual void DoStop()
   {
      
   }

   protected virtual void Stopped()
   {
      m_NodeState = eNodeState.eUNACTIVE;
      if (null != m_parentNode)
      {
         m_parentNode.ChildStopped(this);
      }  
   }
   
  
  
   
}
