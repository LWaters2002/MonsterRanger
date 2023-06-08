using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UtilAI;
using Gallant;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using DG.Tweening;

[RequireComponent(typeof(Gallant_Blackboard))]
[RequireComponent(typeof(UtilityAI))]
public class Gallant_Entity : Entity
{
    private PlayerCharacter _playerCharacter;

    public LookAtConstraint _HeadLookAtConstraint;
    private ConstraintSource _TargetConstraint;

    private UtilityAI _UtilityAI;
    private Hitbox[] _hitboxes;

    private bool _encountered;

    [Header("Visuals")]
    public Renderer bodyRenderer;
    public Material skinMaterial;
    public Material flashMaterial;
    public Material starMaterial;
    public ParticleSystem HealPartFX;

    [Header("Final Segment")]
    public Transform[] teleportPoints;
    public ParticleSystem fireflyFX;

    [Header("Dialogue Sequence")]
    public DialogueSequence powerOverflowDialogue;
    public DialogueSequence markingDialogue;
    public DialogueSequence goodJobDoneDialogue;

    [Header("Cutscens")]
    public PlayableDirector firstEncounterCutscene;
    public PlayableDirector endCutscene;

    bool endStarted;

    protected override void Start()
    {
        base.Start();

        _UtilityAI = GetComponent<UtilityAI>();
        _UtilityAI.Init(this);

        _hitboxes = GetComponentsInChildren<Hitbox>();

        foreach (Hitbox hitbox in _hitboxes)
        {
            hitbox.OnFullHeal.AddListener(CheckAllHitboxes);
            hitbox.OnFullHeal.AddListener(HealFlash);
        }
    }

    private void HealFlash()
    {
        StartCoroutine("HealAnimation");
    }

    private IEnumerator HealAnimation()
    {
        if (!canAttack) yield break;

        bodyRenderer.material = flashMaterial;
        HealPartFX.Play();

        animator.Play("QuickExhaust");
        agent.isStopped = true;
        attackController.SetEnabledAllAttacks(false);
        attackController.enabled = false;
        RadialYeet();

        yield return new WaitForSeconds(3.042f);

        animator.Play("Idle");

        attackController.enabled = true;
        attackController.ChooseAttack();

        bodyRenderer.material = skinMaterial;
    }

    private void RadialYeet()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30f, ~0);

        foreach (Collider col in colliders)
        {
            if (col.attachedRigidbody)
            {
                col.attachedRigidbody.AddExplosionForce(40f, transform.position, 10f, 2f);
            }
        }
    }

    private void CheckAllHitboxes()
    {
        foreach (Hitbox hitbox in _hitboxes)
        {
            if (!hitbox.isHealed) return;
        }

        FullyHealed();
    }

    private void FullyHealed()
    {
        if (endStarted) return;
        endStarted = true;
        Debug.Log("GALLANT FULLY HEALED");
        StartCoroutine(StartEnraged());
    }

    IEnumerator StartEnraged()
    {
        canAttack = false;

        PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();

        animator.CrossFade("Idle", .2f);

        _UtilityAI.enabled = false;
        agent.enabled = false;
        agent.isStopped = true;

        bodyRenderer.material = starMaterial;
        attackController.SetEnabledAllAttacks(false);
        attackController.enabled = false;

        yield return new WaitForSeconds(2f);

        animator.CrossFade("ChargeSequence", .2f);

        GameManager.Get().PlayDialogue(powerOverflowDialogue);

        yield return new WaitForSeconds(2f);

        playerCharacter.rb.isKinematic = true;

        yield return new WaitForSeconds(4f);

        animator.Play("SpaceFloat");

        int pointLength = teleportPoints.Length - 1;
        float offset = 6f;

        for (int i = 0; i <= pointLength; i++)
        {
            transform.SetPositionAndRotation(teleportPoints[i].position, teleportPoints[i].rotation);

            Quaternion targetRotation = teleportPoints[i].rotation * Quaternion.Euler(0, 180, 0);
            Vector3 targetPoint = teleportPoints[i].position + teleportPoints[i].forward * offset + Vector3.one;

            playerCharacter.rb.DOMove(targetPoint, .2f);
            playerCharacter.rb.DOLookAt(teleportPoints[i].position, .2f);

            yield return new WaitForSeconds(2.5f);
        }

        animator.Play("EndExhaust");

        playerCharacter.rb.isKinematic = false;

        yield return new WaitForSeconds(2f);

        fireflyFX.Play();

        GameManager.Get().PlayDialogue(markingDialogue);
        yield return new WaitForSeconds(7f);

        fireflyFX.Stop();

        animator.CrossFade("Sleep", .2f);

        yield return new WaitForSeconds(3f);
        GameManager.Get().PlayDialogue(goodJobDoneDialogue);

        yield return new WaitForSeconds(10f);

        SceneManager.LoadScene("MainMenu");
    }

    public void LookAtTarget(bool lookAtTarget)
    {
        _HeadLookAtConstraint.constraintActive = lookAtTarget;
    }

    public void FirstEncounter(System.Action callback) => StartCoroutine(IEFirstEncounter(callback));

    private IEnumerator IEFirstEncounter(System.Action callback)
    {
        if (_encountered) { callback?.Invoke(); yield break; }
        _encountered = true;

        agent.isStopped = true;
        attackController.SetEnabledAllAttacks(false);
        attackController.enabled = false;

        GameManager.Get().PlayCutscene(firstEncounterCutscene);
        
        double t = firstEncounterCutscene.playableAsset.duration;

        yield return new WaitForSeconds((float)t);

        attackController.enabled = true;

        callback?.Invoke();
    }

}
