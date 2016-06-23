using UnityEngine;
using System.Collections;

public class BaddieMissle : Damagie{
    public float speed=3;
    public float turnSpeed=80;
    Rigidbody rb;
	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        speed = Random.Range(4.5f, 5.5f);
        turnSpeed = Random.Range(180, 220);
	}
	
	// Update is called once per frame
	void Update () {
        GotoTarget(PlatformerController.main.transform);
	}
    void GotoTarget(Transform target) {
        Vector3 dir = target.position-transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        float rDotT = Vector3.Dot(transform.right, dir);
        rb.MovePosition(transform.position + transform.up * speed * Time.deltaTime);
        transform.Rotate(0, 0, -rDotT * turnSpeed * Time.deltaTime);
        
    }
    public override void Kill(){
        Instantiate(Resources.Load("FX/Boom"), transform.position, Quaternion.identity);
        base.Kill();
    }
}
