using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorNode : MonoBehaviour
{
    [SerializeField]
    private GameObject _emitterPrefab;

    public bool _emitterEnabled;
    private GameObject _emitter;

    private void Start()
    {
        if (_emitterEnabled)
        {
            EnableEmitter();
        }
        else
        {
            DisableEmitter();
        }
    }

    public void EnableEmitter()
    {
        gameObject.tag = "Emitting";
        _emitter = Instantiate(_emitterPrefab, transform);
        _emitterEnabled = true;
    }

    public void DisableEmitter()
    {
        gameObject.tag = "Untagged";
        
        if (GetComponentInChildren<LightEmitter>())
        {
            GetComponentInChildren<LightEmitter>().DisableNextEmitter();
        }

        Destroy(_emitter);
        _emitterEnabled = false;
    }

    //TODO: Rotate left on command, rotate right on command, use forward as light guide (z axis)
}
