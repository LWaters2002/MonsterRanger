using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleAttack_Gallant : Attack_Gallant
{
    public GameObject prefab;

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        switch (attackStep)
        {
            case 0:
                CreateOrb(2f);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    private void CreateOrb(float length)
    {

    }

    private IEnumerator ScaleOrb()
    {
        yield return null;
    }

    public override float CalculateEffectiveness()
    {
        return base.CalculateEffectiveness();
    }
}
