using UnityEngine;
using System.Collections;

public class TimeControl : MonoBehaviour {

    public delegate void PauseGameEvent();
    public static event PauseGameEvent pauseGameCallBack;

    public delegate void UnPauseGameEvent();
    public static event UnPauseGameEvent unPauseGameCallBack;

    public static bool isPaused;

    private static float unscaledPauseTime;
    
    private static float _timeScale;
    private static float timeScale {
        get {
            return _timeScale;
        }
        set {
            _timeScale = value;
        }
    }
    public static void SetTimeScale(float t) {
        timeScale = t;
        Time.timeScale = timeScale;
    }

    // Please call this to Pause game;
    public static void PauseGame() {
        isPaused = true;
        unscaledPauseTime = Time.unscaledTime;
        Time.timeScale = 0;
        if (pauseGameCallBack != null) pauseGameCallBack();
    }

    // Call this to unPause;
    public static void UnPauseGame() {
        isPaused = false;
        Time.timeScale = timeScale;
        if (unPauseGameCallBack != null) unPauseGameCallBack();
    }

    public static float TotalPausedTime {
        get{
            return Time.unscaledTime - unscaledPauseTime;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
