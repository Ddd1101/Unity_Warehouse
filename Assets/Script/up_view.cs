using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class up_view : MonoBehaviour
{

    public GameObject camera_1;
    public GameObject camera_2;
    public GameObject camera_3;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            camera_1.SetActive(true);
            camera_2.SetActive(false);
            camera_3.SetActive(false);
            //main_camera.transform.position = new Vector3(-9.46f, 42.05f, 24.97f);
            //main_camera.transform.rotation = Quaternion.Euler(10f, 0, 0);
        });
    }
}
