using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Time : MonoBehaviour
{
    public TextMesh TxtCurrentTime;
    DateTime NowTime;
    DateTime start;
    int duration;
    int millsecond;
    GeneticAlgorithm ga;
    ThreadStart child_thread;
    Thread goods_in_thread = null;
    Thread goods_out_thread = null;
    public bool lock_ = true;
    public int lock__ = 0;
    public static int itor = 0;
    public static UnityEngine.Object o = new UnityEngine.Object();
    int goods_in_ = 0;
    int goods_in_period_ = 0;
    int goods_out_ = 0;

    public void goods_in(int num, int type)
    {
        //Debug.Log("in thread" + num + " " + type);
        //Debug.Log("goods_in : " + (itor++) + " position_put : " + Data.position_put.Count + " lock_ = " + Time.lock_);
        ga = new GeneticAlgorithm(type, num);
        ga.Initialize_population();
        //Debug.Log("GA :" + (itor++));
        for (int i = 0; i < 5; i++)
        {
            ga.Fitness();
            //Debug.Log(itor + "5");
            ga.cal_fmax_favg();
            //Debug.Log(itor + "6");
            ga.Selection();
            //Debug.Log(itor + "7");
            ga.Pick_parents();
            //Debug.Log(itor + "8");
            for (int j = 0; j <= num; j++)
            {
                ga.Offspring();
            }
            //Debug.Log(itor + "9");
            ga.Exchange();
            //Debug.Log(itor + "10");
        }
        //Debug.Log("2");
        ga.Fitness();
        //Debug.Log("3");
        ga.cal_fmax_favg();
        //Debug.Log("4");
        ga.Out_put();
        //Debug.Log(itor + "after ga" + Data.position_put.Count);
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
            Thread.CurrentThread.Abort();
        }
        //Thread.CurrentThread.Abort();
    }

    public void goods_out(int num, int type)
    {
        //Debug.Log("num -- " + num + " type -- " + type);
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
        NowTime = DateTime.Now.ToLocalTime();
        //TxtCurrentTime.text = NowTime.ToString("yyyy-MM-dd HH:mm:ss");
        TxtCurrentTime.text = Convert.ToString(NowTime - start);
        //print(Convert.ToDouble(Convert.ToString(NowTime - start)));
        duration = (int)((NowTime - start).TotalSeconds);
        millsecond = (int)((NowTime - start).TotalMilliseconds);
        //Debug.Log("duration ========================== " + duration);
        if (duration % 9999999 == 0)
        {
            if (lock_ == true)
            {
                lock__ = 0;
                lock_ = false;
                goods_in_thread = new Thread(do_goods_in);
                goods_in_thread.Start();
                goods_out_thread = new Thread(do_goods_out);
                goods_out_thread.Start();
            }


        }

    }



    void do_ga()
    {
        Thread.Sleep(3000);
        Debug.Log("in thread");
        lock_ = true;
    }

    void do_goods_in()
    {
        if (lock__ == 0)
        {
            //Debug.Log("==============================1");
            lock__ = -1;
            goods_in(Data.A_in[(goods_in_) % 12], 1);
        }
        if (lock__ == 1)
        {
            //Debug.Log("==============================2");
            lock__ = -1;
            goods_in(Data.B_in[(goods_in_) % 12], 2);
        }
        if (lock__ == 2)
        {
            //Debug.Log("==============================3");
            lock__ = -1;
            goods_in(Data.C_in[(goods_in_++) % 12], 3);
        }
    }

    void do_goods_in_2()
    {
        goods_in_period_ = 0;
        while (lock_ == false)
        {
            if (lock__ == 0)
            {
                Debug.Log("==============================1");
                lock__ = -1;
                goods_in_thread = new Thread(new ThreadStart(() => goods_in(Data.A_in[(goods_in_) % 12], 1)));
                goods_in_thread.Start();
            }
            if (lock__ == 1)
            {
                Debug.Log("==============================2");
                lock__ = -1;
                goods_in_thread = new Thread(new ThreadStart(() => goods_in(Data.B_in[(goods_in_) % 12], 2)));
                goods_in_thread.Start();
            }
            if (lock__ == 2)
            {
                Debug.Log("==============================3");
                lock__ = -1;
                goods_in_thread = new Thread(new ThreadStart(() => goods_in(Data.C_in[(goods_in_++) % 12], 3)));
                goods_in_thread.Start();
            }
        }
    }


    void do_goods_out()
    {
        goods_out(Data.A_out[(goods_out_) % 12], 1);
        goods_out(Data.B_out[(goods_out_) % 12], 2);
        goods_out(Data.C_out[(goods_out_++) % 12], 3);
    }
}
