using UnityEngine;
using System.Collections;
using System;

public class ActionableOak : Actionable
{
    private Animator animator;

    // Use this for initialization
    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if(GameManager.instance.player.transform.position.x > gameObject.transform.position.x)
        {
            GameManager.instance.HitText(0, this.transform.position + new Vector3(0, 32, 0));
            GameManager.instance.RemoveActionable(this);
            GameManager.instance.AddLevelData("Full Frontal Rammings", 1);
            GameManager.instance.LoseLife();
            GameManager.instance.ResetCombo();
            this.enabled = false;
        }
	}

    public override void AxeHit(double existenceTime, double travelDistance)
    {
        GameManager.instance.HitText(0, this.transform.position + new Vector3(0, 32, 0));
        GameManager.instance.RemoveActionable(this);
        GameManager.instance.AddScore(1+0.1*GameManager.instance.Combo, "Gotcha, damned tree.");
        GameManager.instance.AddLevelData("Oak Logs", 1 + 0.1 * GameManager.instance.Combo);
        GameManager.instance.AddCombo(1);
        this.enabled = false;
    }

    public override void Die()
    {
        if(animator != null)
            animator.SetBool("dead", true);
        this.enabled = false;
        GameManager.instance.AddDoodad(gameObject);
    }

    public override void PerformSuccesfully(bool comboSigns)
    {
        AxeHit(0, 0);
    }
}
