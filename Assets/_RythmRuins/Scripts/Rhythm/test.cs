using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    public AudioClip audClp;
    AudioSource audSrc;

    public float max, min, avg;
	// Use this for initialization
	void Start () {
        audSrc = new GameObject("testAudSrc").AddComponent<AudioSource>();
        audSrc.clip = audClp;
        audSrc.Play();
	}
	
	// Update is called once per frame
	void Update () {
        float[] spec = AnalyzeSound.GetSpectrum(audSrc, 64, out max, out min, out avg);
        for (int i = 0; i < spec.Length-1; i++) {
            Debug.DrawLine(new Vector3(i, Mathf.Log(spec[i]) + 10, 0), new Vector3(i+1, Mathf.Log(spec[i + 1]) + 10, 0), Color.red);
        }
	}
}
