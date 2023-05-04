using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gallant;

[CreateAssetMenu(fileName = "ConsumeFood_GA", menuName = "ScriptObjects/Actions/Gallant/ConsumeFood", order = 0)]
public class ConsumeFood_GA : Gallant_Action
{
    public override void Entry()
    {
        entity.agent.isStopped = true;

        AttackComponent attackC = entity.attackController.SelectAttack("ConsumeFood");
        attackC.OnAttackComplete += Complete;
    }

    public override void FixedTick(float fixedDeltaTime)
    {
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Complete()
    {
        base.Complete();
        entity.blackboard.AdjustFood(22.5f);
    }
}
