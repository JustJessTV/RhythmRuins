using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    public AudioClip audClp;
    AudioSource audSrc;

    public float max, min, avg;
    public float dxMax, dxMin, dxAvg;
    public float pMax, pMin, pAvg;

    GameObject go1;
    GameObject go2;
	// Use this for initialization
	void Start () {
        go1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go2.transform.position = new Vector3(1, 0, 0);
        audSrc = new GameObject("testAudSrc").AddComponent<AudioSource>();
        audSrc.clip = audClp;
        audSrc.Play();
	}
	
	// Update is called once per frame
	void Update () {
        pMin = min;
        float[] spec = AnalyzeSound.GetSpectrum(audSrc, 64, out min, out max, out avg);
        for (int i = 0; i < spec.Length-1; i++) {
            Debug.DrawLine(new Vector3(i, Mathf.Log(spec[i]) + 10, 0), new Vector3(i+1, Mathf.Log(spec[i + 1]) + 10, 0), Color.red);
        }
        if (pMax < max) { 
        
        }
//
//        if (AnalyzeSound.MinChange(ref pMin, ref min) == 1)
//        {
//            dxMin = 1;
//        }
//        else {
//            dxMin *= 0.9f;
//        }
        Vector3 v = new Vector3(0, min, 0);
        go1.transform.position = v;
        go2.transform.position = new Vector3(0, max, 0);
	}
}
