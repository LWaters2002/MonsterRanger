using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectile : MonoBehaviour
{
    public PIDSettings PID_Settings;
    private Rigidbody _rigidbody;
    private PIDController _PIDController;

    private Vector3 _target;

    private ProjectileState _state = ProjectileState.Tracking;

    [SerializeField]
    private float damage;

    public void Init(float damage, Vector3 target)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _PIDController = new PIDController(ref PID_Settings);

        _target = target;
    }

    public void SetTarget(Vector3 target, float SetMoveMultiplier)
    {
        _state = ProjectileState.Tracking;
        _target = target;
    }

    public void SetState(ProjectileState state)
    {
        _state = state;
    }

    public void LaunchAtTarget(Vector3 target, float strength)
    {
        Vector3 direction = (target - transform.position).normalized;
        _rigidbody.AddForce(direction * strength, ForceMode.Impulse);
    }

    public void LaunchAtRigidbody(GameObject targetObject, float predictionInSeconds, float strength)
    {
        Rigidbody targetRigidbody = targetObject.GetComponentInParent<Rigidbody>();
        if (!targetRigidbody) { LaunchAtTarget(targetObject.transform.position, strength); return; }

        Vector3 target = targetObject.transform.position + targetRigidbody.velocity * predictionInSeconds;

        LaunchAtTarget(target, strength);
    }

    private void FixedUpdate()
    {
        if (_state != ProjectileState.Tracking) return;

        Vector3 force = _PIDController.Update(Time.fixedDeltaTime, transform.position, _target);
        _rigidbody.AddForce(force, ForceMode.Acceleration);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter pc = other.GetComponentInParent<PlayerCharacter>();
        if (!pc) return;

        pc.stats.AlterHealth(-damage);
    } 
}

public enum ProjectileState
{
    Tracking,
    Physics
}