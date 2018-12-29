﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class movement_in : MonoBehaviour
{
    private NavMeshAgent agent;

    private Vector3 destination;
    private Vector4 tmp;
    private Vector3 goods;
    private Vector3 origin = new Vector3();
    private Vector4 target = new Vector4();
    int tmp_x;
    int tmp_z;
    int is_stop = 0;
    Vector3 last_position = new Vector3();
    int itor = 0;
    bool store_flag = false;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        destination = new Vector3();

        origin.x = transform.position.x;
        origin.z = transform.position.z;

        last_position = origin;

        tmp.x = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.z + " " + origin.z);
        //Debug.Log("movement " + Data.position_put.Count);
        //Debug.Log(agent.isStopped);
        if ((tmp.x == -1) && (transform.position.z == origin.z) && (transform.position.x == origin.x) && (Data.position_put.Count > 0))
        {
            //Debug.Log("in");
            target = Data.position_put.Dequeue();
            tmp = target;
            store_flag = true;
            //Debug.Log("position : x = " + tmp.x + " y = " + tmp.y + " z = " + tmp.z + " / " + Data.position_put.Count);
            goods = new Vector3();
            if (tmp.y % 2 == 1)
            {
                //Debug.Log("single" + (++itor));
                tmp.x -= 1;
                tmp.y = (int)(tmp.y / 2);
                goods.z = (-3.649f + tmp.y * 3.0f);
                goods.x = (-16.35f + ((int)((int)(tmp.x) / 5)) * 10.4f + ((int)((int)(tmp.x % 5))) * 1.677f);
                goods.y = (0.1639082f + ((int)(tmp.z - 1)) * (0.6737639f - 0.1639082f));
                destination.z = goods.z;
                destination.z -= (float)(1.5);
                destination.x = goods.x;
                destination.y = transform.position.y;
                //Debug.Log("x = " + destination.x + " z = " + destination.z);
            }
            else
            {
                //Debug.Log("multi" + (++itor));
                tmp.x -= 1;
                tmp.y = (int)(tmp.y / 2);
                goods.z = (-3.187f + tmp.y * 3.0f);
                goods.x = (-16.35f + ((int)((int)tmp.x) / 5) * 10.4f + ((int)((int)tmp.x % 5)) * 1.677f);
                goods.y = (0.1639082f + ((int)(tmp.z - 1)) * (0.6737639f - 0.1639082f));
                destination.z = goods.z;
                destination.z += (float)(1.5);
                destination.x = goods.x;
                destination.y = transform.position.y;
                //Debug.Log("x = " + destination.x + " z = " + destination.z);
            }
            agent.SetDestination(destination);
        }
        else if ((last_position.x != origin.x && last_position.z != origin.z) && (last_position.x == transform.position.x && last_position.z == transform.position.z))
        {
            is_stop += 1;
            tmp.x = -1;
            //Debug.Log("is_stop =" + is_stop);
            if (is_stop == 6)
            {
                Debug.Log("position : x = " + target.x + " y = " + target.y + " z = " + target.z + " / " + Data.position_put.Count);
                if (target.w == 1)
                {
                    Data.A_store.Enqueue(new Vector3(target.x, target.y, target.z));
                }
                else if (target.w == 2)
                {
                    Data.B_store.Enqueue(new Vector3(target.x, target.y, target.z));
                }
                else
                {
                    Data.C_store.Enqueue(new Vector3(target.x, target.y, target.z));
                }
            }

            if (is_stop >= 12)
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
