using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PIDBehaviour : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private PIDController _controller;

    [SerializeField]
    private Transform _followTarget;

    public float multiplier;

    [Header("PID Settings")]
    public PIDSettings settings;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _controller = new PIDController(ref settings);
    }

    public void SetTarget(Transform target)
    {
        _followTarget = target;
    }

    private void FixedUpdate()
    {
        Vector3 input = _controller.Update(Time.fixedDeltaTime, transform.position, _followTarget.position);

        _rigidbody.AddForce(input * multiplier, ForceMode.Force);
    }
}
