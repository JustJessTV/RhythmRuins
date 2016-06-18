using UnityEngine;
using System.Collections;

public class BaddieSpawner : MonoBehaviour {

    float nextTrigger;
    void Update() {
        if (nextTrigger < Time.time) {
            nextTrigger = Time.time + Random.Range(0.1f, 0.7f);
            GameObject go = Instantiate(Resources.Load("Baddies/Missle")) as GameObject;
            go.transform.position = Random.insideUnitCircle.normalized * 7;
        }
    }
}
