using UnityEngine;
using System.Collections;

public class ShotSweep : MonoBehaviour {

    float speed = 100;
    float deathTime = 0.3f;
	void Start () {
        Destroy(gameObject, deathTime);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 700 * Time.deltaTime, 0);
	}
}
