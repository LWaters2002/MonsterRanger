using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;

[CreateAssetMenu(fileName = "HostileProximity_F", menuName = "ScriptObjects/Factors/HostileProximity", order = 0)]
public class HostileProximity_Factor : Factor
{
    public float maxDistance;

    public override float Evaluate()
    {

        List<PlayerCharacter> players = blackboard.GetDetected<PlayerCharacter>();

        if (players.Count == 0) return 0;

        float distance = Vector3.Distance(players[0].transform.position, blackboard.transform.position);

        float distancePercent = distance / maxDistance;
        distancePercent = Mathf.Clamp01(distancePercent);

        return 1 - distancePercent;
    }
}
