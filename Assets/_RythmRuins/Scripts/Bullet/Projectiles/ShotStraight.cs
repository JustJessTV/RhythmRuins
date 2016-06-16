using UnityEngine;
using System.Collections;

public class ShotStraight : MonoBehaviour {
    float speed= 100;
    float deathTime=0.3f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, deathTime);
        transform.position -= transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
	}
}
