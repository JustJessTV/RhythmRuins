using UnityEngine;
using System.Collections;

public class BaddieHelicopter : Damagie {
    float delay = 2;
    float fireTime;
    public Vector3 velocity;
    public float friction;
    public float speed;
    public bool firing = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetDir = PlatformerController.main.transform.position - transform.position;
        if (fireTime < Time.time) {
            Fire(targetDir);
        }
        Move(targetDir);
	}
    void Move(Vector3 dir) {

        float dist = dir.magnitude;
        dir.Normalize();
        float rDotD = Vector3.Dot(transform.right,dir);
        Debug.DrawRay(transform.position + Vector3.back, Vector3.right*rDotD);
        if(!firing)
        velocity.x += rDotD * Time.deltaTime*speed;
        transform.position += velocity * Time.deltaTime;
        velocity -= velocity * Time.deltaTime*friction;
    }
    void Fire(Vector3 dir) {
        float dDotD = Vector3.Dot(Vector3.down, dir.normalized);
        if (dDotD < 0.95f) return;
        fireTime = Time.time + delay;
        GameObject shot = Instantiate(Resources.Load("DamageZone"),
            transform.position + Vector3.down * 10,
            Quaternion.Euler(0, 0, 0)) as GameObject;
        shot.transform.localScale = new Vector3(1, 20, 0.05f);
        shot.transform.parent = transform;
        DamageArea da = shot.GetComponent<DamageArea>();
        da.Arm(1f);
        da.onComplete = Complete;
        shot = Instantiate(Resources.Load("DamageZone"),
            transform.position + Vector3.Normalize(Vector3.down + Vector3.right) * 10,
            Quaternion.Euler(0, 0, 45)) as GameObject;
        shot.transform.localScale = new Vector3(1, 20, 0.05f);
        shot.transform.parent = transform;
        da = shot.GetComponent<DamageArea>();
        da.Arm(1f);
        shot = Instantiate(Resources.Load("DamageZone"),
            transform.position + Vector3.Normalize(Vector3.down + -Vector3.right) * 10,
            Quaternion.Euler(0, 0, -45)) as GameObject;
        shot.transform.localScale = new Vector3(1, 20, 0.05f);
        shot.transform.parent = transform;
        da = shot.GetComponent<DamageArea>();
        da.Arm(1f);
        firing = true;
    }
    void Complete() {
        firing = false;
    }
}
