using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IDetectable
{
    public MonoBehaviour mono => this;

    public Action onDestroy { get; set; }
    private void OnDestroy() => onDestroy?.Invoke();
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
