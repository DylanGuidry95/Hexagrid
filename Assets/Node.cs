using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    private List<Node> _neighbors = new List<Node>();

    public Node[] GetNeighbors()
    {
        Node[] myNeighbors = _neighbors.ToArray();
        return myNeighbors;
    }

    public int AddNeighbor(Node newNeighbor)
    {
        if(newNeighbor == null || _neighbors.Contains(newNeighbor))
            return -1;
        
        _neighbors.Add(newNeighbor);                

        return 0;
    }

    public bool CheckIsNeighbor(Node aNode)

    {
        return _neighbors.Contains(aNode);
    }


    public int ClearNeighbors()
    {
        foreach(Node n in _neighbors)
            n.RemoveNeighbor(this);
        
        _neighbors.Clear();

        return 0;
    }

    public int RemoveNeighbor(Node oldNeighbor)
    {
        if(_neighbors.Contains(oldNeighbor))
        {
            _neighbors.Remove(oldNeighbor);
            oldNeighbor.RemoveNeighbor(this);
        }

        return 0;
    }
}