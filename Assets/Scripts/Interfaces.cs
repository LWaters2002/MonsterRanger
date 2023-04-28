using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectable
{
    public GameObject gameObject { get; }
    public MonoBehaviour mono { get; }
    public System.Action onDestroy { get; set; }
}

public enum DetectableType
{
    food,
    drink,
    player
}

public interface IInteractable
{
    public GameObject gameObject { get; }

    public void StartedLooking(PlayerInteractor interactor);
    public void StoppedLooking(PlayerInteractor interactor);
}

public interface IDamagable
{
    public GameObject gameObject { get; }

    public void TakeDamage(Damage damage);
}

public struct Damage
{
    public float amount;
}

public static class ExtensionMethods
{
    public static Vector3 GetClosestArea(this List<UtilAI.Area> locations, Vector3 position)
    {
        Vector3 closest = locations[0].transform.position;

        float closestDistance = Vector3.Distance(position, closest);

        foreach (UtilAI.Area location in locations)
        {
            float distance = Vector3.Distance(position, location.transform.position);

            if (distance < closestDistance)
            {
                closest = location.transform.position;
                closestDistance = distance;
            }
        }

        return closest;
    }
}