using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyNode : MonoBehaviour
{
    [SerializeField]
    private MirrorNode[] _mustHit;
    public bool _completed = false;

    public void CheckCompletion()
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

    private void Complete()
    {
        _completed = true;
    }

    public void Incomplete()
    {
        _completed = false;
    }
}
