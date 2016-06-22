using UnityEngine;
using System.Collections;

public class Root : DispatchEvent {
    public static PlayerManager playerManger;
    public static Root main;
    public override void Awake()
    {
        main = this;
        new PlayerManager();
    }
    private AnimSets _animSets;
    public AnimSets animSets {
        get { return _animSets ?? (_animSets = new AnimSets()); }
    }

}
