using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;
using UnityEngine.AI;

public class Entity : MonoBehaviour, IDetectable
{
    public UtilityAI utilityAI { get; private set; }
    public FSM fsm { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; private set; }
    public EntityBlackboard blackboard { get; private set; }

    public Transform mouth;

    public System.Action ArrivedAtLocation;

    private bool travellingToLocation;

    protected virtual void Start()
    {
        utilityAI = GetComponent<UtilityAI>();
        fsm = new FSM(new Dictionary<System.Type, State>(), null);
        agent = GetComponent<NavMeshAgent>();
        blackboard = GetComponent<EntityBlackboard>();
        animator = GetComponent<Animator>();

        fsm.Init(this);
        utilityAI.Init(this);
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

