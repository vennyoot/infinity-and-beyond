using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static KeyNode[] _keys;
    public UnityEvent _keyPing;

    private bool _levelDone = false;

    private void Start()
    {
        _keys = FindObjectsOfType<KeyNode>();
        _keyPing.AddListener(CheckLevelCompletion);
    }

    public void CheckLevelCompletion()
    {
        _levelDone = true;

        for (int i = 0; i < _keys.Length; i++)
        {
            if (!_keys[i]._completed)
            {
                _levelDone = false;
            }
        }

        if (_levelDone)
        {
            //LEVEL COMPLETE
            Debug.Log("LEVEL COMPLETE");
        }
    }
}
