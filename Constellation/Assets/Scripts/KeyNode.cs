using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyNode : MonoBehaviour
{
    [SerializeField]
    private MirrorNode[] _mustHit;
    public bool _completed = false;

    [SerializeField]
    public GameObject[] hasBeenHitBy;
    [SerializeField]
    bool[] numbered;

    [SerializeField]
    public bool NeedsToBeHitBy;

    public void CheckCompletion()
    {
  
        if (!NeedsToBeHitBy)
        {
            Complete();

            for (int i = 0; i < _mustHit.Length; i++)
            {
                if (!_mustHit[i]._emitterEnabled)
                {
                    Incomplete();
                }
            }

            FindObjectOfType<LevelManager>()._keyPing.Invoke();
        }

        if (NeedsToBeHitBy)
        {
        
            Complete();

            for (int i = 0; i < numbered.Length; i++)
            {
                if (numbered[i] == false)
                {
                    Incomplete();
     
                }
            }
            FindObjectOfType<LevelManager>()._keyPing.Invoke();
        }

    }


    public void LaserToBehit(GameObject laserThatHItiT)
    {
    
        for (int i = 0; i < hasBeenHitBy.Length; i++)
        {
            Debug.Log(laserThatHItiT);
            if (laserThatHItiT == hasBeenHitBy[i])
            {
                numbered[i] = true;
            }
            Debug.Log("test");
        }
    }

    private void Complete()
    {
        _completed = true;
    }

    public void Incomplete()
    {
        _completed = false;
    }
}
