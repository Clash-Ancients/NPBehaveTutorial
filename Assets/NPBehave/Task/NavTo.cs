using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTo : Task
{

    public NavTo() : base("NavTo")
    {
        
    }

    protected override void DoStart()
    {
        
    }

    protected override void DoStop()
    {
        Stopped(true);
    }
    
    
    
}
