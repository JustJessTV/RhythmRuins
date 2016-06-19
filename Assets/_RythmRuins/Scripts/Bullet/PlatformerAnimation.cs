using UnityEngine;
using System.Collections;

public class PlatformerAnimation : MonoBehaviour {
    PlatformerController pc;
    Sprite[] sprites;
    SpriteRenderer sr;
    AnimNode anRun;
    AnimNode anIdle;
    AnimNode anJump;
    AnimNode anCurrent;
    AnimNode anAttack;
    AnimNode anHurt;
    bool invuln {
        get { return pc.invuln; }
        set { pc.invuln = value; }
    }
	// Use this for initialization
	void Awake () {
        pc = GetComponent<PlatformerController>();
        sr = GetComponent<SpriteRenderer>();

        pc.onSwapDirection  += SwapDirection;
        pc.onStartRun       += StartRun;
        pc.onStopRun        += StopRun;
        pc.onStartJump      += StartJump;
        pc.onStopJump       += StopJump;
        pc.onStartAttack    += StartAttack;
        pc.onStartHurt      += StartHurt;

        sprites = Resources.LoadAll<Sprite>("TestF");
        anIdle =    new AnimNode(ref sr, sprites, 1, 1, 10, AnimNodeType.Loop);
        anRun =     new AnimNode(ref sr, sprites, 7, 7, 10, AnimNodeType.Loop);
        anJump =    new AnimNode(ref sr, sprites, 6, 6, 10, AnimNodeType.Loop);
        anAttack =  new AnimNode(ref sr, sprites, 2, 2, 10, AnimNodeType.Single);
        anHurt =    new AnimNode(ref sr, sprites, 3, 3, 05, AnimNodeType.Single);
        anIdle.Play();


        anCurrent = anIdle;
	}
	
	// Update is called once per frame
	void Update () {
        anCurrent.Update();
	}
    void SwapDirection(bool flipX) {
        sr.flipX = flipX;
    }
    void StartRun() {
        if (invuln) return;
        if (!pc.isGrounded) return;
        anRun.Play();
        anCurrent = anRun;
    }
    void StopRun() {
        if (invuln) return;
        if (!pc.isGrounded) return;
        anCurrent.Transition(() => { anIdle.Play(); anCurrent = anIdle; });
    }
    void StartJump() {
        if (invuln) return;
        anCurrent = anJump;
        anJump.Play();
    }
    void StopJump() {
        if (invuln) return;
        if(pc.lastDir==0)
            anCurrent.Transition(() => { anIdle.Play(); anCurrent = anIdle; });
        else
            anCurrent.Transition(() => { anRun.Play(); anCurrent = anRun; });
    }
    void StartAttack() {
        if (invuln) return;
        anCurrent = anAttack;
        anAttack.Play();
        anAttack.OnComplete = StopAttack;
    }
    void StopAttack() {
        if (invuln) return;
        if (pc.lastDir == 0)
            anCurrent.Transition(() => { anIdle.Play(); anCurrent = anIdle; });
        else
            anCurrent.Transition(() => { anRun.Play(); anCurrent = anRun; });
    }
    void StartHurt() {
        invuln = true;
        anCurrent = anHurt;
        anHurt.Play();
        anHurt.OnComplete = StopHurt;
    }
    void StopHurt() {
        invuln = false;
        if (pc.lastDir == 0)
            anCurrent.Transition(() => { anIdle.Play(); anCurrent = anIdle; });
        else
            anCurrent.Transition(() => { anRun.Play(); anCurrent = anRun; });
    }
}
