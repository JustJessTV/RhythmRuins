using UnityEngine;
using System.Collections;

public class AnimTest : MonoBehaviour {
    SpriteRenderer sr;
    Sprite[] sprites;
    int index;
    int fps = 14;
    float spf;
    float nextUpdate;
    public bool finish = false;
    AnimNode an;
	// Use this for initialization
	void Awake () {
        sr = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("MegaMan");
        an = new AnimNode(ref sr, sprites, 19, 29, 12, AnimNodeType.Loop);
        an.PreAnimation = new AnimNode(ref sr, sprites, 0, 9, 12, AnimNodeType.Single);
        an.PostAnimation = new AnimNode(ref sr, sprites, 87, 90, 12, AnimNodeType.Single);
        an.Play();
	}
	
	// Update is called once per frame
	void Update () {
        an.Update();
        if (finish) {
            finish = false;
            an.TryComplete();
        }
	}
}
