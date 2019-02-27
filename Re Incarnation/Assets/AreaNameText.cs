using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaNameText : MonoBehaviour
{

    Vector3 scaleGoal = Vector3.zero;
    float speed = 0;
    Vector3 startScale;
    Text txt;
    float alphaGoal = 1;
    float alphaSpeed = 5;
    [SerializeField] bool disabled = false;

    void Start()
    {
        txt = GetComponent<Text>();
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        if (disabled == false)
        {
            StartCoroutine(Event());
        }
        else if (Application.isEditor == false)
        {
            StartCoroutine(Event());
        }
    }

    IEnumerator Event()
    {
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1);
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        alphaGoal = 1;
        alphaSpeed = 3;
        InvokeRepeating("LerpColor", 0, Time.deltaTime);
        scaleGoal = startScale;
        transform.localScale = startScale / 1.5f;
        speed = 0.25f;
        InvokeRepeating("LerpScale", 0, Time.deltaTime);
        yield return new WaitForSeconds(3);
        alphaGoal = 0;
        alphaSpeed = 10;
        yield return new WaitForSeconds(1);
        scaleGoal = Vector3.zero;
        transform.localScale = Vector3.zero;
        speed = 0;
        yield return new WaitForSeconds(1);
        CancelInvoke("LerpScale");
        CancelInvoke("LerpColor");

    }

    void LerpScale()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, scaleGoal, Time.deltaTime * speed);
    }

    void LerpColor()
    {
        txt.color = Color.Lerp(txt.color, new Color(txt.color.r, txt.color.g, txt.color.b, alphaGoal), Time.deltaTime * alphaSpeed);
    }
}
