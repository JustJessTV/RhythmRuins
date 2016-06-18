using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour{
    PlayerRole role;
    float hp;
    bool isP1;
    bool isP2 { get { return !isP1; } }
    //controller
    PlayerController Buddy;
    void Awake() {
        if (Root.playerManger.p1 == null) {
            Root.playerManger.p1 = this;
            isP1 = true;
        }
        else if (Root.playerManger.p2 == null) {
            Root.playerManger.p2 = this;
            Buddy = Root.playerManger.p1;
            Buddy.Buddy = this;
        }
    }
}
