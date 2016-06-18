using UnityEngine;
using System.Collections;
using Rewired;
[RequireComponent(typeof(CharacterController))]
public class PlatformerController : PlatformerPhysics {
    public static PlatformerController main;
    Player player;
    void Awake() {
        main = this;
        base.Awake();
        player = ReInput.players.GetPlayer(0);
    }
    void Update() {
        Move(player.GetAxis("MoveHorizontal"));
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
