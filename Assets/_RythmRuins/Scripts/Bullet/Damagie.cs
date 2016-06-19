using UnityEngine;
using System.Collections;

public class Damagie : MonoBehaviour{
    public float hp=1;
    public virtual void Hit(float amount) {
        hp -= amount;
        if (hp <= 0) Kill();
    }
    public virtual void Kill() {
        Destroy(gameObject);
    }
	
}
