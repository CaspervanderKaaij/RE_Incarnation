using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreRotation : MonoBehaviour
{

    Quaternion startRot;

    void Start()
    {
        startRot = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = startRot;
    }
}
