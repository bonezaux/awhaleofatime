using UnityEngine;
using System.Collections;
using System;

public class ActionableRock : Actionable
{

    // Use this for initialization
    public void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        if (GameManager.instance.player.transform.position.x > gameObject.transform.position.x-56)
        {
            GameManager.instance.RemoveActionable(this);
            this.enabled = false;
        }
    }

    public override void AxeHit(double existenceTime, double travelDistance)
    {
        GameManager.instance.HitText(3, this.transform.position + new Vector3(0, 32, 0));
        GameManager.instance.AddLevelData("Broken Axes", 1);
        GameManager.instance.LoseLife();
        GameManager.instance.ResetCombo();
    }

    public override void Die()
    {
        Destroy(this.gameObject);
    }

    public override void PerformSuccesfully(bool comboSigns)
    {
    }
}
