using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireflyProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _spreadForce;

    private Rigidbody _rigidbody;
    private Transform _target;

    private HealInfo _healInfo;

    public void Init(Transform target, HealInfo healInfo)
    {
        _healInfo = healInfo;
        _target = target;

        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine("Travel"); ;
    }

    IEnumerator Travel()
    {
        Vector2 randDirection = Random.insideUnitCircle;

        Vector3 spreadDirection = (transform.right * randDirection.x + transform.up * randDirection.y).normalized;
        _rigidbody.AddForce(spreadDirection * _spreadForce, ForceMode.VelocityChange);

        float t = 0;

        Vector3 startScale = transform.localScale;
        float targetSpeed = _speed;
        float minSpeed = _speed / 5f;

        while (t < 0.2f)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, startScale, t / .2f);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;

        while (t < 0.5f)
        {
            _speed = Mathf.Lerp(minSpeed, targetSpeed, t / .5f);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForFixedUpdate();
    }


    private void Update()
    {
        Vector2 direction = _rigidbody.velocity;
        transform.forward = direction.normalized;
    }

    private void FixedUpdate()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        _rigidbody.AddForce(direction * _speed, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealable healable))
        {
            healable.Heal(_healInfo);
            Destroy(gameObject);
        }
    }
}
