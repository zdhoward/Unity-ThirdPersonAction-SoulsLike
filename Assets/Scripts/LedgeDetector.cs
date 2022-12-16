using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3> OnLedgeDetect;

    void OnTriggerEnter(Collider other)
    {
        OnLedgeDetect?.Invoke(other.transform.forward);
    }
}
