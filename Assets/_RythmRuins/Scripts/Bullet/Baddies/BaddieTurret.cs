using UnityEngine;
using System.Collections;

public class BaddieTurret : Damagie {
    public GameObject pivot;
    public GameObject shot;
    public float turnSpeed = 200;
    public bool charging = false;
    public float nextShot;
    public float delayTime = 0.5f;
    bool playAnim = false;
    AnimNode anBeam;
    Sprite[] sprites;
    public SpriteRenderer sr;
    void Awake() {
        sprites = Resources.LoadAll<Sprite>("pod and beam");
        anBeam = new AnimNode(ref sr, sprites, 5, 19, 15, AnimNodeType.Single);
    }
    void Start() {
        hp = 5;
        Vector3 spawnPose = transform.position;
        spawnPose.y = Random.Range(0, 5);
        spawnPose.z = 0;
        transform.position = spawnPose;
    }
	void Update () {
        if(!charging)GotoTarget(PlatformerController.main.transform);
        if (playAnim) anBeam.Update();
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
        shot = Instantiate(Resources.Load("DamageZone"),
            pivot.transform.position + pivot.transform.forward * 10,
            pivot.transform.rotation) as GameObject;
        shot.transform.localScale = new Vector3(0.05f, 0.5f, 20);
        DamageArea da = shot.GetComponent<DamageArea>();
        da.onComplete = Complete;
        da.Arm(0.5f);
        charging = true;
        anBeam.Play();
        playAnim = true;
    }
    void BeamDone() {
        sr.sprite = null;
        playAnim = false;
    }
    void Complete() {
        nextShot = Time.time + delayTime;
        charging = false;
    }
    public override void Kill() {
        if (shot != null) {
            Destroy(shot);
        }
        Vector3 point = transform.position;
        GameObject go = Instantiate(Resources.Load("DamageZoneCircle"))as GameObject;
        go.transform.position = point;
        go.transform.localScale = new Vector3(8, 0.1f, 8);
        DamageArea da = go.GetComponent<DamageArea>();
        da.Arm(0.5f);
        da.onComplete = () => { GameObject boom = Instantiate(Resources.Load("FX/Boom"), point, Quaternion.identity) as GameObject; boom.transform.localScale = Vector3.one * 3; };
        base.Kill();
    }
}
