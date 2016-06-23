using UnityEngine;
using System.Collections;

public class TileLogic : MonoBehaviour {
    Renderer renderer;
    public float width;
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
        if (!hasSpawned && CamController.GetDistFromRightWall(transform.position) < width) {
            hasSpawned = true;
            GameObject go = Instantiate(gameObject,transform.position+Vector3.right*width,transform.rotation)as GameObject;
        }
	}
}
