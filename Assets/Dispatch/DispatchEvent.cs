using UnityEngine;
using System.Collections;

public abstract class DispatchEvent {
    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void OnGUI() { }

}
