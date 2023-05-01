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


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
        blackboard = GetComponent<EntityBlackboard>();
        agent = GetComponent<NavMeshAgent>();

        blackboard.Init();
        attackController.Init(this);
    }

    protected virtual void Update()
    {

    }

    public void TurnToTargetTicked(float speed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(blackboard.Target.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

}

