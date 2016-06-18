using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AnalyzeSound : MonoBehaviour {

    AudioMixer audMix;
    AudioSource audSrc;
    AudioClip audClp;

    public float[] spectrum = new float[64];
    public GameObject testGO;
    public GameObject testGoLow;
    public GameObject testGoMid;
    // Use this for initialization
    void Start () {
        audSrc = GetComponent<AudioSource>();
        audClp = audSrc.clip;
	}

    public static float[] GetSpectrum(AudioSource audSrc, int arraySize, out float max, out float min){
        float avg = -1;
        return GetSpectrum(audSrc, arraySize, out max, out min, out avg);
    }
    public static float[] GetSpectrum(AudioSource audSrc,int arraySize, out float max,out float min, out float avg) {
        float[] spectrum = new float[arraySize];
        audSrc.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        max = float.MinValue;
        min = float.MaxValue;
        avg = 0.0f;
        float sum = 0.0f;
        int size = spectrum.Length;
        if (avg != -1)
        {
            for (int j = 0; j < size; j++)
            {
                if (spectrum[j] > max) max = spectrum[j];
                if (spectrum[j] < min &&
                    spectrum[j] > 0)
                {
                    min = spectrum[j];
                }
                sum += spectrum[j];
            }
            avg = sum / size;
        }
        else { avg = -1; }
        return spectrum;
    }
	
}

