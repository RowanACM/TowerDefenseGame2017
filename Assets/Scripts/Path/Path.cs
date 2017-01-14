using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Path : MonoBehaviour {

    public Transform[] points;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform GetPoint(int index)
    {
        if(index < points.Length && index >= 0)
        {
            return points[index];
        }
        else
        {
            return null;
        }
    }

    public void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(points[0].transform.position, 1.0f);
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i] != null && points[i-1] != null)
            {
                Gizmos.DrawSphere(points[i].transform.position, 1.0f);
                DrawArrow.ForGizmo(points[i - 1].transform.position, points[i].transform.position, arrowHeadLength: 2.0f);
            }
        }
    }
}
