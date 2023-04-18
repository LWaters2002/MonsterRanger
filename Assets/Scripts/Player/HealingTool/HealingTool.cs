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

    [Header("Fire Particles")]
    public ParticleSystem beamParticle;
    public ParticleSystem attractedParticle;
    public float attractionForce;

    public UnityEvent OnFire;

    private Coroutine chargeCoroutine;
    private bool _isPressed;
    private bool _isCharging;
    private bool _isMaxCharge;

    private GameObject _attractor;

    public System.Action<bool> OnCharge;
    public StatModifier speedModifier;

    public void Init(PlayerCharacter player)
    {
        Player = player;
        Player.movement.OnDash += CancelHeal;
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

            bool isStamina = Player.playerStats.ConsumeStamina(staminaCost * Time.deltaTime);

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
        var beamMain = beamParticle.main;

        beamMain.startColor = Color.white;
        main.startColor = Color.white;

        var attractedMain = attractedParticle.main;
        attractedMain.startColor = Color.white;

        _isMaxCharge = false;
    }

    private void MaxCharged()
    {
        var main = chargeParticles.main;
        main.startColor = maxChargeColour;

        var beamMain = beamParticle.main;
        beamMain.startColor = maxChargeColour;

        var attractedMain = attractedParticle.main;
        attractedMain.startColor = maxChargeColour;

        _isMaxCharge = true;
    }

    private void ScaleParticlesToCharge(float percent)
    {
        var shape = chargeParticles.shape;
        shape.scale = Vector3.one * chargeParticleScale * percent;

        var emission = chargeParticles.emission;
        emission.rateOverTime = percent * maxEmissionRate;

        var attractedEmission = attractedParticle.emission;
        attractedEmission.rateOverTime = (percent + .2f) * maxEmissionRate;

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

            // if (_attractor) Destroy(_attractor);

            _attractor = new GameObject("Particle Target");
            _attractor.transform.position = hit.point;
            _attractor.transform.parent = hit.collider.gameObject.transform;

            var forceField = _attractor.AddComponent<ParticleSystemForceField>();
            forceField.endRange = 200.0f;
            forceField.gravity = 2f;
            forceField.drag = 200f;

            // _attractor.AddComponent<Lifetime>().SetLifetime(3f);

            HealInfo tempInfo;
            tempInfo.amount = Mathf.Round(healAmount * percent) * ((_isMaxCharge) ? maxHealMultiplier : 1f);
            tempInfo.infusion = E_Infusion.None;

            SpawnFireflyProjectiles(percent, _attractor.transform, tempInfo);

            IHealable healable = hit.collider.GetComponentInParent<IHealable>();

            if (healable != null)
            {
                healable.Heal(tempInfo);
            }

            Debug.DrawLine(Player.orientation.position, hit.point, Color.red, 2f);
        }

    }

    private void SpawnFireflyProjectiles(float percent, Transform target, HealInfo healInfo)
    {

        for (int i = 0; i < 5; i++)
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
