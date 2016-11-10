using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerPosition = GameManager.instance.player.transform.position*0.99f;
        this.transform.Translate(playerPosition - transform.position + new Vector3(-384, -72, 4));
    }
}
