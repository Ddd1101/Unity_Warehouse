using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class store : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Text>().text = "Store:  A- " + Data.A_store.Count + " B- " + Data.B_store.Count + " C- " + Data.C_store.Count;
	}
}
