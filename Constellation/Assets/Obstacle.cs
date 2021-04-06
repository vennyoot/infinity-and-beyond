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
        for (int i = 0; i < hasBeenHitBy.Length; i++)
        {
            hasBeenHitBy[i].GetComponentInChildren<LightEmitter>().ResetLight();
        }
        

        Destroy(this.gameObject);
    }

}
