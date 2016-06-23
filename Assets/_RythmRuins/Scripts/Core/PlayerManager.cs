using UnityEngine;
using System.Collections;

public class PlayerManager {
    public delegate void CallSwitchPlayer(CharType character);
    public event CallSwitchPlayer onSwitchPlayer;

    public delegate void CallSwitchWeapon(WeaponType weapon);
    public event CallSwitchWeapon onSwitchWeapon;

    public PlayerController p1;
    public PlayerController p2;
    public PlayerManager() {
        Root.playerManger = this;
        GameObject go = new GameObject("Players");
        p1 = go.AddComponent<PlayerController>();
        p2 = go.AddComponent<PlayerController>();
    }
    public void SetPlayer(CharType character) {
        if (onSwitchPlayer != null) onSwitchPlayer(character);
        GameObject.Instantiate(Resources.Load("SwapPrompt"));
    }
    public void SetWeapon(WeaponType weapon) {
        if (storeWeapon == WeaponType.Bare) return;
        if (onSwitchWeapon != null) onSwitchWeapon(weapon);
    }
    WeaponType storeWeapon;
    private float _energy=1;
    public float energyCost = 0.1f;
    public float energy {
        get { return _energy; }
        set {
            if (enoughEnergy&&value<energyCost) {
                storeWeapon = PlatformerController.main.weapon;
                SetWeapon(WeaponType.Bare);
            }
            if (!enoughEnergy && value > energyCost) {
                SetWeapon(storeWeapon);
                storeWeapon = WeaponType.Bare;
            }

            _energy = Mathf.Clamp01(value);
            Debug.Log(_energy);
        }
    }
    public bool enoughEnergy {
        get { return energy > energyCost; }
    }
}
