using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UtilAI;
using Gallant;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Gallant_Blackboard))]
[RequireComponent(typeof(UtilityAI))]
public class Gallant_Entity : Entity
{
    private PlayerCharacter _playerCharacter;

    public LookAtConstraint _HeadLookAtConstraint;
    private ConstraintSource _TargetConstraint;

    private UtilityAI _UtilityAI;
    private Hitbox[] _hitboxes;

    [Header("Final Segment")]
    public Transform[] teleportPoints;
    public Renderer bodyRenderer;
    public Material starMaterial;
    public ParticleSystem fireflyFX;

    public DialogueSequence powerOverflowDialogue;
    public DialogueSequence markingDialogue;
    public DialogueSequence goodJobDoneDialogue;

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

        PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();

        _UtilityAI.enabled = false;
        agent.enabled = false;

        bodyRenderer.material = starMaterial;
        attackController.SetEnabledAllAttacks(false);
        Destroy(attackController);

        animator.Play("ChargeSequence");
        GameManager.Get().PlayDialogue(powerOverflowDialogue);

        yield return new WaitForSeconds(2f);

        playerCharacter.rb.isKinematic = true;

        yield return new WaitForSeconds(3f);

        float offset = 6f;

        int pointLength = teleportPoints.Length - 1;

        animator.Play("SpaceFloat");

        for (int i = 0; i <= pointLength; i++)
        {
            transform.SetPositionAndRotation(teleportPoints[i].position, teleportPoints[i].rotation);

            playerCharacter.rb.MovePosition(teleportPoints[i].position + transform.forward * offset + Vector3.one);
            playerCharacter.rb.MoveRotation(teleportPoints[i].rotation);

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



}
