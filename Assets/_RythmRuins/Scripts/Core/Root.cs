using UnityEngine;
using System.Collections;

public class Root : DispatchEvent {
    public static PlayerManager playerManger;
    public static Root main;
    private AnimSets _animSets;
    public AnimSets animSets {
        get { return _animSets ?? (_animSets = new AnimSets()); }
    }
    public override void Awake() {
        main = this;
        new PlayerManager();
    }
    public override void Update() {
    }

}
