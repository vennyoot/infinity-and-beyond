using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : KeyNode
{



    private void Update()
    {
        if (_completed) 
        {
            PleaseKillYourSelf();
        }
    }

    public void PleaseKillYourSelf()
    {
        Destroy(this.gameObject);
    }

}
