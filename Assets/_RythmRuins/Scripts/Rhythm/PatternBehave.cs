using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RhythmRealm
{
    public class PatternBehave : MonoBehaviour
    {

        public static float delayFactor;
        public static float beginPlayTime;
     //   public static bool gameBegin;
        public class SpawnPath
        {
            int buttonIndex;
            public GameObject beatObj;
            public BeatBehave beatScript;
            public GameObject spawnPoint;
            public GameObject endPoint;
            public SpawnPath(GameObject spawn, GameObject end, int index)
            {
                this.spawnPoint         = spawn;
                this.endPoint           = end;
                this.buttonIndex        = index;
                spawnPaths.Add(this);
                this.beatObj                 = Resources.Load("beats/beat"+index.ToString()) as GameObject;
                this.beatObj.name = "beat" + index;
                Debug.Log(this.spawnPoint.name + ":" + this.endPoint.name + ":" + this.beatObj);
                this.beatScript              = beatObj.GetComponent<BeatBehave>();
                this.beatScript.spawnPoint   = this.spawnPoint;
                this.beatScript.endPoint     = this.endPoint;
            }
            public void Spawn() {
                GameObject go = Instantiate(this.beatObj);
                
            }
        }
        private static List<SpawnPath> _spawnPaths;
        static List<SpawnPath> spawnPaths
        {
            get
            {
                return _spawnPaths ?? (_spawnPaths = new List<SpawnPath>());
            }
            set
            {
                _spawnPaths = value;
            }
        }
        public enum PatternPlayState
        {
            idle,
            begin,
            play
        }
        public static PatternPlayState patternPlayState;
        public List<GameObject> spawnPoints;

        BeatManager beatManager;
        int playDetectIndexer;
        int spawnIndexer;
        // Use this for initialization
        void Start()
        {
            // debugging
            foreach (SpawnPath s in spawnPaths) {
                Debug.Log(s.spawnPoint+" testng ::" + s.beatObj.name);
            }
            beatManager = GetComponent<BeatManager>();
        //    if (delayFactor == 0) delayFactor = 3;
            delayFactor = 5;
        }

        // Update is called once per frame
        void Update()
        {
            if (patternPlayState == PatternPlayState.begin)
            {
                Debug.Log("paternBehave begin");
                BeginPlay();
            }
            if (patternPlayState == PatternPlayState.play)
            {
                if (spawnIndexer < beatManager.inputAndValue.Count)
                {
                    PatternSpawner();
                }

                if (playDetectIndexer < beatManager.inputAndValue.Count)
                {
                    PatternDetector(delayFactor);
                }
                else
                {
                    patternPlayState = PatternPlayState.idle;
                }
            }
        }
        void BeginPlay()
        {
            beatManager.LoadRecordedKeys("pizza");
            // call start all music
            beginPlayTime = Time.time;
            playDetectIndexer = 0;
            spawnIndexer = 0;
            patternPlayState = PatternPlayState.play;
            Debug.Log(beatManager.inputAndValue.Count);
        }
        void PatternDetector(float delay)
        {

            if ((Time.time - beginPlayTime) > delay + beatManager.inputAndValue[playDetectIndexer].time)
            {
                Debug.Log("expect button " + beatManager.inputAndValue[playDetectIndexer].index);
                playDetectIndexer++;
            }
        }
        void PatternSpawner()
        {
            BeatManager.Notes[] notes = new BeatManager.Notes[0];
            PatternSpawner(notes);
        }
        void PatternSpawner(BeatManager.Notes[] cencorNotes)
        {
            if (cencorNotes.Length > 0)
            {
                BeatManager.Notes note;
                beatManager.NoteType(beatManager.inputAndValue[spawnIndexer].time, out note);

                foreach (BeatManager.Notes n in cencorNotes)
                {
                    if (n == note)
                    {
                        spawnIndexer++;
                        Debug.Log("skipping " + note);
                        continue;
                    }
                }
            }
            int butNum = beatManager.inputAndValue[spawnIndexer].index;
            if ((Time.time - beginPlayTime) > beatManager.inputAndValue[spawnIndexer].time)
            {
                // spawn here
                if (butNum > 3) {
                    Debug.Log(butNum + "skip");
                    spawnIndexer++;
                    return;
                }
                spawnPaths[butNum].Spawn();
       //         Debug.Log("spawn : " + beatManager.inputAndValue[butNum].time);
       //         Debug.Log("from  : " + beatManager.inputAndValue[butNum].index);
                spawnIndexer++;
            }
        }
    }
}
