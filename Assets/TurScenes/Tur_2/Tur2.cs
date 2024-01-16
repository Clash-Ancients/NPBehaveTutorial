/*
 * 例子功能描述：每0.5秒切换打印内容(foo, bar)
 * 装饰节点1: Service - 定时修改黑板数据
 * 装饰节点2: BlackboardCondition - 检测指定黑板数据变化，并执行指定action
 * 组合节点: Sequence, Selector
 * 任务节点: WaitUntilStopped
 */
using UnityEngine;
public class Tur2 : TurBase
{
   
    Clock m_clock;
    void Start()
    {
        m_clock = new Clock();
        
        m_Root = new Root(m_clock, 
            
            new Service(0.5f,() => { m_Root.Blackboard["foo"] = !m_Root.Blackboard.Get<bool>("foo"); },
                
                        new Selector(
                            
                            new BlackboardCondition("foo", Operator.IS_EQUAL, true, STOPS.IMMEDIATE_RESTART, 
                                    
                                new Sequence(
                                            new Action(() => Debug.Log("foo")),
                                            new WaitUntilStopped()
                                        )
                                ),
                                new Sequence(
                                    new Action(() => Debug.Log("bar")),
                                    new WaitUntilStopped()
                                    )
                        )
                        
                )
        );
        
        m_Root.Start();
    }
}
