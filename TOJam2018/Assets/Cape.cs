using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TOJAM;

public class Cape : MonoBehaviour {

    private Cloth cape;
    public GameObject attachTo;
    private GameObject target;
    private Rigidbody2D rigidBody2d;
    public Vector3 offset;

    public float minimumSpeed;

    // Use this for initialization
    void Start () {
        cape = this.GetComponent<Cloth>();
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.position = new Vector3(attachTo.transform.position.x + offset.x, attachTo.transform.position.y + offset.y, 5.0f);

        //cape.externalAcceleration = new Vector3(-1 * (rigidBody2d.velocity.x + speed), 0, 0);

    }

    private void FixedUpdate()
    {
        float speed = minimumSpeed;
        if (PlayerManager.Instance.Player.HasLaunched)
        {
            speed = 0;
        }

        target = PlayerManager.Instance.FollowTarget.gameObject;
        rigidBody2d = target.GetComponent<Rigidbody2D>();
        cape.externalAcceleration = new Vector3(-1 * (rigidBody2d.velocity.x + speed), -1 * rigidBody2d.velocity.y, 0);
    }
}
