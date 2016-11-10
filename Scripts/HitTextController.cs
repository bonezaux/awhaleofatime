using UnityEngine;
using System.Collections;

public class HitTextController : MonoBehaviour {

    private double timeSinceStart;
    private Vector3 baseScale;
    public float minScale;
    public int function;
    /// <summary>
    /// How much the function does
    /// </summary>
    public float effect;

	// Use this for initialization
	void Start () {
        timeSinceStart = 0;
        baseScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceStart += Time.deltaTime*3;
        float scale = 1;
        if(function == 0)
            scale = minScale - 4* effect * (float)System.Math.Pow(timeSinceStart-0.5,2);// min-4*eff*(x-0.5)^2
        else if(function == 1) // min+2*eff*x, max min+effect
        {
            if (timeSinceStart < 0.5)
                scale = minScale + (float)timeSinceStart * 2*effect; 
            else
                scale = minScale + 1*effect;
        }
        Vector3 resScale = baseScale;
        resScale = Vector3.Scale(baseScale, new Vector3(scale, scale, scale));
        transform.localScale = resScale;
        if (timeSinceStart > 1)
            Destroy(gameObject);
    }
}
