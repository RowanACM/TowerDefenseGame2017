using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PriorityMode : MonoBehaviour {
    public abstract Transform GetBest(Transform[] options);

    public abstract Transform[] GetBest(Transform[] options, int quantity);
}
