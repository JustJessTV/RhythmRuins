using UnityEngine;
using System.Collections;

public class TileKill : MonoBehaviour {
    float boundWidth;
    void Start() {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
            boundWidth = mr.bounds.size.x;
        else
            boundWidth = 2;
    }
	void Update () {
        if (CamController.GetDistFromLeftWall(transform.position) < -boundWidth*2) {
            Destroy(gameObject);
        }
	}
}
