using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public class Area : MonoBehaviour
    {
        public AreaType type;
        
        void OnTriggerEnter(Collider other)
        {
            Entity entity = other.GetComponentInParent<Entity>();
            if (!entity) return;

            entity.blackboard.AddArea(this);
        }
    }

    public enum AreaType
    {
        Water,
        Food
    }
}