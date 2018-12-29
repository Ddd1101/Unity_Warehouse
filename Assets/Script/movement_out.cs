using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class movement_out : MonoBehaviour
{

    private NavMeshAgent agent;

    private Vector3 destination;
    private Vector3 tmp;
    private Vector3 origin = new Vector3();
    int tmp_x;
    int tmp_z;
    int is_stop = 0;
    Vector3 last_position = new Vector3();
    int itor = 0;
    bool store_falg = false;
    bool lock_ = true;
    int which_out = -1;
    int A_outing = 0;
    int B_outing = 0;
    int C_outing = 0;
    int out_type = 1;
    int movement_flag = -1;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        origin.x = transform.position.x;
        origin.z = transform.position.z;

        destination = new Vector3();
        destination.x = origin.x;
        destination.z = origin.z;

        last_position = origin;

        tmp.x = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("A_outing : " + A_outing);
        //Debug.Log("A_store  : " + Data.A_store.Count);
        if (movement_flag == -1 && (transform.position.z == origin.z) && (transform.position.x == origin.x))
        {
            if (out_type == 1)
            {
                if (A_outing != 0)
                {
                    if (Data.A_store.Count > 0)
                    {
                        //出一个A
                        movement_flag = 1;
                        tmp = Data.A_store.Dequeue();
                        Debug.Log("x = " + tmp.x + " y = " + tmp.y + " z = " + tmp.z);
                        destination = do_move(tmp);
                        agent.SetDestination(destination);
                        A_outing -= 1;
                        //此时这一批任务出完的话
                        if (A_outing == 0)
                        {
                            //转到出2
                            out_type = 2;
                        }
                    }
                    else
                    {
                        out_type = 2;
                    }
                }
                else
                {
                    if (Data.A_assignment.Count > 0)
                    {
                        A_outing = Data.A_assignment.Dequeue();
                    }
                }
                //destination.x = origin.x;
                //destination.z = origin.z;
            }
            if (out_type == 2)
            {
                if (B_outing != 0)
                {
                    if (Data.B_store.Count > 0)
                    {
                        //出一个A
                        movement_flag = 1;
                        tmp = Data.B_store.Dequeue();
                        Debug.Log("x = " + tmp.x + " y = " + tmp.y + " z = " + tmp.z);
                        destination = do_move(tmp);
                        agent.SetDestination(destination);
                        B_outing -= 1;
                        //此时这一批任务出完的话
                        if (B_outing == 0)
                        {
                            //转到出2
                            out_type = 3;
                        }
                    }
                    else
                    {
                        out_type = 3;
                    }
                }
                else
                {
                    if (Data.B_assignment.Count > 0)
                    {
                        B_outing = Data.B_assignment.Dequeue();
                    }
                }
                //destination.x = origin.x;
                //destination.z = origin.z;
            }
            if (out_type == 3)
            {
                if (A_outing != 0)
                {
                    if (Data.C_store.Count > 0)
                    {
                        //出一个A
                        movement_flag = 1;
                        tmp = Data.C_store.Dequeue();
                        Debug.Log("x = " + tmp.x + " y = " + tmp.y + " z = " + tmp.z);
                        destination = do_move(tmp);
                        agent.SetDestination(destination);
                        C_outing -= 1;
                        //此时这一批任务出完的话
                        if (C_outing == 0)
                        {
                            //转到出2
                            out_type = 1;
                        }
                    }
                    else
                    {
                        out_type = 1;
                    }
                }
                else
                {
                    if (Data.C_assignment.Count > 0)
                    {
                        C_outing = Data.C_assignment.Dequeue();
                    }
                }
                //destination.x = origin.x;
                //destination.z = origin.z;
            }
        }
        else if ((last_position.x != origin.x && last_position.z != origin.z) && (last_position.x == transform.position.x && last_position.z == transform.position.z))
        {
            is_stop += 1;
            movement_flag = -1;
            //Debug.Log("is_stop =" + is_stop);
            if (is_stop >= 7)
            {
                origin.y = transform.position.y;
                agent.SetDestination(origin);
                Debug.Log("reset :x = " + tmp.x + " y = " + tmp.y + " z = " + tmp.z);
                Data.position[(int)tmp.x, (int)tmp.y, (int)tmp.z] = 0;
                is_stop = 0;
            }
        }

        last_position.x = transform.position.x;
        last_position.z = transform.position.z;
    }

    public Vector3 do_move(Vector3 target_)
    {
        Vector3 goods = new Vector3();
        Vector3 rt = new Vector3();
        if (target_.y % 2 == 1)
        {
            //Debug.Log("single" + (++itor));
            target_.x -= 1;
            target_.y = (int)(tmp.y / 2);
            goods.z = (-3.649f + target_.y * 3.0f);
            goods.x = (-16.35f + ((int)((int)(target_.x) / 5)) * 10.4f + ((int)((int)(target_.x % 5))) * 1.677f);
            goods.y = (0.1639082f + ((int)(target_.z - 1)) * (0.6737639f - 0.1639082f));
            rt.z = goods.z;
            rt.z -= (float)(1.0);
            rt.x = goods.x;
            rt.y = transform.position.y;
            //Debug.Log("x = " + destination.x + " z = " + destination.z);
        }
        else
        {
            //Debug.Log("multi" + (++itor));
            target_.x -= 1;
            target_.y = (int)(target_.y / 2);
            goods.z = (-3.187f + target_.y * 3.0f);
            goods.x = (-16.35f + ((int)((int)target_.x) / 5) * 10.4f + ((int)((int)target_.x % 5)) * 1.677f);
            goods.y = (0.1639082f + ((int)(target_.z - 1)) * (0.6737639f - 0.1639082f));
            rt.z = goods.z;
            rt.z += (float)(1.0);
            rt.x = goods.x;
            rt.y = transform.position.y;
            //Debug.Log("x = " + destination.x + " z = " + destination.z);
        }
        return rt;
    }
}
