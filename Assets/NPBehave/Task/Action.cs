
using UnityEngine;
public class Action : Task
{

    System.Action m_action;

    System.Func<bool, eRESULT> m_multiFrameAction;
    public Action(System.Action action) : base("Action")
    {
        m_action = action;
    }

    public Action(System.Func<bool, eRESULT> _action) : base("Action")
    {
        m_multiFrameAction = _action;
    }
    
    protected override void DoStart()
    {
        if (null != m_action)
        {
            m_action?.Invoke();
            Stopped(true);
        }
        else if (null != m_multiFrameAction)
        {
            m_rootNode.Clock.OnAddUpdateObserver(UpdateMultiFrameFunc1);
            UpdateMultiFrameFunc1();
        }
    }
    
    protected override void DoStop()
    {
        if (null != m_multiFrameAction)
        {
            var result = m_multiFrameAction?.Invoke(true);
            m_rootNode.Clock.OnRemoveUpdateObserver(UpdateMultiFrameFunc1);
            Stopped(result == eRESULT.eSUCCESS);
        }
        else
        {
            Debug.LogError("DoStop error logic");
        }
        
    }
    

    void UpdateMultiFrameFunc1()
    {
        var result = m_multiFrameAction?.Invoke(false);
    }
    
    
}
