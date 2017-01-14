using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimMode))]
[RequireComponent(typeof(FiringMode))]
[RequireComponent(typeof(Targeter))]
public class TowerController : MonoBehaviour {

    public float firingRate;
    private bool isIdle;
    private IEnumerator firingLoop;
	// Use this for initialization
	void Start () {
        isIdle = true;
        firingLoop = FiringLoop();
        StartCoroutine(firingLoop);
    }

    // Update is called once per frame
    void Update() {
        if (!isIdle)
        {
            Targeter targeter = GetComponent<Targeter>();
            targeter.FindTarget();
            GetComponent<AimMode>().AimAt(targeter.GetTargetTransform());
        }
	}

    public void OnIdle()
    {
        this.isIdle = true;
        StopCoroutine(firingLoop);
        GetComponent<AimMode>().AimAt(null);
    }

    public void OnDeIdle()
    {
        this.isIdle = false;
        StartCoroutine(firingLoop);
        GetComponent<AimMode>().AimAt(GetComponent<Targeter>().GetTargetTransform());
    }

    private IEnumerator FiringLoop()
    {
        while (true)
        {
            if (!isIdle && GetComponent<Targeter>().GetTargetTransform())
            {
                GetComponent<FiringMode>().FireWeapon();
            }
            yield return new WaitForSeconds(firingRate);
        }
    }
}
