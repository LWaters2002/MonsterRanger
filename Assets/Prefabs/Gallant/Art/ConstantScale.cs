using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantScale : MonoBehaviour
{
    public Vector3 scale;

    // Update is called once per frame
    void Update()
    {
        float sX = scale.x / transform.parent.localScale.x;
        float sY = scale.y / transform.parent.localScale.y;
        float sZ = scale.z / transform.parent.localScale.z;

        transform.localScale = new Vector3(sX, sY, sZ);
    }
}
