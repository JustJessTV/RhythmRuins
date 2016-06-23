using UnityEngine;
using System.Collections;
using Rewired;
[RequireComponent(typeof(CharacterController))]
public class PlatformerController : PlatformerPhysics {
    public static PlatformerController main;
    public PlatformerAnimation pm;
    Sprite[] sprites;
    AnimNode anRun;
    AnimNode anIdle;
    AnimNode anCurrent;
    Player player;
    public bool swap;
    public CharType character;
    public WeaponType weapon;
    bool running = false;
    void Awake() {
        main = this;
        base.Awake();
        player = ReInput.players.GetPlayer(0);
        Root.playerManger.onSwitchPlayer += SwapCharacters;
        Root.playerManger.onSwitchWeapon += SetWeapon;
        GameStateHandler.beginPlay += SpawnPlayer;
        pm.sr.enabled = false;
    }
    public void SetWeapon(WeaponType weapon) {
        this.weapon = weapon;
        pm.UpdateSpriteSet();
        Debug.Log("Switching Weapon " + weapon.ToString());
    }
    void SwapCharacters(CharType character) {
        this.character = character;
        pm.UpdateSpriteSet();
        player = ReInput.players.GetPlayer((int)character);
    }
    void SpawnPlayer() {
        Vector3 temp = transform.position;
        temp.x = Camera.main.transform.position.x;
        temp.y = 11;
        transform.position = temp;
        pm.sr.enabled = true;
        Jump();
        JumpRelease();
    }
    void Update() {
        float camX = Camera.main.transform.position.x;
        if (transform.position.x > camX + 10) {
            Vector3 temp = transform.position;
            temp.x = camX + 10;
            transform.position = temp;
        }
        if (transform.position.x < camX - 10) {
            Vector3 temp = transform.position;
            temp.x = camX - 10;
            transform.position = temp;
        }
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
        if (player.GetButtonDown("Swap")) {
            Root.playerManger.SetWeapon(WeaponType.Poke);
            //swap = true;
        }
        if (player.GetButtonDown("Pause")) {
            if (GameStateHandler.gameState == GameStateHandler.State.main)
                GameStateHandler.BeginGame();
        }
        Debug.DrawRay(transform.position, look);
        base.Update();
    }
}
