using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : AimMode {

    public Transform gun;
    public Transform axle;
    private Transform target;
    public float aimSpeed;

    public override void AimAt(Transform target)
    {
        this.target = target;
    }

    public void Update()
    {
        if (target)
        {
            Vector3 flatAxleTarget = Vector3.Scale(target.position, new Vector3(1, 0, 1)) + new Vector3(0, axle.transform.position.y, 0);
            Quaternion lookRot = Quaternion.LookRotation(flatAxleTarget - axle.position);
            axle.rotation = Quaternion.Slerp(axle.rotation, lookRot, aimSpeed * Time.deltaTime);

            Vector3 verticalTarget = target.position - gun.position;
            float yVal = verticalTarget.y;
            verticalTarget.y = 0;
            verticalTarget = new Vector3(0.0f, yVal, verticalTarget.magnitude);
            lookRot = Quaternion.LookRotation(verticalTarget);
            
            gun.localRotation = Quaternion.Slerp(gun.localRotation, lookRot, aimSpeed * Time.deltaTime * (Quaternion.Angle(gun.localRotation, lookRot) / 180));

        }
    }
}
