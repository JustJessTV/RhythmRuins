using UnityEngine;
using System.Collections;

public class BeatBehave : MonoBehaviour {

    public Vector3 spawnPoint;
    public Vector3 endPoint;
    public Vector3 camPoint;
    public float delay;
    public float fullNote;

    private Vector3 travelDir;
    private float delayFactor;
    private float startTime;
    private float lerpDX;

    ParticleSystem particles;
    Material mat;
    public bool testKill;
    enum States { 
        spawn,
        travel,
        end,
        fade
    }
    States state;
	// Use this for initialization
	void Start () {
        camPoint = RhythmRealm.GameRhythmManager.rhythmCamPos;
        Debug.Log(camPoint);
        particles = GetComponentInChildren<ParticleSystem>();
        travelDir = (endPoint - spawnPoint).normalized;
        startTime = Time.time;
        delayFactor = 1 / delay;
        state = States.travel;
        mat = GetComponent<MeshRenderer>().material;
        
	}
	
	// Update is called once per frame
	void Update () {
        if(state == States.travel){
            lerpDX = (Time.time - startTime) * delayFactor;
            this.transform.position = Vector3.Lerp(spawnPoint, endPoint, lerpDX);
            if (lerpDX > 1) {
                state = States.end;
            }
        }
        if (state == States.end) {
            KillMeSoftly();
            state = States.fade;            
        }
        if (state == States.fade){
        //    this.transform.position += travelDir * Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, camPoint, Time.deltaTime*0.2f);
            mat.color = Color.Lerp(mat.color, Color.clear, Time.deltaTime);
        }
        if (testKill) {
            BeatPulsate();
         //   KillMeSoftly();
            testKill = false;
            
        }
	}
    void BeatPulsate() {
        ParticleSystem.Burst[] burst = new ParticleSystem.Burst[]{
            new ParticleSystem.Burst(Time.time,5)};
        particles.emission.SetBursts(burst);
    }

    void KillMeSoftly() {
        float t = particles.startLifetime;
        ParticleSystem.EmissionModule em = particles.emission;
        ParticleSystem.MinMaxCurve minMax = new ParticleSystem.MinMaxCurve(0.0f);
        em.rate = minMax;
        Destroy(gameObject, t);
    }

}
