using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for Effect which implements an interface for applying effects with a given duration
/// </summary>
public abstract class Effect : MonoBehaviour {

	public int magnitude; // the magnitude of the effect (7 poison damage, 7 less defense, etc.)
	public float duration; // time it takes for the effect to wear off

	protected abstract void doEffect(); // should change the affected object based on magnitude
	protected abstract void reverseEffect(); // should reverse the effect

    /// <summary>
    /// Public interface for applying this effect. Call it once after initialization
    /// </summary>
    public void apply()
    {
        StartCoroutine(applyEffect());
    }

    private IEnumerator applyEffect()
    {
        doEffect();
        yield return new WaitForSeconds(duration);
        reverseEffect();
        Destroy(this);
    }
}
