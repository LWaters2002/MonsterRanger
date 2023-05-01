using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDamagedEffect : MonoBehaviour
{
    public PlayerCharacter playerCharacter;

    [Header("Movement Debuff")]
    public StatModifier movementDebuff;
    public float debuffDuration;

    [Header("ScreenShake")]
    public Cinemachine.CinemachineImpulseSource impulseSource;

    [Header("Vignette Flash")]
    public float vignetteDuration;
    public float vignetteIntensity;
    public AnimationCurve vignetteCurve;
    private Vignette vignette;

    private IEnumerator _alterRoutine;

    private void Start()
    {
        PlayerStats stats = playerCharacter.stats;

        stats.OnDamaged += RecieveDamage;
        GetPostProcessComponents();

        _alterRoutine = AlterFloatParameterForDuration(vignette.intensity, vignetteIntensity, vignetteDuration, vignetteCurve);

    }

    private void GetPostProcessComponents()
    {
        Volume volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out vignette);
    }

    IEnumerator AlterFloatParameterForDuration(FloatParameter parameter, float targetValue, float duration, AnimationCurve curve)
    {
        float startValue = parameter.value;

        float t = 0f;

        while (t < targetValue)
        {
            parameter.value = Mathf.Lerp(startValue, targetValue, curve.Evaluate(t));

            t += Time.deltaTime;
            yield return null;
        }

        parameter.value = startValue;
    }

    private void RecieveDamage(float damageAmount)
    {
        impulseSource.GenerateImpulse(.1f);
        StopCoroutine(_alterRoutine);
        StartCoroutine(_alterRoutine);
        playerCharacter.movement.AddMovementMultiplier(movementDebuff, debuffDuration);
    }
}
