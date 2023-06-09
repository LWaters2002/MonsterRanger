using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StompAttack_Gallant : Attack_Gallant
{
    public EnemyProjectile projectilePrefab;

    public int projectileCount;
    public float radius;
    public float projectileDamage;

    public AudioSource launchSound;

    public UnityEvent OnStompLand;

    private List<EnemyProjectile> _projectiles;
    private bool _turning = false;

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        switch (attackStep)
        {
            case 0:
                StartCoroutine(GenerateProjectiles());
                _turning = true;
                StartCoroutine(Turn());
                break;
            case 1:
                _turning = false;
                ChargeProjectiles();
                break;
            case 2:
                StartCoroutine(ShootAtPlayer());
                break;
            case 3: //Stomp Landed
                OnStompLand?.Invoke();
                break;
        }
    }


    private IEnumerator Turn()
    {
        while (_turning)
        {
            _entity.TurnToTargetTicked(1f);
            yield return null;
        }
    }

    public override void StopAttack()
    {
        base.StopAttack();

        if (_projectiles == null) return;

        foreach (EnemyProjectile projectile in _projectiles)
        {
            Destroy(projectile);
        }
    }

    private void ChargeProjectiles()
    {
        float segment = Mathf.PI / projectileCount;
        const float offset = -Mathf.PI / 2;

        for (int i = 1; i <= projectileCount; i++)
        {
            float angle = segment * i + offset - segment / 2;

            Vector3 target = transform.position + GetPositionGivenRadius(angle, radius);
            _projectiles[i - 1].SetTarget(target, 1f);
        }

        float t = 0;

        while (t < .7f)
        {
            t += Time.deltaTime;

            foreach (EnemyProjectile projectile in _projectiles)
            {
                projectile.transform.localScale = Vector3.one * (1 + t);
            }
        }
    }

    private IEnumerator ShootAtPlayer()
    {
        launchSound.Play();

        GameObject target = FindObjectOfType<PlayerCharacter>().gameObject;

        foreach (EnemyProjectile projectile in _projectiles)
        {
            projectile.SetState(ProjectileState.Physics);
            projectile.LaunchAtTarget(target.transform.position, 30f);
            yield return new WaitForSeconds(.8f / projectileCount);
        }
    }

    private IEnumerator GenerateProjectiles()
    {
        float segment = Mathf.PI / projectileCount;
        const float offset = -Mathf.PI / 2;

        _projectiles = new List<EnemyProjectile>();

        for (int i = 1; i <= projectileCount; i++)
        {
            EnemyProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            float angle = segment * i + offset - segment / 2;

            Vector3 target = transform.position + GetPositionGivenRadius(angle, radius);
            projectile.Init(projectileDamage, target, _entity);

            _projectiles.Add(projectile);
            yield return new WaitForSeconds(.8f / projectileCount);

        }
    }

    private Vector3 GetPositionGivenRadius(float angle, float inRadius)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        Vector3 vT = (sin * transform.right + cos * transform.up) * inRadius;
        return vT;
    }
}
