﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Shake : MonoBehaviour
{

    bool isShaking = false;
    float shakestr = 0.5f;
    PostProcessingBehaviour pp;
    [SerializeField] float shakeScale = 1;
    Vector3 rngRemover;


    void Start()
    {
        if (GetComponent<PostProcessingBehaviour>() != null)
        {
            pp = Camera.main.GetComponent<PostProcessingBehaviour>();
            pp.profile.motionBlur.enabled = false;
        }
    }

    void LateUpdate()
    {
        if (isShaking == true)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            Shaking(shakestr);
        }
    }

    void StartShake(float time, float strength)
    {
        CancelInvoke("StopShake");
        isShaking = true;
        shakestr = strength;
        Invoke("StopShake", time);
        transform.eulerAngles -= rngRemover;
        rngRemover = Vector3.zero;
        if (pp != null)
        {
            pp.profile.motionBlur.enabled = true;
        }
    }

    public void SmallShake()
    {
        StartShake(0.15f, 0.25f);
    }

    public void MediumShake()
    {
        StartShake(0.2f, 0.5f);
    }

    public void HardShake()
    {
        StartShake(0.3f, 1f);
    }

    void StopShake()
    {
        isShaking = false;
        if (pp != null)
        {
            pp.profile.motionBlur.enabled = false;
        }
        transform.eulerAngles -= rngRemover;
        rngRemover = Vector3.zero;
    }

    void Shaking(float str)
    {
        transform.eulerAngles -= rngRemover;
        str *= shakeScale;
        rngRemover = new Vector3(Random.Range(-str, str),Random.Range(-str, str),Random.Range(-str, str));
        transform.localEulerAngles = new Vector3(transform.eulerAngles.x + rngRemover.x, transform.eulerAngles.y + rngRemover.y, rngRemover.z);
    }
}
