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

    public System.Action ArrivedAtLocation;

    private bool travellingToLocation;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();

        attackController.Init(this);
    }

    public virtual void GoToLocation(Vector3 location)
    {
        agent.SetDestination(location);
        Invoke("TravelBreak", .2f);
    }

    private void TravelBreak()
    {
        travellingToLocation = true;
    }

    protected virtual void Update()
    {

        if (!travellingToLocation) return;

        if (Vector3.Distance(transform.position, agent.pathEndPosition) < .2f)
        {
            ArrivedAtLocation?.Invoke();
            travellingToLocation = false;
        }
    }

}

