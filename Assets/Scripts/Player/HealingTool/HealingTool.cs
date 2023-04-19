using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealingTool : MonoBehaviour
{
    public PlayerCharacter Player { get; private set; }
    public FireflyProjectile fireflyPrefab;
    public LayerMask mask;

    [Header("Stats")]
    public float range;
    public float staminaCost;
    public float healAmount;
    public Vector2Int FireflyCountRange;
    public float maxHealMultiplier;

    [Header("Charging")]
    public float chargeTime;
    public float minimumChargeTime;
    private float _timeCharged;

    [Header("Charge Particle")]
    public ParticleSystem chargeParticles;
    public float chargeParticleScale;
    public float maxEmissionRate;
    public Color fullChargeColour;
    public Color maxChargeColour;

    [Header("ScreenShake")]
    public Cinemachine.CinemachineImpulseSource impulseSource;

    public UnityEvent OnFire;

    private Coroutine chargeCoroutine;
    private bool _isPressed;
    private bool _isCharging;
    private bool _isMaxCharge;

    public System.Action<bool> OnCharge;
    public StatModifier speedModifier;

    public void Init(PlayerCharacter player)
    {
        Player = player;
        InvokeRepeating("ScreenShake", 0f, .225f);
    }

    private void CancelHeal()
    {
        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        FinishCharge();
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public void Use(bool isPressed)
    {
        _isPressed = isPressed;

        if (isPressed && !_isCharging)
        {
            if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
            chargeCoroutine = StartCoroutine(ChargeAndFire());
        }
    }

    IEnumerator ChargeAndFire()
    {
        OnCharge?.Invoke(true);

        _timeCharged = 0.0f;
        chargeParticles.Play();

        MinCharge();

        _isCharging = true;

        float percent = 0.0f;
        bool maxCharged = false;

        while (_isCharging)
        {
            percent = (_timeCharged / chargeTime);
            percent = Mathf.Clamp01(percent);

            ScaleParticlesToCharge(percent);

            impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 0.12f * percent;

            bool isStamina = Player.stats.ConsumeStamina(staminaCost * Time.deltaTime);

            //If stamina or no button do a minimum charge shot
            if ((!_isPressed || !isStamina) && _timeCharged > minimumChargeTime) _isCharging = false;
            //Signifies Max charge
            if (!maxCharged && _timeCharged > chargeTime) { maxCharged = true; MaxCharged(); }

            _timeCharged += Time.deltaTime;

            yield return null;
        }

        HealCast(percent);
    }

    private void MinCharge()
    {
        var main = chargeParticles.main;
        main.startColor = Color.white;

        _isMaxCharge = false;
    }

    private void MaxCharged()
    {
        var main = chargeParticles.main;
        main.startColor = maxChargeColour;

        _isMaxCharge = true;
    }

    private void ScaleParticlesToCharge(float percent)
    {
        var shape = chargeParticles.shape;
        shape.scale = Vector3.one * chargeParticleScale * percent;

        var emission = chargeParticles.emission;
        emission.rateOverTime = percent * maxEmissionRate;

        var chargeMain = chargeParticles.main;

        float h, s, v;
        Color.RGBToHSV(fullChargeColour, out h, out s, out v);

        if (percent == 1.0f)
        {
            chargeMain.startColor = maxChargeColour;
            return;
        }

        chargeMain.startColor = Color.HSVToRGB(h, s * percent, v);
    }

    public void HealCast(float percent)
    {
        if (!Player) return;

        FinishCharge();
        OnFire?.Invoke();

        if (Physics.Raycast(Player.orientation.position, Player.orientation.forward, out RaycastHit hit, range, mask))
        {
            HealInfo tempInfo;
            tempInfo.amount = Mathf.Round(healAmount * percent) * ((_isMaxCharge) ? maxHealMultiplier : 1f);
            tempInfo.infusion = E_Infusion.None;

            Transform target = SpawnTarget(hit);
            SpawnFireflyProjectiles(percent, target, tempInfo);
        }
    }

    private Transform SpawnTarget(RaycastHit hit)
    {
        GameObject target = new GameObject("Particle Target");
        target.transform.position = hit.point;
        target.transform.parent = hit.collider.gameObject.transform;

        Lifetime lifetime = target.AddComponent<Lifetime>();
        lifetime.SetLifetime(10.0f);

        return target.transform;
    }

    private void SpawnFireflyProjectiles(float percent, Transform target, HealInfo healInfo)
    {

        int count = Mathf.RoundToInt(Mathf.Lerp(FireflyCountRange.x, FireflyCountRange.y, percent));

        for (int i = 0; i < count; i++)
        {
            FireflyProjectile projectile = Instantiate(fireflyPrefab, transform.position, transform.rotation);
            projectile.Init(target, healInfo);
        }
    }

    private void FinishCharge()
    {
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 0.0f;

        _isCharging = false;
        chargeParticles.Stop();

        OnCharge?.Invoke(false);
    }

}
