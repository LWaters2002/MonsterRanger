using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class Entity : MonoBehaviour, IDetectable
{
    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; private set; }
    public EntityBlackboard blackboard { get; private set; }
    public AttackController attackController { get; private set; }

    public MonoBehaviour mono => this;

    public System.Action onDestroy { get; set; }
    private void OnDestroy() => onDestroy?.Invoke();

    public float maxAgentSpeed = 6f;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
        blackboard = GetComponent<EntityBlackboard>();
        agent = GetComponent<NavMeshAgent>();

        blackboard.Init();
        blackboard.SetEntity(this);
        attackController.Init(this);
    }

    protected virtual void Update()
    {
        animator?.SetFloat("Speed", agent.speed / maxAgentSpeed);
    }

    public void TurnToTargetTicked(float speed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(blackboard.Target.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    public IEnumerator WalkToTarget(System.Action callback, float walkTime, float walkPercent, float speed)
    {
        Vector3 location = Vector3.Lerp(transform.position, blackboard.Target.transform.position, walkPercent);

        agent.speed = speed;

        agent.SetDestination(location);
        agent.isStopped = false;

        animator.SetBool("isMoving", true);

        float t = 0;

        while (Vector3.Distance(transform.position, location) > .2f && t < walkTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();

        animator.SetBool("isMoving", false);
        agent.isStopped = true;
    }
}

