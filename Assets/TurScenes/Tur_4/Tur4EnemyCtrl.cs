using NPBehave;
using UnityEngine;


public class Tur4EnemyCtrl : TurBase
{

    Blackboard m_sharedBlackboard;

    Blackboard m_blackboard;

    public Transform m_player;

    float m_engageDis = 7.5f;

    int m_maxEngagedNum = 2;
    
    void Start()
    {
        var clock = UnityContext.GetClock();

        m_sharedBlackboard = UnityContext.GetSharedBlackboard("group enemy ai");

        m_blackboard = new Blackboard(m_sharedBlackboard, clock);

        m_Root = new Root(m_blackboard, new Service(0.125f, UpdateEnemyBehaveData, new Selector(
                
                new BlackboardCondition("engaged", Operator.IS_EQUAL, true, STOPS.IMMEDIATE_RESTART, new Sequence(
                    new Action(() => {Debug.Log("player engaged");}),
                                new Action((shoulcancel) =>
                                {
                                    if (!shoulcancel)
                                    {
                                        MoveToward();
                                        return eRESULT.ePROCESS;
                                    }
                                    
                                    return eRESULT.eFAILED;
                                })
                    )),
                    
                new Selector(
                        new BlackboardCondition("playerInRange", Operator.IS_EQUAL, true, STOPS.BOTH, new Sequence(
                            new Action(() => {Debug.Log("player in range, but not engaged");}),
                            new WaitUntilStopped()
                            )),
                            new Sequence(
                                    new Action(() => {Debug.Log("player not in range.");}),
                                    new WaitUntilStopped()
                                )
                    )
            )));
        
        
        m_Root.Start();
    }


    void UpdateEnemyBehaveData()
    {
        
        // float m_engageDis = 7.5f;
        //
        // int m_maxEngagedNum = 2;
        
        var isInRange =Vector3.Distance(transform.position, m_player.transform.position) < m_engageDis;

        var isEngaged = m_blackboard.Get<bool>("playerInRange");

        var engagedNum = m_sharedBlackboard.Get<int>("sharedEngagedNum");

        //engaged : 在范围内，当前不是engaged && 当前总共engaged数量 < 2
        if (isInRange && !isEngaged && engagedNum < m_maxEngagedNum)
        {
            m_sharedBlackboard["sharedEngagedNum"] = m_sharedBlackboard.Get<int>("sharedEngagedNum") + 1;

            m_blackboard["playerInRange"] = isInRange;

            m_blackboard["engaged"] = !isEngaged;
        }

        if (!isInRange && isEngaged)
        {
            m_blackboard["playerInRange"] = false;
            m_sharedBlackboard["sharedEngagedNum"] = m_sharedBlackboard.Get<int>("sharedEngagedNum") - 1;
            m_blackboard["engaged"] = false;
        }
        
        //playerInRange
    }

    void MoveToward()
    {
        var dis = Vector3.Distance(transform.position, m_player.transform.position);

        if (dis < 1f)
        {
            return;
        }
        
        var dir = ( m_player.transform.position - transform.position).normalized;

        transform.position += 1.5f*dir*Time.deltaTime;
    }
    
}
