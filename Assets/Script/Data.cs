using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Data : MonoBehaviour
{
    //public static int[] A_in = { 112, 105, 126, 123, 110, 65, 99, 95, 99, 98, 87, 86 };
    public static int[] A_in = { 12, 105, 126, 123, 110, 65, 99, 95, 99, 98, 87, 86 };
    //public static int[] A_out = { 116, 110, 122, 120, 116, 68, 103, 93, 94, 96, 81, 90 };
    public static int[] A_out = { 12, 110, 122, 120, 116, 68, 103, 93, 94, 96, 81, 90 };
    public static int[] B_in = { 16, 18, 22, 20, 18, 12, 17, 14, 13, 13, 12, 11 };
    public static int[] B_out = { 17, 19, 22, 23, 18, 12, 18, 14, 14, 13, 11, 12 };
    //public static int[] C_in = { 96, 95, 113, 113, 103, 64, 100, 87, 79, 79, 70, 70 };
    public static int[] C_in = { 6, 95, 113, 113, 103, 64, 100, 87, 79, 79, 70, 70 };
    //public static int[] C_out = { 97, 102, 111, 117, 108, 65, 92, 81, 76, 74, 63, 74 };
    public static int[] C_out = { 6, 102, 111, 117, 108, 65, 92, 81, 76, 74, 63, 74 };

    public static Queue<Vector4> position_put = new Queue<Vector4>();
    public static int[,,] position = new int[16, 13, 4];
    public static int[,,] position_putted = new int[16, 13, 4];

    public static Queue<Vector3> A_store = new Queue<Vector3>();
    public static Queue<Vector3> B_store = new Queue<Vector3>();
    public static Queue<Vector3> C_store = new Queue<Vector3>();

    public static Queue<int> A_assignment = new Queue<int>();
    public static Queue<int> B_assignment = new Queue<int>();
    public static Queue<int> C_assignment = new Queue<int>();

    public static DateTime now_time;

    public Data()
    {
        now_time = DateTime.Now.ToLocalTime();
        for (int i = 1; i < 16; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                for (int h = 1; h < 4; h++)
                {
                    position[i, j, h] = 0;
                }
            }
        }
        for (int i = 1; i < 16; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                for (int k = 1; k < 4; k++)
                {
                    position_putted[i, j, k] = 0;
                }
            }
        }
    }

    private void Update()
    {
        now_time = DateTime.Now.ToLocalTime();
    }
}