using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Time : MonoBehaviour
{
    private static Time instance;

    public TextMesh TxtCurrentTime;
    DateTime NowTime;
    DateTime start;
    int duration;
    GeneticAlgorithm ga;
    ThreadStart child_thread;
    Thread goods_in_thread;
    bool lock_ = false;
    int itor = 0;

    public void goods_in(int num, int type)
    {
        //Debug.Log("in thread" + num + " " + type);
        Debug.Log(itor++);
        ga = new GeneticAlgorithm(type, num);
        ga.Initialize_population();
        //Debug.Log("1");
        for (int i = 0; i < 10; i++)
        {
            //Debug.Log(i);
            ga.Fitness();
            //Debug.Log("5");
            ga.cal_fmax_favg();
            //Debug.Log("6");
            ga.Selection();
            //Debug.Log("7");
            ga.Pick_parents();
            //Debug.Log("8");
            for (int j = 0; j < 50; j++)
            {
                ga.Offspring();
            }
            ga.Exchange();
        }
        //Debug.Log("2");
        ga.Fitness();
        //Debug.Log("3");
        ga.cal_fmax_favg();
        //Debug.Log("4");
        ga.Out_put();
        //Debug.Log("after ga" + Data.position_put.Count);
        Thread.CurrentThread.Abort();
        lock_ = false;
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
        Console.WriteLine();
        //print(Convert.ToDouble(Convert.ToString(NowTime - start)));
        duration = (int)((NowTime - start).TotalSeconds);
        Debug.Log(lock_);
        if (duration % 180 == 0 && lock_ == false)
        {
            lock_ = true;
            goods_in_thread = new Thread(new ThreadStart(() => goods_in(20, 1)));
            goods_in_thread.Start();
        }
    }
}
