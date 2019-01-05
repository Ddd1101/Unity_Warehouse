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
        });
    }
}
