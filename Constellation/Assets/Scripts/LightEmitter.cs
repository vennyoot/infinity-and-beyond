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
        RaycastHit2D[] hit = Physics2D.LinecastAll(transform.position, transform.position + transform.up * _range, LayerMask.GetMask("MirrorNode", "LightNode"));
        
        if (hit.Length > 1)
        {
            ClearLinks();
            HitNode(hit[1]);

            if (hit[1].collider.gameObject.layer == 9)
            {
                var temp = hit[1].collider.gameObject.GetComponent<MirrorNode>();

                if (!temp._emitterEnabled)
                {
                    hit[1].collider.gameObject.GetComponent<MirrorNode>().EnableEmitter();
                }
            }
        }

        if (hit.Length <= 1)    //hitting itself only
        {
            for (int i = 0; i < _currentNodes.Count; i++)
            {
                _currentNodes[i].GetComponent<MirrorNode>().DisableEmitter();
            }

            ResetLight();
        }
        
        /*if (hit && hit.collider.gameObject.tag != "Emitting")
        {
            ClearLinks();
            HitNode(hit);

            if (hit.collider.gameObject.layer == 8)
            {
                //ReflectLight();

                var temp = hit.collider.gameObject.GetComponent<MirrorNode>();

                if (!temp._emitterEnabled)
                {
                    hit.collider.gameObject.GetComponent<MirrorNode>().EnableEmitter();
                }
            }
        }
        else
        {
            for (int i = 0; i < _currentNodes.Count; i++)
            {
                _currentNodes[i].GetComponent<MirrorNode>().DisableEmitter();
            }

            ResetLight();
        }*/
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
