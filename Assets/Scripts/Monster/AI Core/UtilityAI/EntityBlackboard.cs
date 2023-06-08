using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public class EntityBlackboard : Blackboard
    {

        public Transform RoamArea;
        
        [Header("Survival Stats")]
        private SensorManager _sensorManager;

        public float maxTravelDistance;

        [Header("Food")]
        public float foodConsumeRange;
        public float foodDecay;
        public float maxFoodMeter;
        private float _foodMeter;

        public List<Area> FoodAreas { get; private set; }
        public float GetFoodMeter() => _foodMeter / maxFoodMeter;

        [Header("Water")]
        public float waterConsumeRange;
        public float waterDecay;
        public float maxWaterMeter;
        private float _waterMeter;

        public float GetWaterMeter() => _waterMeter / maxWaterMeter;
        public List<Area> WaterAreas { get; private set; }

        [Header("Sleep")]
        public float sleepDecay;
        public float maxSleepMeter;
        private float _sleepMeter = 10;

        public float GetSleepMeter() => _sleepMeter / maxSleepMeter;
        public List<Area> SleepAreas { get; private set; }

        public Entity entity { get; private set; }

        public GameObject Target { get; set; }
        public float DistanceToTarget { get; set; }

        bool isSleeping = false;

        public override void Init()
        {
            _sensorManager = GetComponentInChildren<SensorManager>();
            _sensorManager.Init();

            WaterAreas = new List<Area>();
            FoodAreas = new List<Area>();
            SleepAreas = new List<Area>();

            GetAndSortAreas();
        }

        public void GetAndSortAreas()
        {
            Area[] coreAreas = FindObjectsOfType<Area>();

            foreach (Area a in coreAreas)
            {
                if (!a.isCore) continue;
                AddArea(a);
            }
        }

        public void AdjustFood(float amount)
        {
            _foodMeter += amount;
            _foodMeter = Mathf.Clamp(_foodMeter, 0f, maxFoodMeter);
        }

        public void AdjustSleep(float amount)
        {
            _sleepMeter += amount;
            _sleepMeter = Mathf.Clamp(_sleepMeter, 0f, maxSleepMeter);
        }

        public void AdjustWater(float amount)
        {
            _waterMeter += amount;
            _waterMeter = Mathf.Clamp(_waterMeter, 0f, maxWaterMeter);
        }

        public virtual void SetEntity(Entity entity)
        {
            this.entity = entity;
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

        protected virtual void Update()
        {
            if (!Target) return;
            DistanceToTarget = Vector3.Distance(Target.transform.position, entity.transform.position);
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
                case AreaType.Sleep:
                    if (SleepAreas.Contains(area)) return;
                    SleepAreas.Add(area);
                    break;
            }
        }

        public Vector3 GetClosestArea(AreaType type)
        {
            List<Area> areaList = null;

            switch (type)
            {
                case AreaType.Food:
                    areaList = FoodAreas;
                    break;
                case AreaType.Water:
                    areaList = WaterAreas;
                    break;
                case AreaType.Sleep:
                    areaList = SleepAreas;
                    break;
            }

            if (areaList == null) return Vector3.zero;

            return areaList.GetClosestArea(transform.position);

        }



    }
}