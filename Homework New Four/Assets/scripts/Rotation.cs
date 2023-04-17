using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Rotation : MonoBehaviour
{
    [SerializeField] TMP_Text countRotation;
    public void OnButtonEnter()
    {
        StartCoroutine(Rotationing(Vector3.back * 180, 0.5f));
        CoountRotation.rotationCount = transform.rotation.z;
    }

    IEnumerator Rotationing(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }
}
