using UnityEngine;
using System.Collections;

public class DamageAreaSpawner : MonoBehaviour {
    float nextTrigger;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (nextTrigger < Time.time) {
            nextTrigger = Time.time + Random.Range(0.1f, 0.7f);
            GameObject go = Instantiate(Resources.Load("DamageZone"))as GameObject;
            go.transform.position = Random.insideUnitCircle * 7;
            go.transform.localScale = new Vector3(Random.Range(1, 10), Random.Range(1, 10), 0.1f);
            go.GetComponent<DamageArea>().Arm(1);
        }
	}
}
