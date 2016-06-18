using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class BeatManager : MonoBehaviour {
   
    public static int BPM;
    private static decimal fullNote;
    private static decimal quaterNote;
    private static decimal halfNote;

    delegate void HitFullNote();
    event HitFullNote hitFullNote;
    delegate void HitHalfNote();
    event HitHalfNote hitHalfNote;
    delegate void FinishAnalyzeRecordedKeys();
    event FinishAnalyzeRecordedKeys finishedAnalyzeRecordedKeys;

    public bool setBPM;
    public bool startTimer;
    public bool testHit;

    private static decimal sessionTimer;
    private decimal fullNoteThreshold;
    private decimal quaterNoteThreshold;

    public GameObject debugCube;
    public enum Notes {
        full,
        half,
        quater
    }
    public Notes note;

    List<float> _recordedKeys;
    public List<float> recordedKeys{
        get{
            return _recordedKeys ?? (_recordedKeys = new List<float>());
        }
        set{
            _recordedKeys = value;
        }
    }

    public class InputAndValue {
        public int index;
        public float time;
        public InputAndValue(int index, float time) {
            this.index = index;
            this.time = time;
        }
    }
    List<InputAndValue> _inputAndValue;
    public List<InputAndValue> inputAndValue {
        get {
            return _inputAndValue ?? (_inputAndValue = new List<InputAndValue>());
        }
        set {
            _inputAndValue = value;
        }
    }
   // public Dictionary<int, float> inputAndValue = new Dictionary<int, float>();

	// Use this for initialization
	void Start () {
        hitFullNote += FullNoteDebug;
        debugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (BPM == 0) BPM = 140;
	}
	
	// Update is called once per frame
	void Update () {
        if (setBPM) {
            CalculateBeat(BPM, out fullNote, out quaterNote);
            setBPM = false;
        }
        if (startTimer)
        {
            TimerCallBack();
        }
        else {
            sessionTimer = (decimal)Time.time;
            fullNoteThreshold = fullNote;
        }
        if (testHit) {
            Debug.Log( GetNoteHit(Notes.full) );
            testHit = false;
        }
	}

    void CalculateBeat(int bpm, out decimal fullNote, out decimal quaterNote) {
        if (bpm <= 0) {
            Debug.LogError("Yo Playa~ BPM has to be greater than zero!");
            fullNote = 0.0m;
            quaterNote = 0.0m;
            return;
        }
        float temp  = 60.0f / (float)bpm;
        fullNote    = (decimal)temp;
        quaterNote = (decimal)(temp * 0.25f);
        Debug.Log("BPM : " + bpm + " f : " + fullNote + " q : " + quaterNote);
    }

    void TimerCallBack() {
        decimal t = (decimal)Time.time - sessionTimer;
        if (t>=fullNoteThreshold) {
            if (hitFullNote != null) {
                hitFullNote();
            }
            fullNoteThreshold += fullNote;
        }
    
    }

    void FullNoteDebug() {
        decimal t = (decimal)Time.time - sessionTimer;
  //      Debug.Log("Beep!" + t.ToString() + " d : " +Time.deltaTime);
        Color c = new Color(Random.Range(0.2f,1.0f),
            Random.Range(0.2f,1.0f),
            Random.Range(0.2f,1.0f));
        debugCube.GetComponent<MeshRenderer>().material.color = c;
    }

    public void RecordKey(int index) {
        float keyTime = Time.time - (float)sessionTimer;
        recordedKeys.Add(keyTime);
        InputAndValue iv = new InputAndValue(index, keyTime);
        inputAndValue.Add(iv);
    }
    public void AnalyzeRecordedKeys() {
        SimpleJSON.JSONClass json = new JSONClass();
        for (int i = 0; i < recordedKeys.Count; i++) {
            decimal t = (decimal)recordedKeys[i];
            decimal r = (decimal)t % quaterNote;
            if (r > quaterNote * 0.5m)
            {
                recordedKeys[i] = (float)((t - r) + quaterNote);
            }
            else { 
                recordedKeys[i] = (float)(t - r);
            }
            inputAndValue[i].time       = recordedKeys[i];
            json[i]["value"].AsFloat    = inputAndValue[i].time;
            json[i]["button"].AsInt     = inputAndValue[i].index;

            Debug.Log(inputAndValue[i].index + " : " + inputAndValue[i].time);
        }
        string jsonString = json.ToString();
        //NOTE:

        // should convert to jSon here
    }

    public static float GetNoteHit(Notes type) {
        decimal t = (decimal)Time.time - sessionTimer;
        switch (type) {
            case Notes.full:
                return (float)(t % fullNote);
            case Notes.half:
                return (float)(t % halfNote);
            case Notes.quater:
                return (float)(t % quaterNote);
            default:
                return 1.0f;

        }
    }
}
