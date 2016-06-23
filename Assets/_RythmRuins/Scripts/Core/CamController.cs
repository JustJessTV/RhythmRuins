using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {
    public static Vector3 LeftWall;
    public static Vector3 RightWall;
    public float speed = 0.5f;
    static Camera _cam;
    static Camera cam {
        get {
            return _cam ?? (_cam = Camera.main.GetComponent<Camera>());
        }
        set {
            _cam = value;
        }
    }
    float lastXSpawn = 0;
	// Use this for initialization
    void Awake () {
        cam = GetComponent<Camera>();
        Debug.Log(cam);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        LeftWall = Vector3.Cross(planes[0].normal, Vector3.up);
        RightWall = Vector3.Cross(planes[1].normal*-1, Vector3.up);
        GameStateHandler.beginPlay += GameEnter;
        GameStateHandler.gameOver+=GameEnd;
    }
	void Start () {
	
	}
    void GameEnter() {
        speed = 2;
    }
    void GameEnd() {
        speed = 0.5f;
    }
	// Update is called once per frame
    void Update() {
        Debug.DrawRay(transform.position, LeftWall * 10);
        Debug.DrawRay(transform.position, RightWall * 10);
        CheckForRandomSpawn();
        transform.position += Vector3.right * Time.deltaTime*speed;
	}
    public static Vector3 GetPointAtDistRight(float dist) {
   //     Debug.Log(cam);
    //    Debug.Log(RightWall);
        return cam.transform.position + RightWall * dist;
    }
    public static Vector3 GetPointAtDistLeft(float dist) {
        return cam.transform.position + LeftWall * dist;
    }
    public static float GetDistFromRightWall(Vector3 point) {
        float zDist = point.z - cam.transform.position.z;
        Vector3 intersect = GetPointAtDistRight(zDist);
        return point.x - intersect.x;
    }
    public static float GetDistFromLeftWall(Vector3 point) {
        float zDist = point.z - cam.transform.position.z;
        Vector3 intersect = GetPointAtDistLeft(zDist);
        return point.x - intersect.x;
    }
    void CheckForRandomSpawn() {
        if (lastXSpawn < transform.position.x) {
            SpawnRandom();
            lastXSpawn += 1;
        }
    }
    void SpawnRandom() {
        if (GameStateHandler.gameState != GameStateHandler.State.gamePlaying) return;
        if (Random.value > 0.9) {
            Instantiate(Resources.Load("Baddies/Turret"), GetPointAtDistRight(10)+Vector3.right*2, Quaternion.identity);
            /*
            Vector3 position = transform.position;
            position.x += Random.Range(-7, 7);
            position.y = 0;
            position.z = 0;
            Instantiate(Resources.Load("TankDamage"), position, Quaternion.identity);*/
        }
    }
}
