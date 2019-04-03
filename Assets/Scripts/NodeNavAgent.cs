﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavAgent : MonoBehaviour
{
    public int scanRange;
    public Material pathMaterial, rangeMaterial;


    private TraversableNode _currentPositionNode;
    public TraversableNode currentPositionNode => (_currentPositionNode);

    private TraversableNode _goalPositionNode;
    public TraversableNode goalPositionNode => (_goalPositionNode);

    private Stack<TraversableNode> _nodePathStack;

    public bool hasPath => (_nodePathStack == null && _nodePathStack.Count > 0);

    private bool _autoRepath;
    public bool autoRepath
    {
        get { return _autoRepath; }
        set { _autoRepath = value; }
    }

    public float remainingDistance => TraversableNode.Distance(currentPositionNode, goalPositionNode);



    private void Update()
    {
        ClickSetPath();
        TraversePath();
        VeggieJump();
    }


    private void TraversePath()
    {
        if(_nodePathStack != null && _nodePathStack.Count > 0)
        {
            Vector3 dir = (_nodePathStack.Peek().transform.position - transform.position);

            dir.Normalize();
            transform.Translate(dir * 2.5f * Time.deltaTime);

            if(Vector3.Distance(transform.position, _nodePathStack.Peek().transform.position) <= 0.05f)
            {
                _currentPositionNode = _nodePathStack.Pop();
                _currentPositionNode.GetComponent<Renderer>().material = pathMaterial;
            }

            if(_nodePathStack.Count == 0)
            {
                foreach(Node n in _currentPositionNode.GetNeighborhood(scanRange))
                {
                    n.GetComponent<Renderer>().material = rangeMaterial;
                }
                _nodePathStack = null;
            }
        }
    }

    private void VeggieJump()
    {
        if(_nodePathStack != null)
        {
            TraversableNode nextNode = _nodePathStack.Peek();
            float dist = Vector3.Distance(transform.position, nextNode.transform.position);

            transform.GetChild(0).transform.localPosition =
            Vector3.up + (nextNode.transform.up * (Mathf.Sin(dist * 3.14f)));
        }
    }



    private void ClickSetPath()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            TraversableNode tn = hit.collider.GetComponent<TraversableNode>();
            if(tn == null) return;

            else if(Input.GetMouseButtonDown(1))
            {   _nodePathStack = null;
                _currentPositionNode = tn;
                transform.position = currentPositionNode.transform.position;
            }

            else if(Input.GetMouseButtonDown(0))
            {
                if(_currentPositionNode != null)
                {
                    _nodePathStack = NodeNav.TwinStarII(currentPositionNode, tn, true);
                }
            }
        }
    }
}