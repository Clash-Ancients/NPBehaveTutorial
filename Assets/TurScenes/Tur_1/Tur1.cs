using UnityEngine;

public class Tur1 : MonoBehaviour
{
    Service m_mainTree;
    Root m_rootNode;
    // Start is called before the first frame update
    void Start()
    {
        m_mainTree = new Service(() => { Debug.Log("called"); }, null);

        m_rootNode = new Root("root", m_mainTree);
        
        m_rootNode.Start();
    }

    
}
