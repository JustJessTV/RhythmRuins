using UnityEngine;
using System.Collections;
using Rewired;

public class BeatControllInterface : MonoBehaviour {
    // ActionL0 == up;
    // ActionL1 == left;
    // ActionL2 == down;
    // ActionR1 == Y;
    // ActionR2 == B;
    // ActionR3 == A;

    public delegate void PressedLeft();
    public static event PressedLeft pressedLeft;

    public delegate void PressedUp();
    public static event PressedUp pressedUp;

    public delegate void PressedRight();
    public static event PressedRight pressedRight;

    public delegate void PressedDown();
    public static event PressedDown pressedDown;

    public delegate void PressedWeaponA();
    public static event PressedWeaponA pressedWeaponA;

    public delegate void PressedWeaponB();
    public static event PressedWeaponB pressedWeaponB;

    Player player;
    void Awake() {
    }
    void Start() {
        player = ReInput.players.GetPlayer(1);
        Root.playerManger.onSwitchPlayer += SwapPlayer;
    }
	void Update () {
        ButtonDetect();
	}
    void SwapPlayer(CharType character) {
        player = ReInput.players.GetPlayer(1-(int)character);
    }
    void ButtonDetect() {
        if (player.GetButtonDown("Left") || Input.GetKeyDown (KeyCode.LeftArrow)) {
            if (pressedLeft != null) pressedLeft();
        }
        if (player.GetButtonDown("Up") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (pressedUp != null) pressedUp();
        }
        if (player.GetButtonDown("Right") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (pressedRight != null) pressedRight();
        }
        if (player.GetButtonDown("Down") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (pressedDown != null) pressedDown();
        }
        if (player.GetButtonDown("WeaponA") || Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (pressedWeaponA != null) pressedWeaponA();
            Root.playerManger.SetWeapon(WeaponType.Poke);
        }
        if (player.GetButtonDown("WeaponB") || Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (pressedWeaponB != null) pressedWeaponB();
            Root.playerManger.SetWeapon(WeaponType.Sweep);
        }
    }
}
