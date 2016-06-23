using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RhythmRealm
{
    public class PatternBehave : MonoBehaviour
    {
        public string fileToLoad = "";
        static public GameObject psHit;
        static public GameObject psMiss;
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
            //    this.beatObj.name = "beat" + index;
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
        
        static BeatManager beatManager;
        int playDetectIndexer;
        int playExpiredIndexer;
        int spawnIndexer;
        // Use this for initialization
        void Start()
        {
            psHit = Resources.Load("Beats/psHit") as GameObject;
            psMiss = Resources.Load("Beats/psMiss") as GameObject;
            // debugging
            foreach (SpawnPath s in spawnPaths) {
                Debug.Log(s.spawnPoint+" testng ::" + s.beatObj.name);
            }
            beatManager = GetComponent<BeatManager>();
        //    if (delayFactor == 0) delayFactor = 3;
            delayFactor = BeatManager.getFullNote*5;
            Debug.Log(delayFactor + " factor");

            BeatControllInterface.pressedLeft   += HitLeft;
            BeatControllInterface.pressedUp     += HitUp;
            BeatControllInterface.pressedRight  += HitRight;
            BeatControllInterface.pressedDown   += HitDown;
            BeatControllInterface.pressedWeaponA += HitWeaponA;
            BeatControllInterface.pressedWeaponB += HitWeaponB;
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
            beatManager.LoadRecordedKeys(fileToLoad);
            // call start all music
            beginPlayTime = Time.time;
            playDetectIndexer = 0;
            playExpiredIndexer = 0;
            spawnIndexer = 0;
            patternPlayState = PatternPlayState.play;
            Debug.Log(beatManager.inputAndValue.Count);
        }
        void PatternDetector(float delay)
        {

            if ((Time.time - beginPlayTime) > delay + beatManager.inputAndValue[playDetectIndexer].time - BeatManager.getHalfNote)
            {
                Debug.Log("expect button " + beatManager.inputAndValue[playDetectIndexer].index);
                BeatManager.expectedInputs.Add(beatManager.inputAndValue[playDetectIndexer]);
                Debug.Log("expecting " + BeatManager.expectedInputs.Count);
                playDetectIndexer++;
            }
   //         Debug.Log("time elapse ");
            if ((Time.time - beginPlayTime) > delay + beatManager.inputAndValue[playExpiredIndexer].time + BeatManager.getHalfNote)
            {
                Debug.Log("expired button " + beatManager.inputAndValue[playExpiredIndexer].index);
                if (!beatManager.inputAndValue[playExpiredIndexer].hit) {
                    Root.playerManger.energy -= 0.05f;
                }
                BeatManager.expectedInputs.Remove(beatManager.inputAndValue[playExpiredIndexer]);
                playExpiredIndexer++;
            }
        }
  //      Root.playermanager.enery+
        public static float KeyPressedValidation(int index, float timePressed) {
            Vector3 pos = spawnPaths[index].endPoint.transform.position;
            float timePressedAdjust = Time.time - beginPlayTime;
            GameObject white = Instantiate(psMiss);
            white.transform.position = pos;
            Destroy(white, 1);
            for (int i = 0; i < BeatManager.expectedInputs.Count; i++) {
                if (BeatManager.expectedInputs[i].index == index)
                {
                    BeatManager.expectedInputs[i].hit = true;
                    float raw = timePressedAdjust - (delayFactor + BeatManager.expectedInputs[i].time);
                    Debug.Log(timePressedAdjust + " : " + (delayFactor + BeatManager.expectedInputs[i].time));
                    float abs = Mathf.Abs(raw);
                    Debug.Log("<color=green>hit" + abs + "</color>");
                    GameObject green = Instantiate(psHit);
                    green.transform.position = pos;
                    Destroy(green, 1);
                    BeatManager.expectedInputs.RemoveAt(i);
                    return abs;
                }
            }
            return 2;
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


        public static void HitLeft()
        {
            KeyPressedValidation(0, Time.time);
        }

        public static void HitUp()
        {
            KeyPressedValidation(1, Time.time);
        }

        public static void HitRight()
        {
            KeyPressedValidation(2, Time.time);
        }

        public static void HitDown()
        {
            KeyPressedValidation(3, Time.time);
        }

        public static void HitWeaponA() {
            Root.playerManger.SetWeapon(WeaponType.Poke);
        }
        public static void HitWeaponB() {
            Root.playerManger.SetWeapon(WeaponType.Sweep);
        }
    }
}
