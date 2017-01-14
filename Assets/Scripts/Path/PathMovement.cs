using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleTargetMovement))]
public class PathMovement : MonoBehaviour {

    public Path currPath;
    private int currPathIndex = 0;
    public float arrivalThreshold;

    // Use this for initialization
    void Start () {
        if (currPath)
        {
            setPath(currPath);
        }
	}

    public void setPath(Path path)
    {
        currPath = path;
        currPathIndex = 0;
        Transform point = currPath.GetPoint(currPathIndex);
        updateTargetPoint(point);
         
    }

    public void setPath(Path path, int index)
    {
        currPath = path;
        currPathIndex = index;
        Transform point = currPath.GetPoint(currPathIndex);
        updateTargetPoint(point);

    }

    public void updateTargetPoint(Transform point)
    {
        if (point)
        {
            GetComponent<SimpleTargetMovement>().SetTarget(point);
        }
        else
        {
            GetComponent<SimpleTargetMovement>().SetTarget(null);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (currPath)
        {
            Transform currTarget = currPath.GetPoint(currPathIndex);
            if (currTarget && (currTarget.position - transform.position).magnitude <= arrivalThreshold)
            {
                updateTargetPoint(currPath.GetPoint(++currPathIndex));
            }
        }
    }
}
