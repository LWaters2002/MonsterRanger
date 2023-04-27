using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UtilAI;

using Gallant;

[RequireComponent(typeof(Gallant_Blackboard))]
[RequireComponent(typeof(UtilityAI))]
public class Gallant_Entity : Entity
{
    private PlayerCharacter _playerCharacter;

    public LookAtConstraint _HeadLookAtConstraint;
    private ConstraintSource _TargetConstraint;

    private UtilityAI _UtilityAI;

    protected override void Start()
    {
        base.Start();

        SetConstraints();
        
        _UtilityAI = GetComponent<UtilityAI>();
        _UtilityAI.Init(this);
    }

    private void SetConstraints()
    {

    }

    public void LookAtTarget(bool lookAtTarget)
    {
        _HeadLookAtConstraint.constraintActive = lookAtTarget;
    }

}
