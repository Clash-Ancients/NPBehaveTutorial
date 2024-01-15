using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
   string m_name;

   protected Node m_parentNode;
   
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
      
   }

   public virtual void DoStop()
   {
      
   }
   
   
}
