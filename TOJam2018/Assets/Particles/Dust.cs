using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour {

    public float lowSpeed;
    public float dampen;

    private ParticleSystem ps;
    private GameObject target;

    private Rigidbody2D rigidBody2d;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;

    // Use this for initialization
    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
        emissionModule = ps.emission;
        mainModule = ps.main;
        target = GameObject.FindGameObjectWithTag("PlayerCart");
        if (target != null)
        {
            rigidBody2d = target.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rigidBody2d.velocity.x < lowSpeed)
        {
            ps.Stop();
        }
        else
        {
            if (ps.isStopped)
            {
                ps.Play();
            }
            emissionModule.rateOverTime = rigidBody2d.velocity.x / dampen;
            mainModule.startSizeMultiplier = rigidBody2d.velocity.x / 8.0f / dampen;
        }
    }
}
