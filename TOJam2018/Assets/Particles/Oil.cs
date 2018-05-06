using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour {

    public float lowSpeed;
    public float dampen;

    private ParticleSystem ps;
    private GameObject target;

    private Rigidbody2D rigidBody2d;
    private bool turnedOff;

	// Use this for initialization
	void Start () {
        ps = this.GetComponent<ParticleSystem>();
        target = GameObject.FindGameObjectWithTag("PlayerCart");
        if(target != null)
        {
            rigidBody2d = target.GetComponent<Rigidbody2D>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (rigidBody2d.velocity.x < lowSpeed)
        {
            ps.Stop();
            turnedOff = true;
        }
        else
        {
            if (ps.isStopped && turnedOff)
            {
                ps.Play();
                turnedOff = false;
            }
            ps.startSpeed = rigidBody2d.velocity.x / dampen;
        }
    }
}
