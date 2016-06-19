using UnityEngine;
using System.Collections;

public class DamageArea : MonoBehaviour {

    float startTime;
    float endTime;
    bool dead = false;
    bool going = false;
    public bool trigger = false;
    float t {
        get {
            return Mathf.InverseLerp(startTime, endTime, Time.time);
        }
    }
    public System.Action onComplete;
    Material mat;
    void Awake() {
        Destroy(GetComponent<BoxCollider>());
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
        if (dead) {
            if(onComplete!=null)onComplete();
            Destroy(gameObject);
        }
        mat.SetFloat("_Thickness", t);
        if (t >= 1) {
            dead = true;
            BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            bc.isTrigger = true;
        }
	}
    public void Arm(float duration) {
        startTime = Time.time;
        endTime = startTime + duration;
        going = true;
    }
}
