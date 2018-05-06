using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public ParticleSystem wind;
    public FollowTarget cameraTarget;

    private Rigidbody2D targetRigidBody;
    private ParticleSystem.EmissionModule emissionModule;

    // Use this for initialization
    void Start () {
        emissionModule = wind.emission;
    }
	
	// Update is called once per frame
	void Update () {
        targetRigidBody = cameraTarget.target.GetComponent<Rigidbody2D>();
        if (targetRigidBody.velocity.x < 6)
        {
            wind.gameObject.SetActive(false);
        }
        else
        {
            if (!wind.gameObject.activeSelf)
            {
                wind.gameObject.SetActive(true);
            }
            wind.startSpeed = targetRigidBody.velocity.x;
            emissionModule.rateOverTime = targetRigidBody.velocity.x;
        }

    }
}
