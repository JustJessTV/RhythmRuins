using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternBehave : MonoBehaviour {

    public static float delayFactor;
    public static float beginPlayTime;

    public class SpawnPath {
        int buttonIndex;
        Vector3 spawnPoint;
        Vector3 endPoint;
        public SpawnPath(Vector3 spawn, Vector3 end, int index) {
            this.spawnPoint     = spawn;
            this.endPoint       = end;
            this.buttonIndex    = index;
            spawnPaths.Add(this);
        }
    }
    private static List<SpawnPath> _spawnPaths;
    static List<SpawnPath> spawnPaths {
        get {
            return _spawnPaths ?? (_spawnPaths = new List<SpawnPath>());
        }
        set {
            _spawnPaths = value;
        }
    }
    public enum PatternPlayState { 
        idle,
        begin,
        play
    }
    public PatternPlayState patternPlayState;
    public List<GameObject> spawnPoints;

    BeatManager beatManager;
    int playDetectIndexer;
    int spawnIndexer;
	// Use this for initialization
	void Start () {
        beatManager = GetComponent<BeatManager>();
        if (delayFactor == 0) delayFactor = 3;

	}
	
	// Update is called once per frame
	void Update () {
        if (patternPlayState == PatternPlayState.begin){

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
            else { 
                patternPlayState = PatternPlayState.idle;
            }
        }
	}
    void BeginPlay() {
        beatManager.LoadRecordedKeys();
        // call start all music
        beginPlayTime = Time.time;
        playDetectIndexer = 0;
        spawnIndexer = 0;
        patternPlayState = PatternPlayState.play;
        Debug.Log(beatManager.inputAndValue.Count);
    }
    void PatternDetector(float delay) {
        
        if ((Time.time - beginPlayTime) > delay + beatManager.inputAndValue[playDetectIndexer].time)
        {
            playDetectIndexer++;
        }
    }
    void PatternSpawner() {
        BeatManager.Notes[] notes = new BeatManager.Notes[0];
        PatternSpawner(notes);
    }
    void PatternSpawner(BeatManager.Notes[] cencorNotes) {
        if (cencorNotes.Length > 0)
        {
            BeatManager.Notes note;
            beatManager.NoteType(beatManager.inputAndValue[spawnIndexer].time, out note);
            
            foreach (BeatManager.Notes n in cencorNotes) {
                if (n == note) {
                    spawnIndexer++;
                    Debug.Log("skipping "+note);
                    continue;
                }
            }
        }
        if ((Time.time - beginPlayTime) > beatManager.inputAndValue[spawnIndexer].time)
        {
            // spawn here
            Debug.Log("spawn : " + beatManager.inputAndValue[spawnIndexer].time);
            spawnIndexer++;
        }
    }
}
