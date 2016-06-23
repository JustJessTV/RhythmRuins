using UnityEngine;
using System.Collections;
public class BGSpawner : DispatchEvent {
    float lastXSpawn = 0;
    CamController cc;
    GameObject[] buildings;
    public override void Awake() {
        cc = Camera.main.GetComponent<CamController>();
        buildings = Resources.LoadAll<GameObject>("BG");
        SimulateSpawn(100);
    }
    public override void Update() {
        if (lastXSpawn < cc.transform.position.x) {
            SpawnBuilding(cc.transform.position);
            lastXSpawn++;
        }
    }
    void SpawnBuilding(Vector3 point) {
        SpawnBuilding(point, 0);
    }
    void SpawnBuilding(Vector3 point,float offset){
        if (Random.value < 0.8f) return;
        int index = Random.Range(0, buildings.Length);
        float dist = Random.Range(12f, 50);
        float slide = Random.Range(10, 20);
        GameObject go = GameObject.Instantiate(buildings[index], 
            CamController.GetPointAtDistRight(dist) + Vector3.right * (slide+offset) + Vector3.forward * 2 + Vector3.up*(8-point.y), 
            Quaternion.identity) as GameObject;
    }
    void SimulateSpawn(int dist) {
        lastXSpawn = -dist;
        while (lastXSpawn < 0) {
            SpawnBuilding(cc.transform.position+Vector3.right,lastXSpawn);
            lastXSpawn++;
        }
    }
}
