using UnityEngine;

public class Tur1 : TurBase
{
    Service m_mainTree;
    void Start()
    {
        m_mainTree = new Service(() => { Debug.Log(1); }, null);

        m_Root = new Root(m_mainTree);
        
        m_Root.Start();
    }

   
}
