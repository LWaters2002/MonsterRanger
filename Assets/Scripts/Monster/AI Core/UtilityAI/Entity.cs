using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;
using UnityEngine.AI;

[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class Entity : MonoBehaviour, IDetectable
{
    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; private set; }
    public EntityBlackboard blackboard { get; private set; }
    public AttackController attackController { get; private set; }

    public GameObject Target { get; private set; }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
        blackboard = GetComponent<EntityBlackboard>();

        blackboard.Init();
        attackController.Init(this);
    }

    protected virtual void Update()
    {

    }

}

