using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationAxes
{
    MouseXAndY = 0, MouseX = 1, MouseY = 2
}

public class Camera : MonoBehaviour
{
    /// <summary>
    /// 跟随目标
    /// </summary>
    public Transform followTarget;
    /// <summary>
    /// 方向向量
    /// </summary>
    private Vector3 dir;

    //x轴（水平）速度
    public float sensitivityX = 15F;
    //y轴（垂直）速度
    public float sensitivityY = 15F;
    //x轴（水平）最小旋转值
    public float minimumX = -360F;
    //x轴（水平）最大旋转值
    public float maximumX = 360F;
    //y轴（垂直）最小旋转值
    public float minimumY = -60F;
    //y轴（垂直）最大旋转值
    public float maximumY = 60F;
    //旋转轴
    public RotationAxes axes = RotationAxes.MouseXAndY;
    private float rotationY = 0F;

    private void Start()
    {
        //获取方向向量
        dir = transform.position - followTarget.position;

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    private void Update()
    {
        transform.position = dir + followTarget.position;
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }
}
