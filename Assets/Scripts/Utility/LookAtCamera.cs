using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _camTransform;

    Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
        _camTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = _camTransform.rotation * originalRotation;
    }
}
