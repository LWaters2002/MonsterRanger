using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gallant;

[CreateAssetMenu(fileName = "ConsumeFood_GA", menuName = "ScriptObjects/Actions/Gallant/ConsumeFood", order = 0)]
public class ConsumeFood_GA : Gallant_Action
{
    public override void Entry()
    {
        AttackComponent attackC = entity.attackController.SelectAttack("ConsumeFood");
        attackC.OnAttackComplete += Complete;
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override void Tick(float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}
