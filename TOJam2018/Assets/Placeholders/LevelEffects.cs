using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEffects : MonoBehaviour {

    public GameObject oil;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ToggleOil()
    {
        oil.SetActive(!oil.activeSelf);
    }
}
