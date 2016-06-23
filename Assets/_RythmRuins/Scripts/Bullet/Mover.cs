using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	// Use this for initialization
    public float speed = 10;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.right * Time.deltaTime * speed;
	}
}
