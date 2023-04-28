using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour, IDetectable
{
    public MonoBehaviour mono => this;

    public Action onDestroy { get; set; }
    private void OnDestroy() => onDestroy?.Invoke();
}
