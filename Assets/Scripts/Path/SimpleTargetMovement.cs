using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTargetMovement : MonoBehaviour {

    protected Transform target;
    private bool hasReachedTarget;

    public bool shouldFaceTarget;
    public bool shouldOrientWithGround;
    public float speed;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public bool HasReachedTarget() {
        return hasReachedTarget;
    }
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            Vector3 diff = target.position - transform.position;
            Vector3 movement = diff.normalized * speed * Time.deltaTime;
            if(movement.magnitude > diff.magnitude)
            {
                hasReachedTarget = true;
                movement = diff;
            }
            else
            {
                hasReachedTarget = false;
            }
            RaycastHit hit;
            if (shouldOrientWithGround)
            {
                if (Physics.Raycast(transform.position, -transform.up, out hit, maxDistance: 5.0f, layerMask: LayerMask.NameToLayer("ground")))
                {
                    transform.up = hit.normal;
                }
                
            }
            if (shouldFaceTarget)
            {
                transform.forward = diff.normalized;
            }
            

            this.transform.position += movement;
        }
	}
}
