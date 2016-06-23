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
        SetPortraits();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetPortraits(){
        if (PlatformerController.main.character == CharType.Triq) {
            GameObject go = topPic.GetComponent<SwapPromptAnchor>().anchor;
            topPic.GetComponent<SwapPromptAnchor>().anchor = botPic.GetComponent<SwapPromptAnchor>().anchor;
            botPic.GetComponent<SwapPromptAnchor>().anchor = go;
        }
    }
}
