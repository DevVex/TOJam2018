using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCartMove : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    private float speed;

    public float slow;
    public float fast;
    public float fastest;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
        speed = slow;
        audioSource = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidBody2D.velocity = transform.right * speed;


        audioSource.volume = rigidBody2D.velocity.x / fastest;
        audioSource.pitch = 0.9f + (rigidBody2D.velocity.x / fastest);
    }

    public void SlowSpeed()
    {
        speed = slow;
    }

    public void FastSpeed()
    {
        speed = fast;
    }

    public void FastestSpeed()
    {
        speed = fastest;
    }


}
