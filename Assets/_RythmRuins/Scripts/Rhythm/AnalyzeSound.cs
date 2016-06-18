using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Linq;

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
    public static float[] GetSpectrum(AudioSource audSrc,int arraySize, out float min,out float max, out float avg) {
        float[] spectrum = new float[arraySize];
        audSrc.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        int size = spectrum.Length;
        float sum = 0;
        max = float.MinValue;
        min = float.MaxValue;
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

        return spectrum;
    }
    public static int MinChange(ref float prevMin, ref float currMin){
        return valueChange(ref prevMin, ref currMin);
    }
    public static int MaxChange(ref float prevMax, ref float currMax){
        return valueChange(ref prevMax, ref currMax);
    }
    static int valueChange(ref float prev, ref float cur)
    {
        if (cur > prev){
            return 1;
        }
        
        else {
            return -1;
        }
    }
}

