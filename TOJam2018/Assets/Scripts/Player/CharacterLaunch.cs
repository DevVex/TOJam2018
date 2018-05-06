using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaunch : MonoBehaviour {

    Rigidbody2D[] bodyParts;
    public FollowTarget cameraTarget;

    public Rigidbody2D body;

    // Use this for initialization
    void Start () {
        bodyParts = this.transform.GetComponentsInChildren<Rigidbody2D>();


        foreach (Rigidbody2D rigidBody in bodyParts)
        {
            rigidBody.isKinematic = true;
        }
    }

    public void Launch()
    {

        //cameraTarget.SetTarget(body.gameObject);
        foreach (Rigidbody2D rigidBody in bodyParts)
        {
            rigidBody.isKinematic = false;
        }

        body.AddForce(new Vector2(50.0f, 20.0f) * 1000.0f);


    }
	
	// Update is called once per frame
	void Update () {

    }
}
