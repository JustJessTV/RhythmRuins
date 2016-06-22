using UnityEngine;
using System.Collections;

public class PlayerManager{
    public PlayerController p1;
    public PlayerController p2;
    public PlayerManager() {
        Root.playerManger = this;
        GameObject go = new GameObject("Players");
        p1 = go.AddComponent<PlayerController>();
        p2 = go.AddComponent<PlayerController>();
    }
    public void SetPlayer(CharType character) {
        PlatformerController.main.SetChararacter(character);
    }
    public void SetWeapon(WeaponType weapon) {
        PlatformerController.main.SetWeapon(weapon);
    }
}
