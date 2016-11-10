using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 playerPosition = player.transform.position;
        this.transform.Translate(playerPosition-transform.position+new Vector3(424,328,-10));
	}
}
