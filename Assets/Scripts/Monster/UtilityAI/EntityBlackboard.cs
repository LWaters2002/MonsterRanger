using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public class EntityBlackboard : Blackboard
    {
        #region Variables

        //Sensors

        private List<Sensor> _sensors;

        public HashSet<IDetectable> detectedEntities { get; private set; }

        System.Action<HashSet<IDetectable>> DectatablesUpdated;

        [Header("Survival Stats")]

        [Space]
        public float water;
        public float maxWater;

        public float waterDecayRate;

        [Space]
        public float hunger;
        public float maxHunger;

        public float hungerDecayRate;

        [Space]
        public float sleepiness;
        public float maxSleepiness;

        [Header("Important Locations")]
        public List<Transform> waterAreas;
        public List<Transform> foodAreas;

        public float maxTravelDistance;

        bool isSleeping = false;

        #endregion

        #region Functions

        public override void Init()
        {
            _sensors = new List<Sensor>();
            _sensors.AddRange(GetComponentsInChildren<Sensor>());
            _sensors.ForEach(x => x.Init(this));

            detectedEntities = new();
        }

        public void UpdateDectatables()
        {
            detectedEntities.Clear();

            foreach (Sensor sensor in _sensors)
            {
                detectedEntities.UnionWith(sensor.detectables);
            }

            DectatablesUpdated?.Invoke(detectedEntities);
        }

        public Vector3 GetClosestFood()
        {
            return GetClosestLocation(foodAreas);
        }

        public Vector3 GetClosestWater()
        {
            return GetClosestLocation(waterAreas);
        }

        protected Vector3 GetClosestLocation(List<Transform> locations)
        {
            Vector3 closest = locations[0].position;

            float closestDistance = Vector3.Distance(transform.position, closest);

            foreach (Transform location in locations)
            {
                float distance = Vector3.Distance(transform.position, location.position);

                if (distance < closestDistance)
                {
                    closest = location.position;
                    closestDistance = distance;
                }
            }

            return closest;
        }
        #endregion
    }
}