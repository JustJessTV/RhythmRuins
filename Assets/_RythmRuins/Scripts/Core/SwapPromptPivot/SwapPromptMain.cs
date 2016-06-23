using UnityEngine;
using System.Collections;

public class SwapPromptMain : MonoBehaviour {
    public GameObject pivot;
    public GameObject topPic;
    public GameObject botPic;
	// Use this for initialization
	void Start () {
        transform.parent = Camera.main.transform;
        transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetPortraits(){
        switch (PlatformerController.main.character){
            case CharType.Etta:

                break;
            case CharType.Triq:

                break;
        }
    }
}
