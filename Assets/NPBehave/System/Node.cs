
using UnityEngine;
public abstract class Node
{

   public enum eNodeState
   {
      eACTIVE,
      eUNACTIVE,
      eSTOP_REQUESTED,
   }

   private string label;

   public string Label
   {
      get
      {
         return label;
      }
      set
      {
         label = value;
      }
   }
   
   public string Name
   {
      get
      {
         return m_name;
      }
   }
   
   public bool IsActive => m_NodeState == eNodeState.eACTIVE;
   
   public bool IsStopRequested => m_NodeState == eNodeState.eSTOP_REQUESTED;
   
   protected eNodeState m_NodeState = eNodeState.eUNACTIVE;

   public eNodeState NodeState => m_NodeState;
   
   string m_name;

   protected Container m_parentNode;
   
   public Container ParentNode => m_parentNode;

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
      
      #if UNITY_EDITOR
      m_rootNode.TotalNumStartCalls++;
      this.DebugNumStartCalls++;
#endif
      
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
      
      #if UNITY_EDITOR
      m_rootNode.TotalNumStopCalls++;
      this.DebugLastStopRequestAt = UnityEngine.Time.time;
      this.DebugNumStopCalls++;
#endif
      
      DoStop();
   }

   protected virtual void DoStop()
   {
      
   }

   protected virtual void Stopped(bool success)
   {
      m_NodeState = eNodeState.eUNACTIVE;
      if (null != m_parentNode)
      {
         m_parentNode.ChildStopped(this, success);
      }  
   }
   
   #if UNITY_EDITOR
   public float DebugLastStopRequestAt = 0.0f;
   public float DebugLastStoppedAt = 0.0f;
   public int DebugNumStartCalls = 0;
   public int DebugNumStopCalls = 0;
   public int DebugNumStoppedCalls = 0;
   public bool DebugLastResult = false;
#endif
  
  
   
}
