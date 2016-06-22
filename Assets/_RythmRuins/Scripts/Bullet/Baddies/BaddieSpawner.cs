using UnityEngine;
using System.Collections;

public class BaddieSpawner : MonoBehaviour {
    Camera cam {
        get { return Camera.main; }
    }
    float nextTrigger;
    void Update() {
        if (nextTrigger < Time.time) {
            nextTrigger = Time.time + Random.Range(0.1f, 0.7f);
            GameObject go = Instantiate(Resources.Load("Baddies/Missle")) as GameObject;
            go.transform.position = Random.insideUnitCircle.normalized * 7;
            go.transform.position += cam.transform.position.x * transform.right;
        }
    }
}
