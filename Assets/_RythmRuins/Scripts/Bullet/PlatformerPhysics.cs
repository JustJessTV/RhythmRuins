using UnityEngine;
using System.Collections;

public class PlatformerPhysics:ActorPhysics{
    bool lastFrameShot = false;
    public void Awake() {
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
    public void Shoot(Vector3 dir, Vector3 pos) {
        float magnitude = dir.magnitude;
        if (magnitude < 1) {
            lastFrameShot = false;
            return;
        }
        if (lastFrameShot) return;
        lastFrameShot = true;
        dir.Normalize();
        Object obj = Resources.Load("ShotStraight");
        Instantiate( obj as GameObject,
            pos, Quaternion.LookRotation(dir));

    }
}
