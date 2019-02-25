using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{

    public List<TimeInput> input = new List<TimeInput>();
    public bool paused = false;
    void Start()
    {

    }

    void Update()
    {
        SetTimeScale();
        SetPaused();
    }

    void SetTimeScale()
    {
        if (input.Count > 0)
        {
            int highestPriority = 0;
            for (int i = 1; i < input.Count; i++)
            {
                if (input[highestPriority].priority < input[i].priority)
                {
                    highestPriority = i;
                }
            }
            Time.timeScale = input[highestPriority].newTime;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void SetPaused()
    {
        bool canPause = true;
        for (int i = 0; i < input.Count; i++)
        {
            if (input[i].canPause == false)
            {
                canPause = false;
            }
        }
        if (canPause == true && paused == true)
        {
            Time.timeScale = 0;
        }
    }

    public void NewInput(float newTime, bool canPause, float priority)
    {
        TimeInput toAdd = new TimeInput();
        toAdd.canPause = canPause;
        toAdd.newTime = newTime;
        toAdd.priority = priority;
        input.Add(toAdd);
    }
    public void NewInput(float newTime, bool canPause, float priority, float time)
    {
        TimeInput toAdd = new TimeInput();
        toAdd.canPause = canPause;
        toAdd.newTime = newTime;
        toAdd.priority = priority;
        toAdd.id = Random.Range(0, 6666666);
        for (int i = 0; i < input.Count - 1; i++)
        {
            if (input[i].id == toAdd.id)
            {
                i = 0;
                toAdd.id++;
            }
        }
        input.Add(toAdd);
        StartCoroutine(TimeBackScaled(time, toAdd.id));
    }

    IEnumerator TimeBackScaled(float time, int id)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < input.Count - 1; i++)
        {
            if (input[i].id == id)
            {
                input.Remove(input[i]);
                i = input.Count;
            }
        }
    }
    public void NewInput(float newTime, float priority, float timeUnscaled, bool canPause)
    {
        TimeInput toAdd = new TimeInput();
        toAdd.canPause = canPause;
        toAdd.newTime = newTime;
        toAdd.priority = priority;
        toAdd.id = Random.Range(0, 6666666);
        for (int i = 0; i < input.Count - 1; i++)
        {
            if (input[i].id == toAdd.id)
            {
                i = 0;
                toAdd.id++;
            }
        }
        input.Add(toAdd);
        StartCoroutine(TimeBackUnscaled(timeUnscaled, toAdd.id));
    }
    IEnumerator TimeBackUnscaled(float time, int id)
    {
        yield return new WaitForSecondsRealtime(time);
        for (int i = 0; i < input.Count; i++)
        {
            if (input[i].id == id)
            {
                input.Remove(input[i]);
                i = input.Count;
            }
        }
    }

    public void BigSlowMo(bool canPause)
    {
        NewInput(0.1f, 0.5f, 0.4f, canPause);
    }
    public void MediumSlowMo(bool canPause)
    {
        NewInput(0.15f, 0.4f, 0.3f, canPause);
    }
    public void SmallSlowMo(bool canPause)
    {
        NewInput(0.25f, 0.3f, 0.01f, canPause);
    }
}

[System.Serializable]
public class TimeInput
{
    public float newTime = 1;
    public bool canPause = true;
    public float priority = 1;
    public int id = 0;
}
