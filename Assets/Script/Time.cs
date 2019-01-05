using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.UI;

public class Time : MonoBehaviour
{
    DateTime NowTime;
    DateTime start;
    int duration;
    GeneticAlgorithm ga;
    ThreadStart child_thread;
    Thread goods_in_thread = null;
    Thread goods_in_thread_1 = null;
    Thread goods_in_thread_2 = null;
    Thread goods_in_thread_3 = null;
    Thread goods_out_thread = null;
    public bool lock_ = true;
    public int lock__ = 0;
    public static int itor = 0;
    public static UnityEngine.Object o = new UnityEngine.Object();
    int goods_in_ = 0;
    int goods_in_period_ = 0;
    int goods_out_ = 0;
    bool in_thread = true;
    int itor_ = 0;

    public GameObject type_1;
    public GameObject type_2;
    public GameObject type_3;
    private Vector3 goods;

    public void goods_in(int num, int type)
    {
        ga = new GeneticAlgorithm(type, num);
        ga.Initialize_population();
        for (int i = 0; i < 200; i++)
        {
            ga.Fitness();
            ga.cal_fmax_favg();
            ga.Selection();
            ga.Pick_parents();
            int times = num + 1;
            for (int j = 0; j <= times; j++)
            {
                ga.Offspring();
            }
            ga.Exchange();
        }
        ga.Fitness();
        ga.cal_fmax_favg();
        ga.Out_put();
        if (type == 1)
        {
            lock__ = 1;
        }
        else if (type == 2)
        {
            lock__ = 2;
        }
        else if (type == 3)
        {
            lock_ = true;
            lock__ = 0;
        }
    }

    public void goods_out(int num, int type)
    {
        if (type == 1)
        {
            Data.A_assignment.Enqueue(num);
        }
        if (type == 2)
        {
            Data.B_assignment.Enqueue(num);
        }
        if (type == 3)
        {
            Data.C_assignment.Enqueue(num);
        }
    }

    void Start()
    {
        start = DateTime.Now.ToLocalTime();
    }

    void Update()
    {
        NowTime = DateTime.Now.ToLocalTime();
        this.GetComponent<Text>().text = Convert.ToString(NowTime - start);
        duration = (int)((NowTime - start).TotalSeconds);
        //UnityEngine.Debug.Log("duration ========================== " + duration);
        if (duration % 1200 == 0 && duration / 1200 == itor_)
        {
            itor_++;
            if (lock_ == true)
            {
                lock__ = 0;
                lock_ = false;
                //in_thread = true;
                goods_in_thread = new Thread(do_goods_in);
                Thread.Sleep(1000);
                goods_in_thread.Start();
                goods_out_thread = new Thread(do_goods_out);
                goods_out_thread.Start();
                //StartCoroutine(do_goods_in());
                //StartCoroutine(do_goods_out());
            }
        }
        if (itor == 12)
        {
            Debug.Log("final_result = " + Data.final_result);
        }

        if (Data.position_put.Count > 0)
        {
            int tmp_count = Data.position_put.Count;
            Vector4 tmp_show;
            for (int k = 0; k < tmp_count; k++)
            {
                tmp_show = Data.position_put.Dequeue();
                goods = new Vector3();
                if (tmp_show.y % 2 == 1)
                {
                    //Debug.Log("single" + (++itor));
                    tmp_show.x -= 1;
                    tmp_show.y = (int)(tmp_show.y / 2);
                    goods.z = (-3.3187f + tmp_show.y * 3.0f);
                    goods.x = (-16.35f + ((int)((int)(tmp_show.x) / 5)) * 10.4f + ((int)((int)(tmp_show.x % 5))) * 1.677f);
                    goods.y = (0.1639082f + ((int)(tmp_show.z - 1)) * (0.6737639f - 0.1639082f));
                    //Debug.Log("x = " + destination.x + " z = " + destination.z);
                }
                else
                {
                    //Debug.Log("multi" + (++itor));
                    tmp_show.x -= 1;
                    tmp_show.y = (int)(tmp_show.y / 2);
                    //Debug.Log(tmp.y);
                    goods.z = (-2.69f + (tmp_show.y - 1) * 3.0f);
                    goods.x = (-16.35f + ((int)((int)tmp_show.x) / 5) * 10.4f + ((int)((int)tmp_show.x % 5)) * 1.677f);
                    goods.y = (0.1639082f + ((int)(tmp_show.z - 1)) * (0.6737639f - 0.1639082f));
                    //Debug.Log("x = " + destination.x + " z = " + destination.z);
                }


                GameObject store_object = null;
                if (tmp_show.w == 1)
                {
                    store_object = GameObject.Instantiate(type_1, goods, Quaternion.identity);
                    Data.A_store.Add(new Vector3(tmp_show.x, tmp_show.y, tmp_show.z));
                }
                if (tmp_show.w == 2)
                {
                    store_object = GameObject.Instantiate(type_2, goods, Quaternion.identity);
                    Data.B_store.Add(new Vector3(tmp_show.x, tmp_show.y, tmp_show.z));
                }
                if (tmp_show.w == 3)
                {
                    store_object = GameObject.Instantiate(type_3, goods, Quaternion.identity);
                    Data.C_store.Add(new Vector3(tmp_show.x, tmp_show.y, tmp_show.z));
                }

                Data.A_object_store.Add(store_object);
            }
        }
    }



