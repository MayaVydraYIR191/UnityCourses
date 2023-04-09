using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CoountRotation : MonoBehaviour
{
    TMP_Text countRotation;
    public static double rotationCount;
    void Start()
    {
        countRotation = GetComponent<TMP_Text>();
    }
    void Update()
    {
        countRotation.text = rotationCount.ToString();
    }
}
