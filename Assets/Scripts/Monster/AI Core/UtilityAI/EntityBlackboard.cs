using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public class EntityBlackboard : Blackboard
    {

        [Header("Survival Stats")]
        private SensorManager _sensorManager;

        public float WaterMeter { get; private set; }
        public float HungerMeter { get; private set; }
        public float SleepMeter { get; private set; }

        public List<Area> WaterAreas { get; private set; }
        public List<Area> FoodAreas { get; private set; }

        public float foodConsumeRange;
        public float waterConsumeRange;

        public Entity entity { get; private set; }

        public GameObject Target { get; set; }

        public float maxTravelDistance;
        bool isSleeping = false;

        public override void Init()
        {
            _sensorManager = GetComponentInChildren<SensorManager>();
            _sensorManager.Init();

            WaterAreas = new List<Area>();
            FoodAreas = new List<Area>();
        }

        public List<T> GetDetected<T>() where T : MonoBehaviour
        {
            _sensorManager.RemoveNulls();

            List<T> detectedList = new List<T>();

            foreach (IDetectable detect in _sensorManager._detectables)
            {
                if (detect.mono is T)
                {
                    detectedList.Add((T)detect.mono);
                }
            }

            return detectedList;
        }

        public void AddArea(Area area)
        {
            switch (area.type)
            {
                case AreaType.Food:
                    if (FoodAreas.Contains(area)) return;
                    FoodAreas.Add(area);
                    break;
                case AreaType.Water:
                    if (WaterAreas.Contains(area)) return;
                    WaterAreas.Add(area);
                    break;
            }
        }



    }
}