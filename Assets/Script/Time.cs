using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Time : MonoBehaviour {
    public TextMesh TxtCurrentTime;
    DateTime NowTime;
    DateTime start;

	// Use this for initialization
	void Start () {
        start = DateTime.Now.ToLocalTime();
	}
	
	// Update is called once per frame
	void Update () {
        NowTime = DateTime.Now.ToLocalTime();
        //TxtCurrentTime.text = NowTime.ToString("yyyy-MM-dd HH:mm:ss");
        TxtCurrentTime.text = Convert.ToString(NowTime - start);
	}
}
