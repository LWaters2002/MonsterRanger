using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;

namespace Gallant
{
    public abstract class Gallant_Action : Action
    {
        Gallant_Entity gallantEntity;

        public override void Init(UtilityAI utilityAI, Entity entity)
        {
            base.Init(utilityAI, entity);
            gallantEntity = (Gallant_Entity)entity;
        }
    }
}
