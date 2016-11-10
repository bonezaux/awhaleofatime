using UnityEngine;
using System.Collections;

public abstract class Actionable : MonoBehaviour {

    /// <summary>
    /// Called when the Actionable is hit by an axe.
    /// </summary>
    /// <param name="existenceTime">How long the Axe has been in the air.</param>
    public abstract void AxeHit(double existenceTime, double travelDistance);

    public abstract void PerformSuccesfully(bool comboSigns);

    public abstract void Die();
}
