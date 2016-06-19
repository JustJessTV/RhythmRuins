using UnityEngine;
using System.Collections;

public class BaddieTurret : MonoBehaviour {
    public GameObject pivot;
    public float turnSpeed = 200;
    public bool charging = false;
    public float nextShot;
    public float delayTime = 0.5f;
	void Update () {
        if(!charging)GotoTarget(PlatformerController.main.transform);
	}
    void GotoTarget(Transform target) {
        Vector3 dir = target.position - pivot.transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        float uDotT = Vector3.Dot(-pivot.transform.up, dir);
        float fDotT = Vector3.Dot(-pivot.transform.forward, dir);
        pivot.transform.Rotate(uDotT * Time.deltaTime * turnSpeed, 0, 0);
        if (fDotT<-0.99f&&nextShot<Time.time) {
            Charge();
        }
    }
    void Charge() {
        GameObject go = Instantiate(Resources.Load("DamageZone"),
            pivot.transform.position + pivot.transform.forward * 10,
            pivot.transform.rotation) as GameObject;
        go.transform.localScale = new Vector3(0.05f, 0.5f, 20);
        DamageArea da = go.GetComponent<DamageArea>();
        da.onComplete = Complete;
        da.Arm(0.5f);
        charging = true;
    }
    void Complete() {
        nextShot = Time.time + delayTime;
        charging = false;
    }
}
