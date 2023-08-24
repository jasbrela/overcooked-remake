using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        Quaternion camRot = _cam.transform.rotation;
        
        transform.LookAt(transform.position + camRot * Vector3.forward, camRot * Vector3.up);
    }
}
