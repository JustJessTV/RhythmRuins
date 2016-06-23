using UnityEngine;
using System.Collections;

public class SwapPromptPivot : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = transform.localRotation.eulerAngles;
        rot.z = Mathf.Lerp(rot.z, 180, Time.deltaTime*5);
        if (rot.z > 179) {
            Destroy(transform.parent.gameObject);
        }
        transform.localRotation = Quaternion.Euler(rot);
	}
}
