using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EditorShowVolumeBounds : MonoBehaviour {

    private void OnDrawGizmos()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(this.transform.position + collider.center, collider.size);
    }
}