    void do_ga()
    {
        Thread.Sleep(3000);
        UnityEngine.Debug.Log("in thread");
        lock_ = true;
    }

    //IEnumerator do_goods_in()
    void do_goods_in()
    {
        if (lock__ == 0)
        {
            lock__ = -1;
            goods_in(Data.A_in[(goods_in_) % 12], 1);
        }
        if (lock__ == 1)
        {
            lock__ = -1;
            goods_in(Data.B_in[(goods_in_) % 12], 2);
        }
        if (lock__ == 2)
        {
            lock__ = -1;
            goods_in(Data.C_in[(goods_in_++) % 12], 3);
        }
    }

    void do_goods_in_2()
    {
        while (in_thread)
        {
            if (lock__ == 0)
            {
                UnityEngine.Debug.Log("==============================1");
                lock__ = -1;
                goods_in_thread_1 = new Thread(new ThreadStart(() => goods_in(Data.A_in[(goods_in_) % 12], 1)));
                goods_in_thread_1.Start();
            }
            if (lock__ == 1)
            {
                goods_in_thread_1.Abort();
                UnityEngine.Debug.Log("==============================2");
                lock__ = -1;
                goods_in_thread_2 = new Thread(new ThreadStart(() => goods_in(Data.B_in[(goods_in_) % 12], 2)));
                goods_in_thread_2.Start();
            }
            if (lock__ == 2)
            {
                UnityEngine.Debug.Log("==============================3");
                goods_in_thread_2.Abort();
                lock__ = -1;
                goods_in_thread_3 = new Thread(new ThreadStart(() => goods_in(Data.C_in[(goods_in_++) % 12], 3)));
                goods_in_thread_3.Start();
            }
        }
    }


    //IEnumerator do_goods_out()
    void do_goods_out()
    {
        goods_out(Data.A_out[(goods_out_) % 12], 1);
        goods_out(Data.B_out[(goods_out_) % 12], 2);
        goods_out(Data.C_out[(goods_out_++) % 12], 3);
    }

    void OnApplicationQuit()
    {
        if (goods_in_thread.ThreadState == System.Threading.ThreadState.Running)
        {
            goods_in_thread.Abort();
        }

        if (goods_out_thread.ThreadState == System.Threading.ThreadState.Running)
        {
            goods_out_thread.Abort();
        }

        Debug.Log("==============================out");
    }
}
