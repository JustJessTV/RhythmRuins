using UnityEngine;
using System.Collections;
using Rewired;
[RequireComponent(typeof(CharacterController))]
public class PlatformerController : PlatformerPhysics {
    public static PlatformerController main;
    public PlatformerAnimation pm;
    Sprite[] sprites;
    SpriteRenderer sr;
    AnimNode anRun;
    AnimNode anIdle;
    AnimNode anCurrent;
    Player player;
    public bool swap;
    public CharType character;
    public WeaponType weapon;
    bool running = false;
    public void SetChararacter(CharType character) {
        this.character = character;
        pm.UpdateSpriteSet();
    }
    public void SetWeapon(WeaponType weapon) {
        this.weapon = weapon;
        pm.UpdateSpriteSet();
    }
    void Awake() {
        main = this;
        base.Awake();
        player = ReInput.players.GetPlayer(0);
    }
    void Update() {

        if (swap) {
            swap = false;
            if (PlatformerController.main.character == CharType.Etta) {
                Root.playerManger.SetPlayer(CharType.Triq);
            }
            else {

                Root.playerManger.SetPlayer(CharType.Etta);
            }
        }
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
