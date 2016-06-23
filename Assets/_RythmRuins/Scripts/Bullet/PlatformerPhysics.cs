using UnityEngine;
using System.Collections;

public class PlatformerPhysics:ActorPhysics{

    public delegate void CallStartAttack();
    public event CallStartAttack onStartAttack;

    public delegate void CallStartHurt();
    public event CallStartHurt onStartHurt;

    public delegate void CallDeath();
    public event CallDeath OnDeath;

    bool lastFrameShot = false;
    public bool invuln = false;
    public float hp = 1;
    public float reserveHP = 1;
    public void Awake() {
        base.info = new ActorInfo() {
            runSpeed = 2f,
            gravity = 0.8f,
            jumpPower = 35,
            friction = 15,
            airFriction = 2,
            airSpeed = 0.4f
        };
        base.Awake();
    }
    public void Shoot(Vector3 dir, Transform transform) {
        float magnitude = dir.magnitude;
        if (magnitude < .9f) {
            lastFrameShot = false;
            return;
        }
        if (lastFrameShot) return;
        if(onStartAttack!=null)onStartAttack();
        lastFrameShot = true;
        dir.Normalize();
        Object obj = Resources.Load(PlatformerController.main.weapon.ToString());

        GameObject go = Instantiate( obj as GameObject,
            transform.position, Quaternion.LookRotation(dir,Vector3.forward))as GameObject;
        if (PlatformerController.main.weapon != WeaponType.Bare) {
            Root.playerManger.energy -= Root.playerManger.energyCost;
        }
        //go.transform.parent = transform;
    }
    void OnTriggerEnter(Collider other) {
        RegisterHit(other);
    }
    void OnTriggerStay(Collider other) {
        RegisterHit(other);
    }
    void RegisterHit(Collider other) {
        int mask = (1 << LayerMask.NameToLayer("BadUnits")) | (1 << LayerMask.NameToLayer("BadDamage"));
        int result = (1 << other.gameObject.layer) & (mask);
        if (result == 0) return;
        Debug.Log("Hit " + other.transform.name);
        BaddieMissle bm = other.GetComponent<BaddieMissle>();
        if (bm != null) {
            bm.Kill();
        }
        if (invuln) return;
        if (onStartHurt != null) onStartHurt();
        hp -= 0.1f;
        velocity = new Vector2(facingRight * -10, 10);
        CheckDeath();
    }
    void CheckDeath() {
        if(hp<=0){
            OnDeath();
        }
    }
    void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
            
        int w = Screen.width;
        int h = Screen.height;
        if (hp <= 0.0) {
            GUI.Button(new Rect(80, 80, w-160, h-300), "You died\n Score: "+(PlatformerController.main.finalScore.ToString("0.0")));
            return;
        }
        GUI.Button(new Rect(10, 30, hp * 200, 20), "");
        GUI.Button(new Rect(10, 30, 200, 20), "");
        GUI.Button(new Rect(10, 15, reserveHP * 150, 10), "");
        GUI.Button(new Rect(10, 15, 150, 10), "");
        //GUI.HorizontalScrollbar(new Rect(40, 5, w - 80,20), transform.position.x, 2, 0, 100);

    }

}
