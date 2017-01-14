using UnityEngine;
using System.Collections;

/**
 * Health component stores the health points for objects and contains an interface for dealing damage to them.
 * @author Alexander Mendohlsenn
 * @author Nicholas Weintraut (refactoring)
 */
public class Health : MonoBehaviour {

	public float health;
	private bool inv; // invincibility frames active?

	// Use this for initialization
	void Start () {
		inv = false;
	}

    public bool TakeDamage(GameObject source, int damage)
    {
        if (this.gameObject.GetComponent("Armor") != null)
        {
            Armor armorComponent = this.gameObject.GetComponent("Armor") as Armor;
            //get the damage after it is damped by the receiver's armor
            int finalDamage = damage - armorComponent.GetDamageResistance(source, damage);
            if (finalDamage <= 0)
            {
                return false;
            }
            else
            {
                this.health -= finalDamage;
            }
        }
        else
        {
            this.health -= damage;
        }
        checkHealth();
        //upon taking damage, invincibility will kick in for 0.1 seconds
        StartCoroutine(Invincibility(0.1f));
        return true;
    }

    /**
     * Coroutine which sets invincibility flag to true for the specified duration (in seconds)
     */ 
    IEnumerator Invincibility(float duration)
    {
        inv = true;
        yield return new WaitForSeconds(duration);
        inv = false;
    }

    /**
     * Checks health, and if less than or equal to zero, destroys this object
     */
	public void checkHealth() {
		if (this.health <= 0) {
			this.health = 0;
            // play death animation
            SendMessage("OnDeath");
		}
	}
}
