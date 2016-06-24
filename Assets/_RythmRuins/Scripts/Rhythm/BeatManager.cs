using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

namespace RhythmRealm
{
    public class BeatManager : MonoBehaviour
    {

        public static int BPM;

        private static decimal fullNote;
        private static decimal quaterNote;
        private static decimal halfNote;
        public static float getHalfNote {
            get {
                return (float)fullNote*0.5f;
            }
        }

        public static float getFullNote {
            get {
                return (float)fullNote;
            }
        }
        private static float _fullBar;
        public static float getFullBar {
            get {
                return (float)(fullNote * 4);
            }
        }
        public delegate void HitFullBar();
        public static event HitFullBar hitFullBar;
        public delegate void HitFullNote();
        public static event HitFullNote hitFullNote;
        public delegate void HitHalfNote();
        public static event HitHalfNote hitHalfNote;
        public delegate void FinishAnalyzeRecordedKeys();
        public static event FinishAnalyzeRecordedKeys finishedAnalyzeRecordedKeys;

        public bool setBPM;
        public bool startTimer;
        public static bool beginTimer;
        public bool testHit;

        public static decimal sessionTimer;
        private decimal fullNoteThreshold;
        private decimal quaterNoteThreshold;

        public GameObject debugCube;
        public enum Notes
        {
            full,
            half,
            quarter,
            grace
        }
        public Notes note;

        List<float> _recordedKeys;
        public List<float> recordedKeys
        {
            get
            {
                return _recordedKeys ?? (_recordedKeys = new List<float>());
            }
            set
            {
                _recordedKeys = value;
            }
        }

        public class InputAndValue
        {
            public int index;
            public float time;
            public InputAndValue(int index, float time)
            {
                this.index = index;
                this.time = time;
            }
            public bool hit;
        }
        List<InputAndValue> _inputAndValue;
        public List<InputAndValue> inputAndValue
        {
            get
            {
                return _inputAndValue ?? (_inputAndValue = new List<InputAndValue>());
            }
            set
            {
                _inputAndValue = value;
            }
        }
        static List<InputAndValue> _expectedInputs;
        public static List<InputAndValue> expectedInputs {
            get {
                return _expectedInputs ?? (_expectedInputs = new List<InputAndValue>());
            }
            set {
                _expectedInputs = value;
            }
        }
        // public Dictionary<int, float> inputAndValue = new Dictionary<int, float>();

        // Use this for initialization
        void Start()
        {
            hitFullNote += FullNoteDebug;
            debugCube = GameObject.CreatePrimitive(PrimitiveType.Quad);
            debugCube.transform.localScale = Vector3.one * 0.0001f;
            debugCube.layer = 5;
            Destroy(debugCube.GetComponent<BoxCollider>());
            NodePosition nPos = debugCube.AddComponent<NodePosition>();
            nPos.xPercent = 0.5f;
            nPos.yPercent = 0.05f;
            nPos.zDist = 100000;
            nPos.setScreenPercent = true;
            if (BPM == 0) BPM = 140;
            CalculateBeat(BPM, out fullNote, out quaterNote);
        }

        // Update is called once per frame
        void Update()
        {
            if (setBPM)
            {
                CalculateBeat(BPM, out fullNote, out quaterNote);
                setBPM = false;
            }
            if (startTimer)
            {
                TimerCallBack();
                //     Debug.Log(sessionTimer);
            }
            else
            {
                sessionTimer = (decimal)Time.time;
                fullNoteThreshold = 0;
            }
            if (testHit)
            {
                Debug.Log(GetNoteHit(Notes.full));
                testHit = false;
            }
        }

        void CalculateBeat(int bpm, out decimal fullNote, out decimal quaterNote)
        {
            if (bpm <= 0)
            {
                Debug.LogError("Yo Playa~ BPM has to be greater than zero!");
                fullNote = 0.0m;
                quaterNote = 0.0m;
                return;
            }
            float temp = 60.0f / (float)bpm;
            fullNote = (decimal)temp;
            quaterNote = (decimal)(temp * 0.25f);
            Debug.Log("BPM : " + bpm + " f : " + fullNote + " q : " + quaterNote);
        }

