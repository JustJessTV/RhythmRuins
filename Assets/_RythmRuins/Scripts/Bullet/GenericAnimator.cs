using UnityEngine;
using System.Collections;

public class GenericAnimator : MonoBehaviour {
    public string sheetName;
    public float fps;
    AnimNode anSet;
    Sprite[] sprites;
    SpriteRenderer sr;
    public GameObject parent;
    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(sheetName);
        anSet = new AnimNode(ref sr, sprites, 0, sprites.Length-1,fps , AnimNodeType.Single);
        anSet.Play();
        anSet.OnComplete = Kill;
    }
	
	// Update is called once per frame
	void Update () {
        anSet.Update();
	}
    void Kill() {
        if (parent != null) {
            Destroy(parent);
        }
        else {
            Destroy(gameObject);
        }
    }
}
