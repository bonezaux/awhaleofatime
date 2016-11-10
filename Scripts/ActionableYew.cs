using UnityEngine;
using System.Collections;
using System;

public class ActionableYew : Actionable
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
        travelDistance += 1;
        if (travelDistance > 6)
            return;
        else
        {
            GameManager.instance.RemoveActionable(this);
            this.enabled = false;
            int comboRecursions = 2;
            if (travelDistance < 4 && travelDistance > 2)
            {
                comboRecursions = 4;
                GameManager.instance.HitText(2, this.transform.position + new Vector3(0, 64, 0));
            }
            else if (travelDistance < 5 && travelDistance > 1)
            {
                comboRecursions = 3;
                GameManager.instance.HitText(1, this.transform.position + new Vector3(0, 64, 0));
            }
            else
            {
                GameManager.instance.HitText(0, this.transform.position + new Vector3(0, 64, 0));

            }
            for(int loop=0;loop<comboRecursions;loop++)
            {
                GameManager.instance.AddCombo(1);
                GameManager.instance.AddScore(2 + 0.2 * GameManager.instance.Combo, "");
                GameManager.instance.AddLevelData("Yew Logs", 1 + 0.1 * GameManager.instance.Combo);
            }
        }
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
        if (!comboSigns)
            AxeHit(0, 2);
        else
            AxeHit(0, 5);
    }
    

}
