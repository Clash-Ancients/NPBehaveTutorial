using UnityEngine;
using UnityEngine.AI;
public class Tur3EnemyCtrl : TurBase
{
    public GameObject m_target;
    NavMeshAgent m_agent;
    void Start()
    {

        m_agent = m_target.GetComponent<NavMeshAgent>();
        
        m_Root = new Root(new Service(0.125f, UpdateBlackboard, new Selector(
            
                new BlackboardCondition("playerDistance", Operator.IS_LESS, 7.5f, STOPS.IMMEDIATE_RESTART, new Sequence(
                        
                    new Action(() => { Debug.Log("begin to chase");}),
                                new NavTo(),
                                new Action(() => { Debug.Log("successfully chased the target");})
                    )),
                
                new Sequence(
                        new Action(() => {Debug.Log("too far to chase");}),
                        new WaitUntilStopped()
                    )
            )));
        
        
        m_Root.Start();
    }
    
    void UpdateBlackboard()
    {
        var distance = Vector3.Distance(transform.position, m_target.transform.position);
        m_Root.Blackboard["playerDistance"] = distance;
    }
}
