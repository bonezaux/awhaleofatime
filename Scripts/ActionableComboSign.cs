using UnityEngine;
using System.Collections;
using System;

public class ActionableComboSign : Actionable
{
    private Animator animator;
    /// <summary>
    /// How many quarters of combo the ComboSign removes.
    /// </summary>
    public int removedQuarters;

    // Use this for initialization
    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if(GameManager.instance.player.transform.position.x > gameObject.transform.position.x-56)
        {
            GameManager.instance.RemoveActionable(this);
            this.enabled = false;
        }
	}

    public override void AxeHit(double existenceTime, double travelDistance)
    {
        GameManager.instance.RemoveActionable(this);
        GameManager.instance.AddCombo(-(int)Math.Round((GameManager.instance.Combo*(double)removedQuarters)/4));
        GameManager.instance.HitText(0, this.transform.position + new Vector3(0, 32, 0));
        this.enabled = false;
        if (animator != null)
            animator.SetTrigger("dead");
    }

    public override void Die()
    {
        this.enabled = false;
        GameManager.instance.AddDoodad(gameObject);
    }

    public override void PerformSuccesfully(bool comboSigns)
    {
        if (comboSigns)
            AxeHit(0, 0);
    }
}
