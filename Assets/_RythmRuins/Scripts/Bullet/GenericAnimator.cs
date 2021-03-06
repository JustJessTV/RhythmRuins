﻿using UnityEngine;
using System.Collections;

public class GenericAnimator : MonoBehaviour {
    public string sheetName;
    public float fps;
    public bool loop;
    public bool kill = true;
    public bool testReset;
    public int start = 0;
    public int end = 0;
    AnimNode anSet;
    Sprite[] sprites;
    SpriteRenderer sr;
    public GameObject parent;
    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(sheetName);
        if (end == 0) end = sprites.Length;
        if (loop)
            anSet = new AnimNode(ref sr, sprites, start, end - 1, fps, AnimNodeType.Loop);
        else
            anSet = new AnimNode(ref sr, sprites, start, end - 1, fps, AnimNodeType.Single);
        anSet.Play();
        if(kill)
            anSet.OnComplete = Kill;
    }
	
	// Update is called once per frame
	void Update () {
        anSet.Update();
        if (testReset) {
            testReset = false;
            Reset();
        }
	}
    void Kill() {
        if (parent != null) {
            Destroy(parent);
        }
        else {
            Destroy(gameObject);
        }
    }
    public void Reset() {
        anSet.Play();
    }
}
