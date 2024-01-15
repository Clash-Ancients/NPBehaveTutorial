using UnityEngine;

public class TurBase : MonoBehaviour
{
    protected Root m_Root;

    public virtual void OnGUI()
    {
        if (GUI.Button(new Rect(20f, 20f, 120f, 60), "Stop"))
        {
            m_Root.Stop();
        }
        else if (GUI.Button(new Rect(20f, 100f, 120f, 60), "Start"))
        {
            m_Root.Start();
        }

    }
}
