using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{

    public AnimationCurve turnCurve;

    public void Init(Vector3 target)
    {
        StartCoroutine(FloatAndPoint(target));
    }

    IEnumerator FloatAndPoint(Vector3 target)
    {
        float t = 0;

        while (t < 1)
        {
            transform.position += Vector3.up * .5f * Time.deltaTime * .25f;
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        float length = 1.5f;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target);

        while (t < length)
        {
            float eval = turnCurve.Evaluate(t / length);

            transform.rotation = Quaternion.LerpUnclamped(startRotation, targetRotation, eval);

            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        t = 0;

        float shrinkLength = 1f;

        while (t < shrinkLength)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t / shrinkLength);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

