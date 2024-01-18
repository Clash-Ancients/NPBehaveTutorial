using NPBehave;
using UnityEngine;
using UnityEngine.AI;
public class Tur3EnemyCtrl : TurBase
{
    public GameObject m_target;
    NavMeshAgent m_agent;
    void Start()
    {

        m_agent = gameObject.GetComponent<NavMeshAgent>();
        
        m_Root = new Root(new Service(0.125f, UpdateBlackboard, new Selector(
            
                new BlackboardCondition("playerDistance", Operator.IS_SMALLER, 7.5f, STOPS.IMMEDIATE_RESTART, new Sequence(
                        
                    new Action(() => { Debug.Log("begin to chase");}),
                                new NavTo(m_agent, "playerTrans", 1.5f),
                                new Action(() => { Debug.Log("successfully chased the target");})
                    )),
                
                new Sequence(
                        new Action(() => {Debug.Log("too far to chase");}),
                        new WaitUntilStopped()
                    )
            )));
        
        
        m_Root.Start();
        
           #if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = m_Root;
#endif
    }
    
    void UpdateBlackboard()
    {
        var distance = Vector3.Distance(transform.position, m_target.transform.position);
        m_Root.Blackboard["playerDistance"] = distance;
        m_Root.Blackboard["playerTrans"] = m_target.transform;
    }
}