        void TimerCallBack()
        {
            decimal t = (decimal)Time.time - sessionTimer;
            if (t >= fullNoteThreshold)
            {
                if (hitFullNote != null)
                {
                    hitFullNote();
                }
                fullNoteThreshold += fullNote;
            }
        }

        void FullNoteDebug()
        {
            decimal t = (decimal)Time.time - sessionTimer;
       //     Debug.Log("Beep!" + t.ToString() + " d : " + Time.deltaTime);
            Color c = new Color(Random.Range(0.2f, 1.0f),
                Random.Range(0.2f, 1.0f),
                Random.Range(0.2f, 1.0f));
            if(debugCube!=null)
            debugCube.GetComponent<MeshRenderer>().material.color = c;
        }

        public void RecordKey(int index)
        {
            float keyTime = Time.time - (float)sessionTimer;
            recordedKeys.Add(keyTime);
            InputAndValue iv = new InputAndValue(index, keyTime);
            inputAndValue.Add(iv);
        }

        public void NoteType(float timeValue, out Notes note)
        {

            decimal r = (decimal)timeValue % fullNote;
            float n = Mathf.InverseLerp(0, (float)fullNote, (float)r);
            Debug.Log("normalized note :" + n);
            float absN = Mathf.Abs(n);
            if (absN <= .05f)
            {
                note = Notes.full;
                return;
            }
            if (absN >= .2f &&
                absN <= .3f)
            {
                note = Notes.quarter;
                return;
            }
            if (absN >= .45f &&
                absN <= .55f)
            {
                note = Notes.half;
            }
            if (absN >= .7f &&
                absN <= .8f)
            {
                note = Notes.quarter;
                return;
            }
            if (absN >= .95f)
            {
                note = Notes.full;
                return;
            }
            else
            {
                note = Notes.grace;
                return;
            }

        }
        public void AnalyzeRecordedKeys()
        {

            JSONNode jnode = new JSONArray();
            for (int i = 0; i < recordedKeys.Count; i++)
            {
                decimal t = (decimal)recordedKeys[i];
                decimal r = (decimal)t % quaterNote;
                if (r > quaterNote * 0.5m)
                {
                    recordedKeys[i] = (float)((t - r) + quaterNote);
                }
                else
                {
                    recordedKeys[i] = (float)(t - r);
                }
                inputAndValue[i].time = recordedKeys[i];

                JSONClass json = new JSONClass();
                json["value"].AsFloat = inputAndValue[i].time;
                json["button"].AsInt = inputAndValue[i].index;
                jnode.Add(json);
                Debug.Log(inputAndValue[i].index + " : " + inputAndValue[i].time);
            }
            PatternPath.SaveNewFile(jnode, PatternGenerator.fileName);
        }
        public void ClearRecordedKeys()
        {
            inputAndValue.Clear();
        }
        public void LoadRecordedKeys(string fileName)
        {
          //  string name = PatternGenerator.fileName;
            Debug.Log(fileName);
            string preJson = PatternPath.LoadPatternFile(fileName);
            JSONNode jc = JSONClass.Parse(preJson) as JSONNode;
            Debug.Log(jc.Count);
            for (int i = 0; i < jc.Count; i++)
            {
                int index = jc[i]["button"].AsInt;
                float time = jc[i]["value"].AsFloat;
                Debug.Log("i :" + index + " t :" + time);
                InputAndValue iv = new InputAndValue(index, time);
                inputAndValue.Add(iv);
            }
        }
        public static float GetNoteHit(Notes type)
        {
            decimal t = (decimal)Time.time - sessionTimer;
            switch (type)
            {
                case Notes.full:
                    return (float)(t % fullNote);
                case Notes.half:
                    return (float)(t % halfNote);
                case Notes.quarter:
                    return (float)(t % quaterNote);
                default:
                    return 1.0f;

            }
        }
    }
}
