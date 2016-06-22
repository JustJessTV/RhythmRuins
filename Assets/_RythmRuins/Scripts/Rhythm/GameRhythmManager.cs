using UnityEngine;
using System.Collections;

namespace RhythmRealm
{
    public class GameRhythmManager : MonoBehaviour
    {
        public GameObject spawnL0, spawnL1, spawnL2, spawnR0, spawnR1, spawnR2;
        public GameObject endL0, endL1, endL2, endR0, endR1, endR2;

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

            PatternBehave.SpawnPath pathL0 = new PatternBehave.SpawnPath(spawnL0.transform.position, endL0.transform.position, 0);
            PatternBehave.SpawnPath pathL1 = new PatternBehave.SpawnPath(spawnL1.transform.position, endL1.transform.position, 1);
            PatternBehave.SpawnPath pathL2 = new PatternBehave.SpawnPath(spawnL2.transform.position, endL2.transform.position, 2);
            PatternBehave.SpawnPath pathR0 = new PatternBehave.SpawnPath(spawnR0.transform.position, endR0.transform.position, 3);
            PatternBehave.SpawnPath pathR1 = new PatternBehave.SpawnPath(spawnR1.transform.position, endR1.transform.position, 4);
            PatternBehave.SpawnPath pathR2 = new PatternBehave.SpawnPath(spawnR2.transform.position, endR2.transform.position, 5);
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
