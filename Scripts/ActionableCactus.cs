using UnityEngine;
using System.Collections;
using System;

public class ActionableCactus : Actionable
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
            GameManager.instance.RemoveActionable(this);
            GameManager.instance.AddLevelData("Full Frontal Rammings", 1);
            GameManager.instance.LoseLife();
            GameManager.instance.ResetCombo();
            GameManager.instance.HitText(3, this.transform.position + new Vector3(0, 32, 0));
            this.enabled = false;
        }
	}

    public override void AxeHit(double existenceTime, double travelDistance)
    {
        this.enabled = false;
        GameManager.instance.RemoveActionable(this);
        GameManager.instance.AddScore(1+0.1*GameManager.instance.Combo, "Gotcha, damned cactus.");
        GameManager.instance.AddLevelData("Cactus Needles", 1 + 0.1 * GameManager.instance.Combo);
        GameManager.instance.AddCombo(1);
        GameManager.instance.HitText(0, this.transform.position + new Vector3(0, 32, 0));
    }

    public override void PerformSuccesfully(bool comboSigns)
    {
        AxeHit(0, 0);
    }

    public override void Die()
    {
        if(animator != null)
            animator.SetBool("dead", true);
        this.enabled = false;
        GameManager.instance.AddDoodad(gameObject);
    }
}
