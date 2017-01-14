using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanFire : FiringMode {

    public int damage;
    public float range;

    public override void FireWeapon()
    {
        RaycastHit hit;
        Physics.Raycast(projectileSpawn.position, projectileSpawn.forward, out hit, range);
        if (hit.collider)
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.GetComponent<Health>())
            {
                hitObject.GetComponent<Health>().TakeDamage(this.gameObject, damage);
            }
        }

    }
}
