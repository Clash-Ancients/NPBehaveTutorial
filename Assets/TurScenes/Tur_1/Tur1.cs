using UnityEngine;

public class Tur1 : MonoBehaviour
{
    Service m_mainTree;
    void Start()
    {
        m_mainTree = new Service(() => { Debug.Log(1); }, null);

        var root = new Root(m_mainTree);
        
        root.Start();
    }

   
}
