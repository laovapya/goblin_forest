using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    void Start()
    {
        Camera myCamera = GetComponent<Camera>();
        myCamera.tag = "MainCamera";
    }
}
