using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour {

    public float dragHeight;
    public float visibilityRadius;

    public SpawnCell spawnCell;

    private Vector3 hitPosition;
    private Collider placementVolume;
    private TimedTargetMovement movement;
    private Transform moveTarget;
    public float magicTime;

    public enum PlacementState
    {
        Begin, StartDrag, Dragged, Placed, Invalid
    }

    private PlacementState placementState;

	// Use this for initialization
	void Start () {
        movement = this.gameObject.AddComponent<TimedTargetMovement>();
        movement.movementTime = magicTime;
        movement.shouldOrientWithGround = false;
        movement.shouldFaceTarget = false;
        moveTarget = new GameObject("MoveTarget").transform;
        moveTarget.position = this.transform.position;
        movement.SetTarget(moveTarget);
        movement.fixedTime = false;
        placementState = PlacementState.Begin;
        placementVolume = GameObject.Find("PlacementVolume").GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        SpawnCell closest;
        
        switch (placementState)
        {
            case PlacementState.Begin:
                spawnCell = GetClosestCell();
                if (placementVolume.bounds.Contains(hitPosition)) {
                    moveTarget.position = hitPosition + new Vector3(0.0f, dragHeight, 0.0f);
                    if (Input.GetMouseButtonUp(0) && spawnCell)
                    {
                        OnMouseUp();
                    }
                }

                break;
            case PlacementState.StartDrag:
                closest = GetClosestCell();
                if(closest != spawnCell && placementVolume.bounds.Contains(hitPosition))
                {
                    spawnCell.SetItem(null);
                    spawnCell = closest;
                    placementState = PlacementState.Dragged;
                    SendMessage("OnDrag", SendMessageOptions.DontRequireReceiver);
                    Vector3 targetPosition = hitPosition + new Vector3(0.0f, dragHeight, 0.0f);
                    moveTarget.position = targetPosition;
                }
                break;
            case PlacementState.Dragged:
                spawnCell = GetClosestCell();
                moveTarget.position = hitPosition + new Vector3(0.0f, dragHeight, 0.0f);
                break;
            case PlacementState.Placed:
                break;
            case PlacementState.Invalid:
                if (CanPlace())
                {
                    placementState = PlacementState.Placed;
                    moveTarget.position = spawnCell.transform.position;
                    SendMessage("OnPlace", SendMessageOptions.DontRequireReceiver);
                }
                break;
            default:
                break;
        }
	}

    public void OnMouseUp()
    {
        switch (placementState)
        {
            case PlacementState.StartDrag:
                placementState = PlacementState.Placed;
                SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                break;
            case PlacementState.Begin:
            case PlacementState.Dragged:
                if (CanPlace())
                {
                    placementState = PlacementState.Placed;
                    moveTarget.position = spawnCell.transform.position;
                    spawnCell.SetItem(this.gameObject);
                    SendMessage("OnPlace", SendMessageOptions.DontRequireReceiver);
                } else
                {
                    placementState = PlacementState.Invalid;
                    SendMessage("OnInvalid", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case PlacementState.Placed:
                //This won't happen
                break;
            case PlacementState.Invalid:
                //This also won't happen
                break;
            default:
                break;
        }
    }

    public void OnMouseDown()
    {
        switch (placementState)
        {
            case PlacementState.Dragged:
                //Clicking while dragging? This shouldn't happen
                break;
            case PlacementState.Placed:
                placementState = PlacementState.StartDrag;

                break;
            case PlacementState.Invalid:
                placementState = PlacementState.Dragged;
                SendMessage("OnDrag", SendMessageOptions.DontRequireReceiver);
                break;
            default:
                break;
        }
    }

    public bool IsPlaced()
    {
        return placementState == PlacementState.Placed || placementState == PlacementState.StartDrag;
    }

    public bool CanPlace()
    {
        Purchaseable purchase = GetComponent<Purchaseable>();
        if (purchase)
        {
            if (spawnCell)
                return (purchase.WasPurchased() || purchase.CanBePurchased()) && spawnCell.IsOpen();
            else
                return false;
        } else if (spawnCell)
        {
            return spawnCell.IsOpen();
        }else
        {
            return false;
        }
        //return spawnCell && (purchase && purchase.WasPurchased() && spawnCell.IsOpen()) || (!purchase && spawnCell.IsOpen());
    }

    private SpawnCell GetClosestCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(ray, out hit, maxDistance: 300.0f, layerMask: layerMask))
        {
            hitPosition = hit.point;
            if (!placementVolume.bounds.Contains(hitPosition))
            {
                hitPosition = placementVolume.bounds.ClosestPoint(hitPosition);
            }
            //now we'll look for objects within a radius, and find the closest object with CellScript
            SpawnCell closestCell = null;
            float closestDist = visibilityRadius + 1.0f;
            Collider[] hitColliders = Physics.OverlapSphere(hitPosition, visibilityRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                SpawnCell foundCell = hitColliders[i].GetComponent<SpawnCell>();
                if (foundCell != null)
                {
                    float dist = (foundCell.transform.position - hitPosition).magnitude;
                    if (dist < closestDist)
                    {
                        closestCell = foundCell;
                        closestDist = dist;
                    }
                }
            }
            return closestCell;
        }
        return null;
    }



}
