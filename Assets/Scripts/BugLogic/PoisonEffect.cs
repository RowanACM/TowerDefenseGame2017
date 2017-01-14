using UnityEngine;
using System.Collections;

public class PoisonEffect : Effect {

    private IEnumerator poisonDamage;
    public float poisonRate;

	protected override void doEffect () {
		poisonDamage = PoisonCoroutine();
        StartCoroutine(poisonDamage);
	}

	IEnumerator PoisonCoroutine() {
        while (true)
        {
            if (this.gameObject.GetComponent<Health>() != null)
            {
                Health healthComponent = this.gameObject.GetComponent<Health>() as Health;
                healthComponent.health -= this.magnitude;
                healthComponent.checkHealth();
                yield return new WaitForSeconds(poisonRate); //time between occurances of damage is currently half a second
            }
        }
	}

	protected override void reverseEffect() {
        StopCoroutine(poisonDamage);
	}
}
