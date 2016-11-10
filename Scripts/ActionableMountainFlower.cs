using UnityEngine;
using System.Collections;
using System;

public class ActionableMountainFlower : Actionable
{
    private Animator animator;

    // Use this for initialization
    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (GameManager.instance.player.transform.position.x > gameObject.transform.position.x-46)
        {
            PerformSuccesfully(true);
        }
    }

    public override void AxeHit(double existenceTime, double travelDistance)
    {
        GameManager.instance.HitText(3, this.transform.position + new Vector3(0, 32, 0));
        GameManager.instance.ResetCombo();
        this.enabled = false;
        GameManager.instance.RemoveActionable(this);
        GameManager.instance.LoseLife();
        Destroy(this.gameObject);
    }
    
    public override void Die()
    {
        if (animator != null)
            animator.SetBool("dead", true);
        this.enabled = false;
        GameManager.instance.AddDoodad(gameObject);
    }


    public override void PerformSuccesfully(bool comboSigns)
    {
        GameManager.instance.RemoveActionable(this);
        this.enabled = false;
        GameManager.instance.HitText(2, this.transform.position + new Vector3(0, 32, 0));
        for (int loop=0;loop<5;loop++)
        {
            GameManager.instance.AddScore(2 + 0.2 * GameManager.instance.Combo, loop == 0 ? "I got the flower!." : "");
            GameManager.instance.AddLevelData("Mountain Flower Petals", 2 + 0.2 * GameManager.instance.Combo);
            GameManager.instance.AddCombo(1);
        }
    }
}
