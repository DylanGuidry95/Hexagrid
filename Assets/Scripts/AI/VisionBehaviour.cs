using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBehaviour : MonoBehaviour
{
    public float ViewDistance;
    public float ViewAngle;
    public float AngleChecks;
    public bool ToggleGizmos;
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
        if (Rays == null)
            return;
        foreach(var r in Rays)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, r.direction, out hit, ViewDistance))
            {
                if (hit.transform.GetComponent<TraversableNode>())
                {
                    var n = hit.transform.GetComponent<TraversableNode>();
                    Debug.Log(hit.collider.name);
                    n.isOccupied = true;
                }
            }
        }
    }

    [ContextMenu("Gen Cone")]
    void GenCone()
    {

        Rays = new List<Ray>();
        AngleChecks = AngleChecks < 1 ? 1 : AngleChecks;
        for (float j = -2; j <= 2; j += 0.25f)
        {
            for (float i = -ViewAngle; i < ViewAngle; i += 120)
            {
                Quaternion spread = Quaternion.AngleAxis(i, transform.up);
                Vector3 newVec = spread * transform.forward;
                Rays.Add(new Ray(transform.position + new Vector3(0,j,0), newVec));
            }
        }

        HighlightHex();
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawSphere(transform.position, 1);
        if(Rays == null)
            return;
        foreach (var r in Rays)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, r.direction, out hit, ViewDistance))
            {
                if (hit.transform.GetComponent<TraversableNode>())
                {
                    Gizmos.DrawRay(r.origin, r.direction * ViewDistance);
                    var n = hit.transform.GetComponent<TraversableNode>();
                    Gizmos.DrawCube(hit.point, new Vector3(1, 1, 1));
                }
            }
        }

    }
}
