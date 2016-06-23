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
    public float startTime;
    public float finalScore;
    void Awake() {
        main = this;
        base.Awake();
        base.OnDeath += Death;
    }
    void Start() {
        pm.sr.enabled = false;
        player = ReInput.players.GetPlayer(0);
    }
    void OnEnable() {

        Root.playerManger.onSwitchPlayer += SwapCharacters;
        Root.playerManger.onSwitchWeapon += SetWeapon;
        GameStateHandler.beginPlay += SpawnPlayer;
    }
    void OnDisable() {

        Root.playerManger.onSwitchPlayer -= SwapCharacters;
        Root.playerManger.onSwitchWeapon -= SetWeapon;
        GameStateHandler.beginPlay -= SpawnPlayer;
    }
    void Death() {
        finalScore = Time.time - startTime;
        Debug.Log("You got a score of: " + (finalScore));
        GameStateHandler.EndGame();
    }
    public void SetWeapon(WeaponType weapon) {
        this.weapon = weapon;
        pm.UpdateSpriteSet();
        Debug.Log("Switching Weapon " + weapon.ToString());
    }
    void SwapCharacters(CharType character) {
        if (this.character == character) return;
        invuln = false;
        float temp = hp;
        hp = reserveHP;
        reserveHP = temp;
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
        startTime = Time.time;
    }
    void Update() {
        if (player.GetButtonDown("Pause")) {
            if (GameStateHandler.gameState == GameStateHandler.State.main)
                GameStateHandler.BeginGame();
            if (GameStateHandler.gameState == GameStateHandler.State.gameOver) {
                GameStateHandler.gameState = GameStateHandler.State.transitionToMain;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
        if (GameStateHandler.gameState != GameStateHandler.State.gamePlaying) return;
        reserveHP = Mathf.Clamp01(reserveHP + Time.deltaTime * 0.033f);
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
            //Root.playerManger.SetWeapon(WeaponType.Poke);
            UnityEngine.SceneManagement.SceneManager.UnloadScene(0);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            //swap = true;
        }
        Debug.DrawRay(transform.position, look);
        base.Update();
    }
}
