using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class NodePath : MonoBehaviour
{
    public Material current, open, closed;

    public TraversableNode _startNode, _endNode;

    private List<TraversableNode> _closedList = new List<TraversableNode>();
    private List<TraversableNode> _openList = new List<TraversableNode>();

    public int AddToSortedList(TraversableNode node, ref List<TraversableNode> sortedList)
    {
        for(int i = 0; i < sortedList.Count; i++)
        {
            if(node < sortedList[i])
            {
                sortedList.Insert(i, node);
                return i;
            }
        }

        sortedList.Add(node);
        return sortedList.Count;
    }

    [ContextMenu("Get Path")]
    public void BeginAStar()
    {
        StartCoroutine(AStar());
    }

    public IEnumerator AStar()
    {
        TraversableNode currentNode;
        currentNode = _startNode;
        _openList.Add(currentNode);

        while(_openList.Count > 0)
        {
            currentNode = _openList[0];
            currentNode.GetComponent<Renderer>().material = current;            
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);
            currentNode.GetComponent<Renderer>().material = closed;
            if(_closedList.Contains(_endNode))
            {
                    TraversableNode n = _endNode;
                    while(n != null)
                    {
                        n.GetComponent<Renderer>().material = current;
                        n = n._parentNode;
                        yield return null;
                    }
                    yield break;            
            }
            foreach(TraversableNode node in currentNode.GetNeighbors())
            {
                if(!_closedList.Contains(node))
                {
                    if(_openList.Contains(node))
                    {
                        var tentG = currentNode._gValue + node._travelCost;
                        if(tentG <= node._gValue)
                        {
                            node._parentNode = currentNode;
                            node.GetGValue();
                            break;
                        }
                    }
                    else
                    {                         
                        node._parentNode = currentNode;
                        node.GetGValue();
                        node._hValue = TraversableNode.Distance(node, _endNode);
                        AddToSortedList(node, ref _openList);
                        node.GetComponent<Renderer>().material = open;
                    }                
                }                            
            }            
        }

        yield return null;
    }

    public Stack<TraversableNode> NodeStackPath(TraversableNode endNode)
    {
        Stack<TraversableNode> returnStack = new Stack<TraversableNode>();

        TraversableNode currentNode = endNode;

        while(currentNode._parentNode != null)
        {
            returnStack.Push(currentNode);
            currentNode = currentNode._parentNode;
        }

        return returnStack;
    }
}