using UnityEngine;
using System.Collections;

public class PlayerController : ActorPhysics {

    void Awake() {
        base.info = new ActorInfo() {
            runSpeed = 1.7f,
            gravity = 1,
            jumpPower = 32,
            friction = 10,
            airFriction = 2,
            airSpeed = 0.4f
        };
        base.Awake();
    }
    void Update() {
        if (Input.GetKey("w")) {

        }
        if (Input.GetKey("s")) {

        }
        if (Input.GetKey("a")) {
            Move(-1);
        }
        if (Input.GetKey("d")) {
            Move(1);
        }
        if (Input.GetKeyDown("space")) {
            Jump();
        }
        if (Input.GetKeyUp("space")) {
            JumpRelease();
        }
        base.Update();
    }
}
