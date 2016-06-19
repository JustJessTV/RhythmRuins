using UnityEngine;
using System.Collections;

public class TileKill : MonoBehaviour {
	void Update () {

        if (CamController.GetDistFromLeftWall(transform.position) < -20) {
            Destroy(gameObject);
        }
	}
}
