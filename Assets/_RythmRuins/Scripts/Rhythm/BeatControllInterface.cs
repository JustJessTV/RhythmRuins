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
        player = ReInput.players.GetPlayer(0);
        Root.playerManger.onSwitchPlayer += SwapPlayer;
    }
	void Update () {
        ButtonDetect();
	}
    void SwapPlayer(CharType character) {
        player = ReInput.players.GetPlayer(1-(int)character);
    }
    void ButtonDetect() {
        if (player.GetButton("Left")) {
            if (pressedLeft != null) pressedLeft();
        }
        if (player.GetButton("Up"))
        {
            if (pressedUp != null) pressedUp();
        }
        if (player.GetButton("Right"))
        {
            if (pressedLeft != null) pressedLeft();
        }
        if (player.GetButton("Down"))
        {
            if (pressedDown != null) pressedDown();
        }
        if (player.GetButton("WeaponA")) {
            if (pressedWeaponA != null) pressedWeaponA();
        }
        if (player.GetButton("WeaponB")) {
            if (pressedWeaponB != null) pressedWeaponB();
        }
    }
}
