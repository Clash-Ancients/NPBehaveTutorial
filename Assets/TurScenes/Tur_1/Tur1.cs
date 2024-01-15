using System;
using UnityEngine;

public class Tur1 : TurBase
{
    Service m_mainTree;
    public float TimeFrequency = 1f;
    float m_leftTime = 0f;
    Clock m_clock;
    void Start()
    {
        m_mainTree = new Service(() => { Debug.Log(1); }, null);
        m_clock = new Clock();
        m_Root = new Root(m_clock, m_mainTree);
        
        m_Root.Start();
    }
    
    void Update()
    {
        if (m_leftTime >= TimeFrequency)
        {
            m_leftTime -= TimeFrequency;
            m_clock.Update(Time.deltaTime);
        }
        m_leftTime += Time.deltaTime;
    }

}
