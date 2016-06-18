using UnityEngine;
using System.Collections;

public class AnimNode{
    public AnimNodeState state=AnimNodeState.Idle;
    public AnimNodeType type;
    public AnimNode PreAnimation;
    public AnimNode PostAnimation;
    public System.Action OnComplete;
    public SpriteRenderer sr;
    Sprite[] frames;
    float fps;
    float spf;
    float timeTIllNextFrame;
    int index;
    public AnimNode(ref SpriteRenderer sr, Sprite[] frames, int start, int end, float fps, AnimNodeType type) {
        this.frames = SubSet(ref frames, start, end);
        this.fps = fps;
        spf = 1 / fps;
        this.type=type;
        this.sr = sr;
    }
    public void Update() {
        switch (state) {
            case AnimNodeState.Playing:
                FrameCalculate();
                break;
            case AnimNodeState.WaitingPre:
                if (PreAnimation != null) PreAnimation.Update();
                break;
            case AnimNodeState.WaitingPost:
                if (PostAnimation != null) PostAnimation.Update();
                break;

        }
    }
    void FrameCalculate() {
        if (timeTIllNextFrame < Time.time) {
            timeTIllNextFrame += spf;
            index++;
            if (index > frames.Length&&type==AnimNodeType.Single) {
                TryComplete();
                return;
            }
            sr.sprite = frames[index % frames.Length];
        }
    }
    public void TryComplete() {
        if (PostAnimation != null) {
            state = AnimNodeState.WaitingPost;
            PostAnimation.Play();
            PostAnimation.OnComplete = SubAnimComplete;
        }
        else {
            Finalize();
        }
    }
    public void Transition(System.Action OnComplete) {
        this.OnComplete = OnComplete;
        TryComplete();
    }
    public void Play() {
        if (state != AnimNodeState.Idle) return;
        sr.sprite = frames[0];
        if (PreAnimation != null) {
            PreAnimation.Play();
            state = AnimNodeState.WaitingPre;
            PreAnimation.OnComplete = SubAnimComplete;
        }
        else {
            Start();
        }
    }
    void Start() {
        state = AnimNodeState.Playing;
        index = 0;
        timeTIllNextFrame = Time.time + spf;
    }
    void Finalize() {
        state = AnimNodeState.Idle;
        if (OnComplete != null)
            OnComplete();
    }
    private void SubAnimComplete() {
        if (state == AnimNodeState.WaitingPre) {
            Start();
        }
        if (state == AnimNodeState.WaitingPost) {
            Finalize();
        }
    }
    Sprite[] SubSet(ref Sprite[] sprites, int start, int end) {
        Sprite[] subSprite = new Sprite[end - start + 1];
        for (int x = start; x < end + 1; x++) {
            subSprite[x - start] = sprites[x];
        }
        return subSprite;
    }
}
