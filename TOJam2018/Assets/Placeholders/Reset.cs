using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

    public GameObject startingPlace;

    private bool hit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hit)
        {
            other.gameObject.transform.position = startingPlace.transform.position;
        }
        else
        {
            GameObject.FindGameObjectWithTag("PlayerPerson").GetComponent<CharacterLaunch>().Launch();
        }
    }

    public void Hit()
    {
        hit = true;
    }
}
