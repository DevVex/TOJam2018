using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TOJAM;

public class BodyPart : MonoBehaviour {

    public GameObject effect;
    private GameObject currentEffect; 

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Constants.TAG_GROUND))
        {
            SetEffect(LayerMask.LayerToName(other.gameObject.layer), effect);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Constants.TAG_GROUND))
        {
            Destroy(currentEffect, 0.8f);
        }
    }

    void SetEffect(string layer, GameObject effect)
    {
        GameObject newEffect = Instantiate(effect);
        newEffect.GetComponent<FollowTarget>().SetTarget(this.gameObject);
        currentEffect = newEffect;
    }
}
