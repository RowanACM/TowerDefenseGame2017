using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Base Class for Targetting modes (Single-Target, Multi-Target)
 */
[RequireComponent(typeof(PriorityMode))]
public class SingleTargeter : Targeter {

    public Transform target;

    public override void FindTarget()
    {
        target = GetComponent<PriorityMode>().GetBest(GetNearbyTransforms());
    }

    public override Transform GetTargetTransform()
    {
        if (target)
        {
            return target.transform;
        } else {
            return null;
        }
    }
}
