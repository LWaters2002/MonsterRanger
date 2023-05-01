using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarAttack_Gallant : Attack_Gallant
{
    public float roarRadius;
    public GameObject aura;
    public GameObject roar;

    public AudioSource absorbSound;
    public AudioSource roarSound;

    public float pullStrength;
    public float repelStrength;

    public float maxWalkTime = 1f;

    private List<Rigidbody> _rigidbodiesInRadius;

    #region Attack Steps
    public override void StartAttack()
    {
        StartCoroutine(WalkToPlayer());
    }

    public IEnumerator WalkToPlayer()
    {
        Vector3 location = Vector3.Lerp(_entity.transform.position, _entity.blackboard.Target.transform.position, .8f);

        _entity.agent.SetDestination(location);
        _entity.agent.isStopped = false;

        float t = 0;

        while (Vector3.Distance(_entity.transform.position, location) > .1f && t < maxWalkTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        base.StartAttack();
        _entity.agent.isStopped = true;
    }

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        switch (attackStep)
        {
            case 0:
                StartCoroutine(GrowAura());
                break;
            case 1:
                StartCoroutine(PullRigidbodies());
                break;
            case 2:
                StartCoroutine(RepelRigidbodies());
                break;
        }
    }

    public IEnumerator GrowAura()
    {
        aura.SetActive(true);

        float t = 0;

        while (t < .5f)
        {
            aura.transform.localScale = Vector3.Slerp(Vector3.one, Vector3.one * roarRadius, t / .5f);
            t += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator PullRigidbodies()
    {
        absorbSound.Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, roarRadius, ~0);
        _rigidbodiesInRadius = new List<Rigidbody>();

        foreach (Collider col in colliders)
        {
            if (!col.attachedRigidbody) continue;
            _rigidbodiesInRadius.Add(col.attachedRigidbody);
        }

        float t = 0f;
        float length = 1f;

        while (t < length)
        {
            t += Time.fixedDeltaTime;
            float percent = t / length;

            aura.transform.localScale = Vector3.Lerp(Vector3.one * roarRadius, Vector3.one, percent);

            foreach (Rigidbody rigidbody in _rigidbodiesInRadius)
            {
                Vector3 direction = (-rigidbody.position + transform.position).normalized;
                rigidbody.AddForce(direction * pullStrength * (1 - percent), ForceMode.Force);
            }

            yield return new WaitForFixedUpdate();
        }

        aura.SetActive(false);
    }

    public IEnumerator RepelRigidbodies()
    {
        float t = 0f;
        float length = .5f;

        roar.SetActive(true);
        roarSound.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, roarRadius, ~0);
        _rigidbodiesInRadius = new List<Rigidbody>();

        foreach (Collider col in colliders)
        {
            if (!col.attachedRigidbody) continue;
            _rigidbodiesInRadius.Add(col.attachedRigidbody);
        }

        while (t < length)
        {
            t += Time.fixedDeltaTime;

            float percent = t / length;
            roar.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * roarRadius, percent);

            foreach (Rigidbody rigidbody in _rigidbodiesInRadius)
            {
                Vector3 direction = (rigidbody.position - transform.position).normalized;
                rigidbody.AddForce(direction * repelStrength * length, ForceMode.Force);
            }

            yield return new WaitForFixedUpdate();
        }

        roar.SetActive(false);

    }
    #endregion




}
