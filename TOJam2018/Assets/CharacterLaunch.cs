using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaunch : MonoBehaviour {

    public Rigidbody2D body;
    public float force;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("space");

            Launch();
        }
    }

    void Launch()
    {
        Vector3 dir = new Vector3(150f, 50f, 0f);
        dir.Normalize();
        body.AddForce(dir * force);
    }
}
