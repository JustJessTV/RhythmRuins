using UnityEngine;
using System.Collections;

public class PlatformerAnimation : MonoBehaviour {
    PlatformerController pc;
    Sprite[] sprites;
    public SpriteRenderer sr;
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
        pc.pm = this;

        pc.onSwapDirection  += SwapDirection;
        pc.onStartRun       += StartRun;
        pc.onStopRun        += StopRun;
        pc.onStartJump      += StartJump;
        pc.onStopJump       += StopJump;
        pc.onStartAttack    += StartAttack;
        pc.onStartHurt      += StartHurt;
	}
    void Start() {
        BuildSpriteLib(Root.main.animSets.ETTA_SWEEP);
        pc.OnDeath += Death;
    }
    void Death() {
        if (pc.character == CharType.Triq) {
            sr.sprite = sprites[22];
        }
        else {
            sr.sprite = sprites[17];
        }
        pc.invuln = true;
        anCurrent = null;
    }
    public void UpdateSpriteSet() {
        switch (pc.character) {
            case CharType.Etta:
                switch (pc.weapon) {
                    case WeaponType.Bare:
                        BuildSpriteLib(Root.main.animSets.ETTA_BARE);
                        break;
                    case WeaponType.Poke:
                        BuildSpriteLib(Root.main.animSets.ETTA_POKE);
                        break;
                    case WeaponType.Sweep:
                        BuildSpriteLib(Root.main.animSets.ETTA_SWEEP);
                        break;
                }
                break;
            case CharType.Triq:
                BuildSpriteLib(Root.main.animSets.TRIQ_SWEEP);
                switch (pc.weapon) {
                    case WeaponType.Bare:
                        BuildSpriteLib(Root.main.animSets.TRIQ_BARE);
                        break;
                    case WeaponType.Poke:
                        BuildSpriteLib(Root.main.animSets.TRIQ_POKE);
                        break;
                    case WeaponType.Sweep:
                        BuildSpriteLib(Root.main.animSets.TRIQ_SWEEP);
                        break;
                }
                break;
        }
    }
    public void BuildSpriteLib(AnimSets.AnimSet animSet) {

        sprites                 = Resources.LoadAll<Sprite>(animSet.fileName);
        anIdle                  = new AnimNode(ref sr, sprites, animSet.idle,       7, AnimNodeType.Loop);
        anRun                   = new AnimNode(ref sr, sprites, animSet.run,        10, AnimNodeType.Loop);
        anJump                  = new AnimNode(ref sr, sprites, animSet.jump,       10, AnimNodeType.Loop);
        anAttack                = new AnimNode(ref sr, sprites, animSet.attack,     10, AnimNodeType.Single);
        anHurt                  = new AnimNode(ref sr, sprites, animSet.hurt,       05, AnimNodeType.Single);

        anRun.PreAnimation      = new AnimNode(ref sr, sprites, animSet.runPre,     20, AnimNodeType.Single);
        anRun.PostAnimation     = new AnimNode(ref sr, sprites, animSet.runPost,    10, AnimNodeType.Single);

        anJump.PreAnimation     = new AnimNode(ref sr, sprites, animSet.jumpPre,    10, AnimNodeType.Single);
        anJump.PostAnimation    = new AnimNode(ref sr, sprites, animSet.jumpPost,   10, AnimNodeType.Single);

        anIdle.Play();

        anCurrent = anIdle;
    }
	// Update is called once per frame
	void Update () {
        if (anCurrent != null) {
            anCurrent.Update();
        }
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
