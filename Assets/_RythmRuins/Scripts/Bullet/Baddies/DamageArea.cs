using UnityEngine;
using System.Collections;

public class DamageArea : MonoBehaviour {
    public enum HitType { Square,Circle}
    public HitType hitType = HitType.Square;
    float startTime;
    float endTime;
    bool dead = false;
    bool going = false;
    public bool trigger = false;
    int deadCounter = 0;
    float t {
        get {
            return Mathf.InverseLerp(startTime, endTime, Time.time);
        }
    }
    public System.Action onComplete;
    Material mat;
    void Awake() {
        Destroy(GetComponent<Collider>());
    }
	void Start () {
        mat = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        if (trigger) {
            trigger = false;
            Arm(1);
        }
        if (!going) return;
        if (deadCounter > 2) {
            if (onComplete != null) onComplete();
            Destroy(gameObject);
        }
        if (dead) {
            deadCounter++;
        }
        mat.SetFloat("_Thickness", t);
        if (t >= 1) {
            dead = true;
            Collider col;
            switch (hitType) {
                case HitType.Square:
                    col = gameObject.AddComponent<BoxCollider>();
                    col.isTrigger = true;
                    break;
                case HitType.Circle:
                    col = gameObject.AddComponent<CapsuleCollider>();
                    col.isTrigger = true;
                    break;
            }
        }
	}
    public void Arm(float duration) {
        startTime = Time.time;
        endTime = startTime + duration;
        going = true;
    }
}
