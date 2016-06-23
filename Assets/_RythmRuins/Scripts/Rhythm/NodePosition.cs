using UnityEngine;
using System.Collections;

namespace RhythmRealm{
    public class NodePosition : MonoBehaviour {

        public bool setScreenPercent;
        public float xPercent, yPercent;
        public float zDist;
        public string name;
        Camera cam;
	    // Use this for initialization
	    void Start () {
            cam = GameRhythmManager.rhythmCam;
	    }
	
	    // Update is called once per frame
	    void Update () {
            if (setScreenPercent) {
                Vector3 screen = new Vector3(Screen.width*xPercent,Screen.height*yPercent,zDist);
                Vector3 pos = cam.ScreenToWorldPoint(screen);
                transform.position = pos;
                setScreenPercent = false;
            }
	    }
    }
}
