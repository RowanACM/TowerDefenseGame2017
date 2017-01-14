using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a type of effect which slows the speed of the movement component attached to this game object
/// </summary>
public class SlowEffect : Effect {

	protected override void doEffect () {
		if (gameObject.GetComponent<Movement>() != null) {
			Movement movementComponent = gameObject.GetComponent<Movement>() as Movement;
			movementComponent.movementSpeed = movementComponent.movementSpeed / magnitude;
		}
	}

	protected override void reverseEffect () {
        if (gameObject.GetComponent<Movement>() != null)
        {
            Movement movementComponent = gameObject.GetComponent<Movement>() as Movement;
			movementComponent.movementSpeed = movementComponent.movementSpeed * magnitude;
		}
	}
}
