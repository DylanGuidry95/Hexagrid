using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBehaviour : MonoBehaviour
{
    public float ViewDistance;
    public float ViewAngle;
    public float AngleChecks;

    private List<Ray> Rays;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HighlightHex()
    {
        foreach(var r in Rays)
        {
            
        }
    }

    [ContextMenu("Gen Cone")]
    void GenCone()
    {
        Debug.Log("hit");
        Rays = new List<Ray>();
        AngleChecks = AngleChecks < 1 ? 1 : AngleChecks;
        for (float i = -ViewAngle; i < ViewAngle; i += AngleChecks)
        {
            Quaternion spread = Quaternion.AngleAxis(i, transform.up);            
            Vector3 newVec = spread * transform.forward;
            Rays.Add(new Ray(transform.position, newVec));
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawSphere(transform.position, 1);
        if(Rays == null)
            return;
        foreach(var r in Rays)
        {
            Gizmos.DrawRay(transform.position, r.direction * ViewDistance);
        }
    }
}
