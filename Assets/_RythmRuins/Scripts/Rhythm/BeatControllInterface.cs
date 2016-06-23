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
    Player player;

    void Start() {
        player = ReInput.players.GetPlayer(0);
    }
	void Update () {
        ButtonDetect();
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
    }
}
