using UnityEngine;
using System.Collections;

public class Axe : MonoBehaviour {

    public double ExistenceTime { get; private set; }
    public double TravelDistance { get; private set; }
    public float SpeedMod { get; set; }

	// Use this for initialization
	void Start () {
        ExistenceTime = 0;
        TravelDistance = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(!GameManager.instance.levelPaused)
        {
            this.transform.Translate(Time.deltaTime * 13*64 * SpeedMod, 0, 0);
            TravelDistance += Time.deltaTime * 13  * SpeedMod;
            ExistenceTime += Time.deltaTime;
            if (ExistenceTime > 60)
                DestroySelf();
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.transform.position.x < GameManager.instance.player.transform.position.x)
            return;
        if (coll.gameObject.tag == "Actionable")
        {
            Actionable a = coll.gameObject.GetComponent<Actionable>();
            if (!a.enabled)
                return;
            coll.gameObject.GetComponent<Actionable>().AxeHit(ExistenceTime, TravelDistance);
        }
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
