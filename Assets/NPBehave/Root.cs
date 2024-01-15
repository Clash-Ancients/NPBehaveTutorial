using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Decorator
{

    protected Node m_mainNode;

    protected Clock m_clock;

    public Clock GetRootClock()
    {
        return m_clock;
    }
    
    public Root(Clock _clock, Node _mainnode) : base("Root", _mainnode)
    {
        m_mainNode = _mainnode;
        m_clock = _clock;
        SetRoot(this);
    }

    public override void SetRoot(Root _root)
    {
        base.SetRoot(_root);
        m_mainNode.SetRoot(_root);
    }
    
    protected override void DoStart()
    {
        m_mainNode.Start();
    }
    
}
