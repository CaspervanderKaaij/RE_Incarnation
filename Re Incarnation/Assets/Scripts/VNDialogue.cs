using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class VNDialogue : MonoBehaviour
{

    public VNDiaParams[] dia;
    Text txt;
    int curText = 0;
    [SerializeField] UnityEvent baseEvent;
    [SerializeField] UnityEvent endEvent;

    void Start()
    {
        txt = GetComponent<Text>();
        curText--;
        txt.text = "";
        NextLine();
    }

    void Update()
    {
        if (IsInvoking("WaitFunction") == false)
        {
            SetTextLetters();
        }
    }

    public void NextLine()
    {
        if (curText < dia.Length - 1)
        {
            if (IsInvoking("WaitFunction") == false)
            {
                curText++;
                txt.text = "";
                Invoke("WaitFunction", dia[curText].waitTime);

                float totalTime = 0;
                for (int i = 0; i < dia[curText].timedEvents.Length; i++)
                {
                    totalTime += dia[curText].timedEvents[i].nextEvent;
                    StartCoroutine(TimedEvents(dia[curText].timedEvents[i].curEvent, totalTime));
                }
            }
        }
        else
        {
            endEvent.Invoke();
        }
    }

    IEnumerator TimedEvents(UnityEvent ev, float time)
    {
        yield return new WaitForSeconds(time);
        ev.Invoke();
    }

    void SetTextLetters()
    {
        txt.text = dia[curText].dia;
    }

    void WaitFunction()
    {
        dia[curText].ev.Invoke();
        baseEvent.Invoke();
    }
}

[System.Serializable]
public class VNDiaParams
{
    [Header("---New Line---")]
    public UnityEvent ev;
    public EventArray[] timedEvents;
    public float waitTime = 0;
     [Header("---End line---")]
    [TextArea] public string dia;
}
