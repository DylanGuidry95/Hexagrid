using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversableNode : Node
{
    public TraversableNode _parentNode;

    public int _xCoord, _yCoord;

    public float _travelCost;
    public float _hValue;
    public float _gValue = 0;
    public float _fValue => (_hValue + _gValue);


    public float GetNeighboorTravelCost(TraversableNode aNode)
    {
        return CheckIsNeighbor(aNode) ? (aNode._travelCost - this._travelCost) : float.MaxValue;
    }

    public static float Distance(TraversableNode aNode, TraversableNode bNode)
    {
        float a = Mathf.Abs(aNode._xCoord - bNode._xCoord);
        float b = Mathf.Abs(aNode._yCoord - bNode._yCoord);

        //a *= a;
        //b *= b;

        return a + b;
    }

    public void GetGValue()
    {
        if(_parentNode == null)
            _gValue = 0;
        
        else
        {
            if(_parentNode._xCoord == _xCoord || _parentNode._yCoord == _yCoord)
                _travelCost = 10;
            else
                _travelCost = 14;
            _gValue = _travelCost + _parentNode._gValue;
        }
    }


    public static bool operator >(TraversableNode lhs, TraversableNode rhs)
    {
        return lhs._fValue > rhs._fValue;
    }
    public static bool operator <(TraversableNode lhs, TraversableNode rhs)
    {
        return lhs._fValue < rhs._fValue;
    }
}
