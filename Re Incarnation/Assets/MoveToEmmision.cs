using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToEmmision : MonoBehaviour
{
    Material mat;
    public float curIntensity = 1;
    [SerializeField] AnimationCurve speed;
    [SerializeField] float maxIntensity = 10;
    [SerializeField] Color glowColor = Color.red;
    [SerializeField] float hitIntensityScale = 0.1f;
    [SerializeField] AnimationCurve intensityToEmission;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        mat.SetColor("_EmissionColor", new Color(glowColor.r,glowColor.g,glowColor.b,1.0f) * intensityToEmission.Evaluate(curIntensity));
        curIntensity = Mathf.MoveTowards(curIntensity,0,Time.deltaTime * speed.Evaluate(curIntensity / maxIntensity));
        curIntensity = Mathf.Min(maxIntensity,curIntensity);
    }

    public void AddIntensity(float by){
        by *= hitIntensityScale;
        curIntensity += by * maxIntensity;
    }
}
