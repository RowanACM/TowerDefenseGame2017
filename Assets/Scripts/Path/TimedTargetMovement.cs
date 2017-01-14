using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTargetMovement : MonoBehaviour {

    protected Transform target;
    protected bool hasReachedTarget;
    protected float remainingMovementTime;
    protected Vector3 lastTargetPosition;

    public bool shouldFaceTarget;
    public bool shouldOrientWithGround;
    public float movementTime;
    public bool fixedTime;

    /// <summary>
    /// Awake method initializes this components internal values BEFORE the Start method of any object is called. Do not change this!
    /// </summary>
    public void Awake()
    {
        hasReachedTarget = true;
        target = null;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        if (target != null)
        {
            remainingMovementTime = movementTime;
            hasReachedTarget = false;
        }
    }

    public bool IsMoving()
    {
        return !hasReachedTarget;
    }

    public bool HasReachedTarget()
    {
        return hasReachedTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            //once the time is up and we have reached a target, we will continuously lock to its position as if it is our parent
            if (hasReachedTarget && !(!fixedTime && (target.position != lastTargetPosition)))
            {
                this.transform.position = target.position;
            }
            else
            {
                Vector3 diff = target.position - transform.position;
                if (!fixedTime && (target.position != lastTargetPosition))
                {
                    //if the difference in position is greater than the last remaining distance we recorded, change some values up.
                    remainingMovementTime = movementTime;
                }
                remainingMovementTime -= Time.deltaTime;
                //Debug.Log(remainingMovementTime);
                if (remainingMovementTime <= 0)
                {
                    remainingMovementTime = 0.0000001f;
                }
                float speed = diff.magnitude / remainingMovementTime;
                Vector3 movement = diff.normalized * speed * Time.deltaTime;
                if (movement.magnitude > diff.magnitude)
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
            lastTargetPosition = target.position;
        }
    }
}
