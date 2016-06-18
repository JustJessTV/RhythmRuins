using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
namespace RhythmRealm
{
    public class PatternGenerator : MonoBehaviour
    {
        BPMCalculator bpmCalculator;
        BeatManager beatManager;

        public AudioSource audSrc;
        public AudioClip audClip;
        public static int bpm;
        Rect rectLayout;
        IList<string> keys;
        bool keyCodeSet;

        bool debugPlayBack;
        float debugPlayBackTime;
        int debugPlayBackIndexer;
        // Use this for initialization
        void Start()
        {
            beatManager = GetComponent<BeatManager>();
            rectLayout = new Rect(Screen.dpi * 0.1f, Screen.height * 0.5f, Screen.width * 0.9f, Screen.height * 0.9f);
            string s = "a";
            keys = new List<string>();
            keys.Add(s);
            bpmCalculator = GetComponent<BPMCalculator>();
            if (audSrc == null) {
                audSrc = new GameObject("audSrc").AddComponent<AudioSource>();
            }
            audSrc.clip = audClip;
        }

        // Update is called once per frame
        void Update()
        {
            if (keyCodeSet)
            {
                int amount = keys.Count;
                float spacing = (float)8/(float)amount;
                for(int i = 0; i < keys.Count; i++){
                string s = keys[i];

                    if (Input.GetKeyDown(s))
                    {
                        beatManager.RecordKey(i);
                        Debug.Log("key ' " + s + " ' is pressed");
                        GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        test.transform.position = new Vector3(-3 + (spacing * i), 3, 0);
                        Destroy(test, 0.1f);
                    }

                }
            }
            if (!audSrc.isPlaying)
            {
                beatManager.startTimer = false;
            }
            else {
                float min, max, avg;
                float[] spec = AnalyzeSound.GetSpectrum(audSrc, 64, out min, out max, out avg);    
                beatManager.debugCube.transform.position = new Vector3(0, max, 0);
            }
        }

        void OnGUI() {
            if (GUI.Button(new Rect(0, 0, 50, 50), "menu")) {
                keyCodeSet = !keyCodeSet; 
            }
            if (keyCodeSet)
            {
                //keyCodeSet =bpmCalculator.Draw();
                //return;
                GUILayout.BeginArea(rectLayout);
                if (GUILayout.Button("finish and analyze inputs")) {
                    beatManager.AnalyzeRecordedKeys();
                    audSrc.Stop();
                    beatManager.startTimer = false;
                 //   keyCodeSet = false;
                }
                if (!beatManager.startTimer) {
                    if (GUILayout.Button("Playback debug")) {
                        audSrc.Stop();
                        audSrc.Play();
                        debugPlayBackTime = Time.time;
                        debugPlayBackIndexer = 0;
                        debugPlayBack = true;
                        Debug.Log(beatManager.inputAndValue.Count);
                    }
                    if (debugPlayBack) {
                        int amount = keys.Count;
                        float spacing = (float)8 / (float)amount;
                        if (debugPlayBackIndexer < beatManager.inputAndValue.Count)
                        {
                            if (Time.time - debugPlayBackTime > beatManager.inputAndValue[debugPlayBackIndexer].time)
                            {

                                GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                float x = -3 + (spacing * beatManager.inputAndValue[debugPlayBackIndexer].index);
                                test.transform.position = new Vector3(x, 3, 0);
                                Destroy(test, 0.1f);
                                debugPlayBackIndexer++;
                            }
                        }
                        else { debugPlayBack = false; }
                    }
                }
                GUILayout.EndArea();
            }
            else
            {
                GUILayout.BeginArea(rectLayout);
                if (audClip == null) {
                    if (GUILayout.Button("Please assign AudioClip\nand press button")) {
                        audSrc.clip = audClip;
                    }
                }
                else { 
                GUILayout.Box("audio : " + audClip.name);
                    }
                GUILayout.BeginHorizontal();
                GUILayout.Box("BPM");
                bpm = BeatManager.BPM;
                if (GUILayout.RepeatButton("<<")) {
                    if (bpm > 1){
                        bpm--;
                    }
                }
                if(GUILayout.Button("-")){
                    if (bpm > 1)
                    {
                        bpm--;
                    }
                }
                GUILayout.Box(bpm.ToString());
                if (GUILayout.Button("+")) {
                    bpm++;
                }
                if (GUILayout.RepeatButton(">>")) {
                    bpm++;
                }
                BeatManager.BPM = bpm;
                GUILayout.EndHorizontal();
                GUILayout.Box("Amount of keys being used");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(" - "))
                {
                    if (keys.Count > 1)
                    {
                        keys.RemoveAt(keys.Count - 1);
                    }
                }
                GUILayout.Label(keys.Count + " buttons");
                if (GUILayout.Button(" + "))
                {
                    keys.Add("");
                }
                GUILayout.EndHorizontal();
                for (int i = 0; i < keys.Count; i++)
                {
                    keys[i] = GUILayout.TextField(keys[i]);
                }
                if (GUILayout.Button("Begin AudioClip &\nSet Key Code"))
                {
                    audSrc.Stop();
                    audSrc.Play();
                    beatManager.setBPM = true;
                    beatManager.startTimer = true;
                    keyCodeSet = true;
                    for (int i = 0; i < keys.Count; i++) { 
                        
                    }
                }
        //        if (GUILayout.Button("Start timer")) {
        //            beatManager.startTimer = true;
        //            keyCodeSet = true;
        //        }
                GUILayout.EndArea();
            }
        }
    }
}
