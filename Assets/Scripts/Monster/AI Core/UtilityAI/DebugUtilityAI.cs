using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UtilAI
{
    public class DebugUtilityAI : MonoBehaviour
    {
        UtilityAI utilityAI;
        TextMeshPro text;

        void Start()
        {
            utilityAI = GetComponentInParent<UtilityAI>();
            text = GetComponent<TextMeshPro>();
        }

        void Update()
        {
            if (!text || !utilityAI || !utilityAI.currentAction) return;

            text.SetText(utilityAI.currentAction.name + "\n" + utilityAI.blackboard.ToString());
        }
    }
}
