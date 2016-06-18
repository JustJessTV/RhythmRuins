using UnityEngine;
using System.Collections;
using Rewired;
[RequireComponent(typeof(CharacterController))]
public class PlatformerController : PlatformerPhysics {
    public static PlatformerController main;
    Sprite[] sprites;
    SpriteRenderer sr;
    AnimNode anRun;
    AnimNode anIdle;
    AnimNode anCurrent;
    Player player;
    bool running = false;
    void Awake() {
        main = this;
        base.Awake();
        player = ReInput.players.GetPlayer(0);
        sprites = Resources.LoadAll<Sprite>("MegaMan");
        sr = GetComponent<SpriteRenderer>();
        anIdle = new AnimNode(ref sr, sprites, 7, 9, 10, AnimNodeType.Loop);
        anRun = new AnimNode(ref sr, sprites, 41, 50, 12, AnimNodeType.Loop);
        anRun.PreAnimation = new AnimNode(ref sr, sprites, 36, 40, 14, AnimNodeType.Single);
        anRun.PostAnimation = new AnimNode(ref sr, sprites, 30, 36, 14, AnimNodeType.Single);

        anRun.OnComplete = () => {
            anCurrent = anIdle;
            anCurrent.Play();
        };

        anCurrent = anIdle;
        anIdle.Play();
    }
    void Update() {
        anCurrent.Update();
        float move = player.GetAxis("MoveHorizontal");
        Move(move);
        if (running&&move==0) {
            running = false;
            anCurrent.TryComplete();
        }
        if (!running && move != 0) {
            running = true;
            anCurrent = anRun;
            anRun.Play();
        }
        sr.flipX = !isFacingRight;
        if (player.GetButtonDown("Jump")) {
            Jump();
        }
        if (player.GetButtonUp("Jump")) {
            JumpRelease();
        }
        Vector3 look = new Vector3(
            player.GetAxis("LookHorizontal"),
            player.GetAxis("LookVertical"),
            0);
        Shoot(look, transform);
        Debug.DrawRay(transform.position, look);
        base.Update();
    }
}
