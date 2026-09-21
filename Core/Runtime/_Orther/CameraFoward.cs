using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFoward : MonoBehaviour
{
    public Camera main;

    private void Start()
    {
        main = Camera.main;
    }

    public void Update()
    {
        if (main != null) transform.forward = main.transform.forward;
        else
        {
            main = Camera.main;
        }
    }
}