using UnityEngine;
using System.Collections;

public class SwapPromptAnchor : MonoBehaviour {
    public GameObject anchor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = anchor.transform.position;
	}
}
