using UnityEngine;
using System.Collections;

public class ExplosionLogic : MonoBehaviour {

    AnimNode anBoom;
    Sprite[] sprites;
    SpriteRenderer sr;
    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("effectSprites");
        anBoom = new AnimNode(ref sr, sprites, 0, sprites.Length-1,12 , AnimNodeType.Single);
        anBoom.Play();
        anBoom.OnComplete = Kill;
    }
	
	// Update is called once per frame
	void Update () {
        anBoom.Update();
	}
    void Kill() {
        Destroy(gameObject);
    }
}
