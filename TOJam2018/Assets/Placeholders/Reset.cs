using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

    public GameObject startingPlace;

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.transform.position = startingPlace.transform.position;
    }
}
