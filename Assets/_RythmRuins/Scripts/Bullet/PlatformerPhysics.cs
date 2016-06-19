using UnityEngine;
using System.Collections;

public class PlatformerPhysics:ActorPhysics{

    public delegate void CallStartAttack();
    public event CallStartAttack onStartAttack;

    public delegate void CallStartHurt();
    public event CallStartHurt onStartHurt;
    bool lastFrameShot = false;
    public bool invuln = false;
    float hp = 1;
    public void Awake() {
        base.info = new ActorInfo() {
            runSpeed = 2f,
            gravity = 1,
            jumpPower = 32,
            friction = 15,
            airFriction = 2,
            airSpeed = 0.4f
        };
        base.Awake();
    }
    public void Shoot(Vector3 dir, Transform transform) {
        float magnitude = dir.magnitude;
        if (magnitude < 1) {
            lastFrameShot = false;
            return;
        }
        if (lastFrameShot) return;
        if(onStartAttack!=null)onStartAttack();
        lastFrameShot = true;
        dir.Normalize();
        Object obj = Resources.Load("ShotSweep");

        GameObject go = Instantiate( obj as GameObject,
            transform.position, Quaternion.LookRotation(dir,Vector3.forward))as GameObject;
        go.transform.parent = transform;
    }
    void OnTriggerEnter(Collider other) {
        if ((other.gameObject.layer & (LayerMask.NameToLayer("BadUnits") | LayerMask.NameToLayer("BadDamage"))) == 0) return;
        Debug.Log("Hit " + other.transform.name);
        BaddieMissle bm = other.GetComponent<BaddieMissle>();
        if (bm != null) {
            bm.Kill();
        }
        if (invuln) return;
        if (onStartHurt != null) onStartHurt();
        hp -= 0.2f;
        velocity = new Vector2(facingRight*-10, 10);
    }
    void OnGUI() {
        if (hp <= 0.1) {
            GUI.Button(new Rect(10, 10, 200, 20), "Dead");
            return;
        }
        GUI.Button(new Rect(10, 10, hp * 200, 20), "");
        GUI.Button(new Rect(10, 10, 200, 20), "");
    }

}
