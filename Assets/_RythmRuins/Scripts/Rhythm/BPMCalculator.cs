using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RhythmRealm
{
    public class BPMCalculator : MonoBehaviour
    {

        float x, y, w, h;
        Rect rect;

        float timeOfFirstTap;
        public List<float> hits = new List<float>();
        int hitCount;
        int result;

        enum States
        {
            idle,
            record,
            complete
        }
        States state;

        void Start()
        {
           
        }

        void Update()
        {

        }

       public bool Draw()
        {
            x = 0;
            y = Screen.height * 0.5f;
            w = Screen.width;
            h = Screen.height * 0.5f;
            
            rect = new Rect(x, y, w, h);

            if (state == States.idle)
            {
                GUILayout.BeginArea(rect);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-"))
                {
                    if (hits.Count > 0)
                    {
                        hits.RemoveAt(hits.Count - 1);
                    }
                }
                GUILayout.Box(hits.Count.ToString());
                if (GUILayout.Button(" + "))
                {
                    hits.Add(0.0f);
                }
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Ready"))
                {
                    hitCount = 0;
                    timeOfFirstTap = 0;
                    state = States.record;
                }
                GUILayout.EndArea();
            }
            if (state == States.record)
            {
                Debug.Log("hitCount == " + hitCount + "hit.Count == " + hits.Count.ToString());
                if (hitCount < hits.Count)
                {
                    if (GUI.Button(rect, "Tap"))
                    {
                        if (timeOfFirstTap == 0) timeOfFirstTap = Time.time;
                        float t = Time.time - timeOfFirstTap;
                        hits[hitCount] = t;
                        hitCount++;
                    }
                }
                else
                {
                    float totalSecs = hits[hits.Count - 1];
                    Debug.Log("total secs" + totalSecs);
                    float beatsInASecond = (float)hits.Count / totalSecs;
                 
                    result = Mathf.RoundToInt(beatsInASecond * 60.0f);
                    state = States.complete;
                    Debug.Log("bpm complete calculate"+beatsInASecond);
                }
            }
            if (state == States.complete)
            {
                GUILayout.BeginArea(rect);
                GUILayout.Box(result.ToString() + " BPM");
                if (GUILayout.Button("done"))
                {
                    state = States.idle;
                    return false;
                }
                GUILayout.EndArea();
            }
            return true;
        }
    }
}
