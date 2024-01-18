using UnityEngine;
using UnityEngine.AI;
public class NavTo : Task
{

    NavMeshAgent m_agent;
    string m_playerkey;
    Transform m_playerTrans;
    float m_tolerance;
    public NavTo(NavMeshAgent agent, string _playerkey, float _tolerance) : base("NavTo")
    {
        m_agent = agent;
        m_playerkey = _playerkey;
        m_tolerance = _tolerance;
    }

    protected override void DoStart()
    {
        //定时更新
        m_rootNode.Clock.AddTimer(0.125f, -1, UpdateNavTo);
        
        //事件注册 : 如果黑板数据发生改变，通知navmeshagent
        m_rootNode.Clock.OnAddUpdateObserver(UpdateNavTo);

        UpdateNavTo();
    }
    
    void UpdateNavTo()
    {
        if (null == m_agent)
        {
            StopNavTo(false);
            return;
        }

        var target = m_rootNode.Blackboard[m_playerkey];

        if (null == target)
        {
            StopNavTo(false);
            return;
        }

        //get target
        if (target is Transform && null == m_playerTrans)
        {
            m_playerTrans = (Transform)target;
        }
        else if (target is Vector3)
        {
            
        }
        else if(target is not Vector3 && target is not Transform)
        {
            Debug.LogError("target type is not correct, type: " + target.GetType());
            StopNavTo(false);
            return;
        }
        
        if (null == m_playerTrans)
        {
            Debug.LogError("null == m_playerTrans ");
            StopNavTo(false);
            return;
        }

        var dis = Vector3.Distance(m_playerTrans.transform.position, m_agent.transform.position);
        
        if (m_tolerance >= dis)
        {
            StopNavTo(true);
            return;
        }
        
        m_agent.destination = m_playerTrans.transform.position;

        
        

    }

    protected override void DoStop()
    {
        StopNavTo(false);
    }
    
    void StopNavTo(bool success)
    {
        m_agent.destination = m_agent.transform.position;
        m_rootNode.Clock.RemoveTimer(UpdateNavTo);
        m_rootNode.Clock.OnRemoveUpdateObserver(UpdateNavTo);
        Stopped(success);
    }
}
