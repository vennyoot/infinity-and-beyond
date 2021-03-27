using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour
{
    private LineRenderer _lineRender;
    private List<Vector2> _lineLink = new List<Vector2>();
    private List<GameObject> _currentNodes = new List<GameObject>();

    private float _range = 10f;

    private void Start()
    {
        _lineRender = GetComponent<LineRenderer>();
        ResetLight();
    }

    private void Update()
    {
        RaycastHit2D[] hit = Physics2D.LinecastAll(transform.position, transform.position + transform.up * _range, LayerMask.GetMask("MirrorNode", "KeyNode"));
        
        if (hit.Length > 1)
        {
            if (hit[1].collider.gameObject.layer == 20 && !hit[1].collider.gameObject.GetComponent<MirrorNode>()._emitterEnabled)
            {
                ClearLinks();
                HitNode(hit[1]);
                hit[1].collider.gameObject.GetComponent<MirrorNode>().EnableEmitter();
            }

            if (hit[1].collider.gameObject.layer == 21)
            {
                ClearLinks();
                HitNode(hit[1]);
                hit[1].collider.gameObject.GetComponent<KeyNode>().CheckCompletion();
            }
        }
        else
        {
            DisableNextNode();
            ResetLight();
        }

        if (_currentNodes.Count > 0)
        {
            ClearLinks();
            AddLink(_currentNodes[0].transform.position);
        }
        else
        {
            ResetLight();
        }
    }

    public void DisableNextNode()
    {
        for (int i = 0; i < _currentNodes.Count; i++)
        {
            if (_currentNodes[i].GetComponent<MirrorNode>())
            {
                _currentNodes[i].GetComponent<MirrorNode>().DisableEmitter();
            }

            if (_currentNodes[i].GetComponent<KeyNode>())
            {
                _currentNodes[i].GetComponent<KeyNode>().Incomplete();
            }
        }
    }

    private void AddLink(Vector2 pos)
    {
        _lineLink.Add(pos);
        _lineRender.positionCount = _lineLink.Count;
        _lineRender.SetPosition(_lineLink.Count - 1, _lineLink[_lineLink.Count - 1]);
    }

    private void ResetLight()
    {
        ClearLinks();
        ClearCurrentNodes();
        AddTail(transform);
    }

    private void ClearCurrentNodes()
    {
        _currentNodes.Clear();
    }

    private void ClearLinks()
    {
        _lineLink.Clear();
        AddLink(transform.position);
    }

    private void HitNode(RaycastHit2D hit)
    {
        if (!_currentNodes.Contains(hit.collider.gameObject))
        {
            _currentNodes.Add(hit.collider.gameObject);
        }

        for (int i = 0; i < _currentNodes.Count; i++)
        {
            AddLink(_currentNodes[i].transform.position);
        }
    }

    private void AddTail(Transform transform)
    {
        AddLink(transform.position + transform.up * _range);
    }

    private void ReflectLight() //needs to happen after clearlinks/hitnode
    {
        Transform temp = _currentNodes[_currentNodes.Count - 1].transform;

        AddLink(temp.position);
        AddTail(temp);
    }
}
