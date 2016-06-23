using UnityEngine;
using System.Collections;

public class TileLogic : MonoBehaviour {
    Renderer renderer;
    bool hasSpawned = false;
    private Plane[] planes;
	void Awake () {
        renderer = GetComponent<Renderer>();
	}
    Camera cam {
        get { return Camera.main; }
    }
	
	// Update is called once per frame
	void Update () {
        if (!hasSpawned && CamController.GetDistFromRightWall(transform.position) < 20) {
            hasSpawned = true;
            GameObject go = Instantiate(gameObject,transform.position+Vector3.right*3,transform.rotation)as GameObject;
        }
	}
}
