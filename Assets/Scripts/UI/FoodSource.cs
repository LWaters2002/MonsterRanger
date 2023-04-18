using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour, IDetectable
{
    public System.Action OnConsume;
    private Transform _mouthTarget;

    protected float scaleTarget = .2f;

    private PIDController _controller;
    public float PIDmultiplier;
    public PIDSettings settings;

    private Rigidbody _rigidbody;

    public float foodValue = 1f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;

        _controller = new PIDController(ref settings);
    }

    public void Consume(Transform mouthTarget)
    {
        _mouthTarget = mouthTarget;

        GetComponent<Collider>().enabled = false;
        _rigidbody.isKinematic = false;

        scaleTarget *= Random.Range(.8f, 1.2f);

        StartCoroutine(IE_Consume());
    }

    private IEnumerator IE_Consume()
    {
        float floatUpTime = Random.Range(.8f, 1.3f);
        float time = 0f;

        Vector3 targetPosition = transform.position + Vector3.up * Random.Range(2.5f, 3.5f);

        while (time < floatUpTime)
        {
            Vector3 force = _controller.Update(Time.deltaTime, transform.position, targetPosition);
            time += Time.deltaTime;

            _rigidbody.AddForce(force * PIDmultiplier, ForceMode.Acceleration);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_mouthTarget.position - transform.position), time / floatUpTime);

            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Distance(transform.position, _mouthTarget.position) > 1f)
        {
            Vector3 force = _controller.Update(Time.deltaTime, transform.position, _mouthTarget.position);
            _rigidbody.AddForce(force * PIDmultiplier, ForceMode.Acceleration);

            settings.proportionGain += Time.fixedDeltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(force), Time.fixedDeltaTime * 6f);

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scaleTarget, Time.fixedDeltaTime * 2f);

            yield return new WaitForFixedUpdate();
        }

        OnConsume?.Invoke();
        Destroy(gameObject);
    }
}
