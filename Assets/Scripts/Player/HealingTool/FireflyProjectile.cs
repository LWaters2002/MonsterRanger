using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class FireflyProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _spreadForce;

    [Header("References")]
    public PopupText popupTextPrefab;
    public ParticleSystem[] particleSystems;

    private Rigidbody _rigidbody;
    private Transform _target;

    private HealInfo _healInfo;

    public UnityEvent OnHit;

    private bool _hasHit = false;

    private Material _material;

    public void Init(Transform target, HealInfo healInfo)
    {
        _healInfo = healInfo;
        _healInfo.amount *= Random.Range(.9f, 1.1f);
        _healInfo.amount = Mathf.RoundToInt(_healInfo.amount);

        _target = target;

        _rigidbody = GetComponent<Rigidbody>();

        CacheMaterial();
        SetInfusionColour();

        StartCoroutine("Travel"); ;
    }

    private void CacheMaterial()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        _material = new Material(renderer.materials[1]);

    }

    private void SetInfusionColour()
    {
        var colour = HealHelper.InfusionLookup[((int)_healInfo.infusion)];
        
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            var main = particleSystem.main;
            main.startColor = colour;
        }

        _material.SetColor("_EmissionColor", colour);
    }

    IEnumerator Travel()
    {
        Vector2 randDirection = Random.insideUnitCircle.normalized * Random.Range(.7f, 1.1f);

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
        if (_target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;
        _rigidbody.AddForce(direction * _speed, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_hasHit) return;
        IHealable healable = other.GetComponentInParent<IHealable>();

        if (healable != null)
        {
            healable.Heal(_healInfo);
            SpawnPopupText();

            OnHit?.Invoke();

            _hasHit = true;
        }
    }

    private void SpawnPopupText()
    {
        Vector3 spawnPosition = transform.position - transform.forward * .5f;
        Vector2 offset = Random.insideUnitCircle * .3f;
        spawnPosition += offset.x * transform.right + offset.y * transform.up;

        PopupText text = Instantiate(popupTextPrefab, spawnPosition, Quaternion.identity);

        text.transform.localScale = Random.Range(.5f, .8f) * Vector3.one;
        text.Init(_healInfo.amount.ToString(), Color.green);
    }
}
