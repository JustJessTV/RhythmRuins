using UnityEngine;
using System.Collections;

public class Root : DispatchEvent {
    public static PlayerManager playerManger;
    public override void Awake()
    {
       new PlayerManager();
    }

}
