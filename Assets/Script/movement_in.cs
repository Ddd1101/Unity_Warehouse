using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class movement_in : MonoBehaviour
{

    public Transform TargetObject = null;

    private NavMeshAgent agent;

    private Vector3 destination;
    private Vector4 tmp;
    private Vector3 goods;
    private Vector3 origin = new Vector3();
    int tmp_x;
    int tmp_z;
    int is_stop = 0;
    Vector3 last_position = new Vector3();

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        destination = new Vector3();

        //agent.SetDestination(dest);
        origin.x = transform.position.x;
        origin.z = transform.position.z;

        last_position = origin;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.z + " " + origin.z);
        Debug.Log("movement " + Data.position_put.Count);
        if ((transform.position.z == origin.z) && (transform.position.x == origin.x) && (Data.position_put.Count > 0))
        {
            //Debug.Log("in");
            tmp = Data.position_put.Dequeue();
            //Debug.Log("position : x = " + tmp.x + " y = " + tmp.y + " z = " + tmp.z);
            goods = new Vector3();
            if (tmp.y % 2 == 1)
            {
                tmp.y /= 2;
                goods.z = (float)(-3.187 + tmp.y * 3.0);
                goods.x = (float)(-16.36 + (tmp.x / 5) * 10.4 + ((tmp.x % 5) - 1) * 1.677);
                goods.y = (float)(0.1639082 + (tmp.z - 1) * (0.6737639 - 0.1639082));
                destination.z = goods.z;
                destination.x = goods.x;
                destination.y = transform.position.y;
            }else{
                tmp.y /= 2;
                goods.z = (float)(-3.187 + tmp.x * 3.0);
                goods.x = (float)(-16.36 + (tmp.y / 5) * 10.4 + ((tmp.y % 5) - 1) * 1.677);
                goods.y = (float)(0.1639082 + (tmp.z - 1) * (0.6737639 - 0.1639082));
                destination.z = goods.z;
                destination.x = goods.x;
                destination.y = transform.position.y;
            }
            agent.SetDestination(destination);
        }
        else if (last_position.x == transform.position.x && last_position.z == transform.position.z)
        {
            is_stop += 1;
            //Debug.Log("is_stop =" + is_stop);
            if (is_stop >= 5)
            {
                origin.y = transform.position.y;
                agent.SetDestination(origin);
                is_stop = 0;
            }
        }

        last_position.x = transform.position.x;
        last_position.z = transform.position.z;
    }
}
