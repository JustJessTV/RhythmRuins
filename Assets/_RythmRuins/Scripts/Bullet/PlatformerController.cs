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
    }
    void Update() {
        //anCurrent.Update();
        float move = player.GetAxis("MoveHorizontal");
        Move(move);
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
