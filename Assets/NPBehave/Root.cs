using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Decorator
{

    protected Node m_mainNode;
    
    public Root(Node _mainnode) : base("Root", _mainnode)
    {
        m_mainNode = _mainnode;
    }
    
    protected override void DoStart()
    {
        m_mainNode.Start();
    }
    
}
