using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class movement : MonoBehaviour {

    public Transform TargetObject = null;

    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();

        Vector3 dest = new Vector3(-14.07f, this.transform.position.y, 14.02f);

        agent.SetDestination(dest);
	}
	
	// Update is called once per frame
	void Update () {
       
	}  
}
