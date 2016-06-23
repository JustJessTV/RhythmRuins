using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class MusicTrackHandler : MonoBehaviour {
    public bool debugFunctions;
    public bool debug02;
    public Vector3 deCubeOriginalPos = Vector3.zero;
    private static Texture2D _texSpectrum;
    public static Texture2D texSpectrum {
        get {
            return _texSpectrum ?? (_texSpectrum = new Texture2D(64, 1));
        }
        set {
            _texSpectrum = value;
        }
    }
    RhythmRealm.BeatManager beatManager;
    AudioSource cubeDrive;
    public enum GameState { 
        idle,
        main,
        transition,
        beginGamePlay,
        gamePlay,
        gameEnd
    }

    public static GameState gameState;

    string MAIN     = "SynthEttriq_BASE_OUTSYNC";
    string ETTA     = "SynthEttriq_BackingETTA";
    string TRIQ     = "SynthEttriq_BackingTRIQ";
    string BASE     = "SynthEttriq_BASE";
    string KICK     = "SynthEttriq_KICK";
    string KODACHI  = "SynthEttriq_MelodyKodachi";
    string SPEAR    = "SynthEttriq_MelodySpear";
    public static bool setLow;
    public static bool setNorm;
    public static float lowPassFilterDelay;

    public delegate void SetLowComplete();
    public static event SetLowComplete setLowComplete;
    public delegate void SetNormComplete();
    public static event SetNormComplete setNormComplete;

    AudioLowPassFilter lowPassFilter;
    AudioReverbFilter reverbFilter;
    IList<SoundTrack> soundTrack = new List<SoundTrack>();
    public class SoundTrack {
        public string name;
        public AudioSource audSrc;
        public AudioClip audClp;
        public float volume{
            get{
                return audSrc.volume;
            }
            set{
                audSrc.volume = value;
            }
        }
        public float lowPass;
        public float wetMix;
        public SoundTrack(AudioClip audClp, string name, GameObject go) {
            this.name           = name;
            this.audClp         = audClp;
            GameObject child    = new GameObject(name);
            child.transform.SetParent(go.transform);
            this.audSrc         = child.AddComponent<AudioSource>();
            this.audSrc.clip    = this.audClp;
            this.audSrc.playOnAwake = false;
            this.audSrc.loop    = true;
        }
    }
    void LoadAllSoundTrack(AudioClip[] clips) {
        lowPassFilter               = gameObject.AddComponent<AudioLowPassFilter>();
        lowPassFilter.lowpassResonanceQ = 3.12f;
        lowPassFilter.cutoffFrequency = 22000;
        reverbFilter                = gameObject.AddComponent<AudioReverbFilter>();
        reverbFilter.reverbPreset   = AudioReverbPreset.Off;
        foreach (AudioClip ac in clips) {
            string name = ac.name;//ac.name.Remove(0,12);
            Debug.Log(name);
            SoundTrack track = new SoundTrack(ac, name, gameObject);
            soundTrack.Add(track);
        }
    }
    void PlayAll(bool set) {
        PlayAll(set, 1.0f);
    }
    void PlayAll(bool set, float volume) {
        foreach (SoundTrack st in soundTrack) {
            st.audSrc.volume = volume;
            if (set)
            {
                st.audSrc.Play();
            }
            else {
                st.audSrc.Stop();
            }
            
        }
    }
    SoundTrack GetTrack(string name) {
        foreach (SoundTrack st in soundTrack) {
            if (st.name.Contains(name)) {
                Debug.Log("found "+name);
                return st;
            }
        }
        Debug.Log("can't find : " + name);
        return null;
    }

    bool LowPass(bool set, float delay) {
        float i = 1;
        if (set) {
            i = -1;
        }
        float dx = i*(Time.deltaTime*Mathf.Pow(delay,-1));
        float t = Mathf.InverseLerp(1120, 22000, lowPassFilter.cutoffFrequency)+dx;
        lowPassFilter.cutoffFrequency = Mathf.Lerp(1120,22000,t);
        if(t >= 1 || t <= 0){
            return true;
        }
        else return false;
    }
	// Use this for initialization
	void Start () {
        texSpectrum = new Texture2D(64, 1, TextureFormat.RGBA32, false);
        Shader.SetGlobalTexture("_SPECTRUM", texSpectrum);
        beatManager = FindObjectOfType<RhythmRealm.BeatManager>().GetComponent<RhythmRealm.BeatManager>();
        
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Tracks") as AudioClip[];
        
        Debug.Log(clips.Length);
        LoadAllSoundTrack(clips);
        PlayAll(true,0);
        AudioSource cubeDrive = soundTrack[0].audSrc;
   //    string[] initTracks = new string[]{
   //        "BASE","KICK"
   //    };
   //    AdjustVolume(initTracks, 1.0f);
	}

    void StateManager() {
        if (cubeDrive != null) {
            if (deCubeOriginalPos == Vector3.zero) {
                deCubeOriginalPos = beatManager.debugCube.transform.position;
            }
            float min, max, avg;
            float[] spec = AnalyzeSound.GetSpectrum(cubeDrive, 64, out min, out max, out avg);
            Color[] clrs = new Color[64];
            for (int i = 0; i < spec.Length; i++)
            {
                clrs[i] = Color.black;
                clrs[i][0] = spec[i];
            }
            texSpectrum.SetPixels(clrs);
            texSpectrum.Apply(false);
            
            beatManager.debugCube.transform.position = new Vector3(0, max, 0) + deCubeOriginalPos;  
        }
        if (gameState == GameState.idle) {
            SoundTrack st = GetTrack(MAIN);
            st.audSrc.volume = 0.5f;
            st.audSrc.pitch = 0.5f;
            gameState = GameState.main;
            cubeDrive = GetTrack(MAIN).audSrc;
        }
        if (gameState == GameState.main) {
            
        }
        if (gameState == GameState.transition) {
            
            SoundTrack st = GetTrack(MAIN);
            st.audSrc.volume -= Time.deltaTime*0.1f;
            st.audSrc.pitch += Time.deltaTime*0.1f;
       //     setLow = true;
            SetLowPassFilter(true, 2);
            if (st.audSrc.pitch >= 1) {
                st.audSrc.volume =0;
                st.audSrc.pitch = 1;
                PlayAll(false);
                PlayAll(true, 0);
                gameState = GameState.beginGamePlay;
            }
        }

        
        if (gameState == GameState.beginGamePlay) {
            GetTrack(BASE).volume += Time.deltaTime*0.5f;
            GetTrack(KICK).volume += Time.deltaTime*0.5f;
            cubeDrive = GetTrack(BASE).audSrc;
            // full bar call back and volume is full
            if (GetTrack(KICK).volume >= 1) {
                GetTrack(KICK).volume = 1;
            //    setNorm = true;
                SetLowPassFilter(false, 2);
                if (GetTrack(TRIQ).volume < 1)
                {
                    GetTrack(TRIQ).volume += Time.deltaTime;
                }
                else
                {
                 //   RhythmRealm.PatternBehave.patternPlayState = RhythmRealm.PatternBehave.PatternPlayState.begin;
                    beatManager.startTimer = true;
                    RhythmRealm.PatternBehave.patternPlayState = RhythmRealm.PatternBehave.PatternPlayState.begin;
                    gameState = GameState.gamePlay;
                 //   cubeDrive = GetTrack(BASE).audSrc;
                }
            }
        }
        
        if (gameState == GameState.gamePlay) {
                     
        }
        if (gameState == GameState.gameEnd) {
            foreach (SoundTrack s in soundTrack)
            {
                if (s.volume > 0)
                {
                    s.volume = 0;
                }
            }
            gameState = GameState.idle;
        }
    }
    void AdjustVolume(string[] names, float volume){
        List<SoundTrack> list = new List<SoundTrack>();
        foreach (string n in names) {
            list.Add(GetTrack(n));
        }
        foreach (SoundTrack st in list) {
            Debug.Log("volume "+st.name);
            st.audSrc.volume = 1.0f;
        }
    }
    bool SwitchFromTo(string fromClip, string toClip, float crossFadeTime) {
        SoundTrack from = GetTrack(fromClip);
        SoundTrack to = GetTrack(toClip);
        float dx = Time.deltaTime * Mathf.Pow(crossFadeTime, -1);
        float t = Mathf.InverseLerp(0, 1, to.audSrc.volume) + dx;
        if (from.audSrc.volume > 0)
        {
            from.audSrc.volume -= dx;
        }
        else from.audSrc.volume = 0;
        to.audSrc.volume    = t;
        if (t >= 1) return true;
        else return false;
    }
    void SetLowPassFilter(bool set, float delay) {
        if (set) setLow = true;
        else setNorm = true;
        lowPassFilterDelay = delay;
    }
    void OnGUI() {
        if(gameState == GameState.main){
            if (GUI.Button(new Rect(0,0,Screen.dpi,Screen.dpi),"Start")) {
                gameState = GameState.transition;
            }
        }
    }
	// Update is called once per frame
	void Update () {
        StateManager();
        if (setLow)
        {
            if (LowPass(true, lowPassFilterDelay)) {
                if (setLowComplete != null)
                {
                    setLowComplete();
                }
                setLow = false;
            }
        }
        if (setNorm) {
            if (LowPass(false,lowPassFilterDelay)) {
                if (setNormComplete != null)
                {
                    setNormComplete();
                }
                    setNorm = false;
                
            }
        }
        if (debugFunctions) {
            if (SwitchFromTo(TRIQ, ETTA, 5.0f)) {
                debugFunctions = false;
            }
        }
        if (debug02) {
            if (SwitchFromTo(ETTA, TRIQ, 5.0f)) {
                debug02 = false;
            }
        }
	}
}
