using UnityEngine;
using System.Collections;
using System;

public class ActionableComboPost : Actionable
{
    private Animator animator;
    /// <summary>
    /// How many quarters of combo the ComboSign removes.
    /// </summary>
    public int maxCombo;

    // Use this for initialization
    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if(GameManager.instance.player.transform.position.x > gameObject.transform.position.x-56)
        {
            PerformSuccesfully(false);
        }
	}

    public override void AxeHit(double existenceTime, double travelDistance)
    {
        PerformSuccesfully(false);
    }

    public override void Die()
    {
        this.enabled = false;
        GameManager.instance.AddDoodad(gameObject);
    }

    public override void PerformSuccesfully(bool comboSigns)
    {
        GameManager.instance.RemoveActionable(this);
        GameManager.instance.MaxCombo = maxCombo;
        this.enabled = false;
        if (animator != null)
            animator.SetTrigger("dead");
    }
}
