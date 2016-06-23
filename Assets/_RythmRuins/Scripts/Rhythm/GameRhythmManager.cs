using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RhythmRealm
{
    public class GameRhythmManager : MonoBehaviour
    {
        public static List<GameObject> spawnNodes  = new List<GameObject>();//spawn_L0, spawn_L1, spawn_L2, spawn_R0, spawn_R1, spawn_R2;
        public static List<GameObject> endNodes    = new List<GameObject>();//end_L0, end_L1, end_L2, end_R0, end_R1, end_R2;

        public bool setPathNodes;

        private static Camera _rhythmCam;
        public static Camera rhythmCam{
            get {
                return _rhythmCam ?? (_rhythmCam = FindObjectOfType<RhythmCam>().GetComponent<Camera>());
            }   
        }
        public static Vector3 rhythmCamPos {
            get {
                return rhythmCam.transform.position;
            }
        }
        // Use this for initialization
        void Start()
        {
            
            CreateSpawnNodes();
            PatternBehave.SpawnPath pathL0 = new PatternBehave.SpawnPath(spawnNodes[0], endNodes[0], 0);
            PatternBehave.SpawnPath pathL1 = new PatternBehave.SpawnPath(spawnNodes[1], endNodes[1], 1);
            PatternBehave.SpawnPath pathR0 = new PatternBehave.SpawnPath(spawnNodes[2], endNodes[2], 2);
            PatternBehave.SpawnPath pathR1 = new PatternBehave.SpawnPath(spawnNodes[3], endNodes[3], 3);
   //         PatternBehave.SpawnPath pathR1 = new PatternBehave.SpawnPath(spawn_R1.transform.position, end_R1.transform.position, 4);
   //         PatternBehave.SpawnPath pathR2 = new PatternBehave.SpawnPath(spawn_R2.transform.position, end_R2.transform.position, 5);
        }
        void CreateSpawnNodes() {

            GameObject[] nodesPrefabs = Resources.LoadAll<GameObject>("Beats/SpawnNodes");// as GameObject[];
            List<GameObject> nodesObjects = new List<GameObject>();
            for (int n = 0; n < nodesPrefabs.Length; n++){ // (GameObject n in nodesPrefabs) {
                
                if (nodesPrefabs[n].name.Contains("2")) continue;
                GameObject go = Instantiate(nodesPrefabs[n]);
                nodesObjects.Add(go);
            }
            for (int i = 0; i < nodesObjects.Count; i++)
            {
                if (nodesObjects[i].name.Contains("L2") ||
                    nodesObjects[i].name.Contains("R2"))
                {
                    Debug.Log("Cancel!");
                    continue; // cancel last button;
                }
                if (nodesObjects[i].name.Contains("spawn"))
                {
                    Debug.Log("add spawn :" + nodesObjects[i].name);
                    spawnNodes.Add(nodesObjects[i]);
                }
                if (nodesObjects[i].name.Contains("end"))
                {
                    Debug.Log("add end :" + nodesObjects[i].name);
                    endNodes.Add(nodesObjects[i]);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (setPathNodes)
            {
                setPathNodes = false;
            }
        }

        void SetPathNodes()
        {

        }

    }
}
