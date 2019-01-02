using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
//using System.Diagnostics;

public class Time : MonoBehaviour
{
    public TextMesh TxtCurrentTime;
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

    public void goods_in(int num, int type)
    {
        //UnityEngine.Debug.Log("in thread" + num + " " + type);
        //UnityEngine.Debug.Log("goods_in : " + (itor++) + " position_put : " + Data.position_put.Count + " lock_ = " + Time.lock_);
        ga = new GeneticAlgorithm(type, num);
        Debug.Log("num = = = =  " + num);
        ga.Initialize_population();
        //UnityEngine.Debug.Log("GA :" + (itor++));
        //Debug.Log("1");
        for (int i = 0; i < 100; i++)
        {
            ga.Fitness();
            //Debug.Log("5");
            ga.cal_fmax_favg();
            //UnityEngine.Debug.Log("6");
            ga.Selection();
            //UnityEngine.Debug.Log("7");
            ga.Pick_parents();
            //UnityEngine.Debug.Log("8");
            int times = num + 1;
            for (int j = 0; j <= times; j++)
            {
                ga.Offspring();
                //UnityEngine.Debug.Log("loop-----------");
            }
            //Debug.Log("9");
            //UnityEngine.Debug.Log("is alive " + Thread.CurrentThread.IsAlive);
            ga.Exchange();
            Debug.Log("10");
        }
        //Debug.Log("2");
        ga.Fitness();
        //Debug.Log("3");
        ga.cal_fmax_favg();
        //Debug.Log("4");
        ga.Out_put();
        //UnityEngine.Debug.Log(itor + "after ga" + Data.position_put.Count);
        //UnityEngine.Debug.Log("===================================");
        if (type == 1)
        {
            //UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!_1");
            lock__ = 1;
        }
        else if (type == 2)
        {
            //UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!_2");
            lock__ = 2;
        }
        else if (type == 3)
        {
            lock_ = true;
            lock__ = 0;
            //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //goods_in_thread.Abort();
            //in_thread = false;
            //StopCoroutine(do_goods_in());
        }
    }

    public void goods_out(int num, int type)
    {
        //UnityEngine.Debug.Log("num -- " + num + " type -- " + type);
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

    // Use this for initialization
    void Start()
    {
        start = DateTime.Now.ToLocalTime();
    }

    // Update is called once per frame
    void Update()
    {
        //UnityEngine.Debug.Log("is alive " + Thread.CurrentThread.IsAlive);
        NowTime = DateTime.Now.ToLocalTime();
        //TxtCurrentTime.text = NowTime.ToString("yyyy-MM-dd HH:mm:ss");
        TxtCurrentTime.text = Convert.ToString(NowTime - start);
        //print(Convert.ToDouble(Convert.ToString(NowTime - start)));
        duration = (int)((NowTime - start).TotalSeconds);
        //UnityEngine.Debug.Log("duration ========================== " + duration);
        if (duration % 600 == 0 && duration / 600 == itor_)
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

        //Debug.Log("in _thread " + in_thread);
        if (lock__ == 0)
        {
            Debug.Log("==============================1");
            lock__ = -1;
            goods_in(Data.A_in[(goods_in_) % 12], 1);
        }
        if (lock__ == 1)
        {
            Debug.Log("==============================2");
            lock__ = -1;
            goods_in(Data.B_in[(goods_in_) % 12], 2);
        }
        if (lock__ == 2)
        {
            Debug.Log("==============================3");
            lock__ = -1;
            goods_in(Data.C_in[(goods_in_++) % 12], 3);
        }
        //yield return new WaitForSeconds(2.0f);
    }

    void do_goods_in_2()
    {
        while (in_thread)
        {
            //goods_in_period_ = 0;
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
        //goods_out_thread.Abort();
        //yield return new WaitForSeconds(2.0f);
    }

    void OnApplicationQuit()
    {
        /*if (goods_in_thread.ThreadState == System.Threading.ThreadState.Running)
        {
            goods_in_thread.Abort();
        }

        if (goods_out_thread.ThreadState == System.Threading.ThreadState.Running)
        {
            goods_out_thread.Abort();
        }*/

        Debug.Log("==============================out");
        //PlayerPrefs.Save();
    }
}
