using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHP : MonoBehaviour
{
    public Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 항상 카메라 쳐다보도록
        transform.LookAt(_camera.transform);
    }
}
