using UnityEngine;
using System.Collections;

public class Damager : MonoBehaviour {
    public LayerMask layerMask;
    void OnTriggerEnter(Collider other) {
        if (((1<<other.gameObject.layer) & layerMask) == 0) return;
        Debug.Log("Hit " + other.transform.name);
        Damagie damagie = other.gameObject.GetComponent<Damagie>();
        if (damagie != null) {
            damagie.Hit(1);
        }
    }
}
