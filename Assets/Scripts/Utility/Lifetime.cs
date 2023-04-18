using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = -1;
    private float _life;

    public bool active = false;

    public void SetLifetime(float life)
    {
        active = true;
        lifetime = life;
    }

    private void Update()
    {
        if (!active) return;

        if (_life > lifetime) Destroy(gameObject);
        _life += Time.deltaTime;
    }
}
