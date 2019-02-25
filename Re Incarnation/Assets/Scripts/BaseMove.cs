using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseMove : MonoBehaviour
{

    [SerializeField] string reminderInputName = "Set name of move here";
    [Header("BaseMove")]
    [SerializeField] float speed = 30;
    [HideInInspector] public CharacterController cc;
    [HideInInspector] public Vector3 moveV3 = Vector3.zero;
    [SerializeField] float moveTime = 0.3f;
    [SerializeField] string animationName = "Roll";
    [SerializeField] UnityEvent endEvent;
    [Header("Set which inputs to check for during move here")]
    [SerializeField] InputEvent[] inputs;
    [SerializeField] float inputIgnoreTime = 0.3f;
    [Header("---Spawn Particles Here---")]
    [SerializeField] UnityEvent startEvent;
    [SerializeField] EventArray[] timedEvents;
    [SerializeField] bool canInterupt = false;
    public virtual void Move()
    {
        moveV3.x = transform.TransformDirection(0, 0, 1).x * speed;
        moveV3.z = transform.TransformDirection(0, 0, 1).z * speed;
    }

    void Update()
    {
        if (IsInvoking("StopMove") == true)
        {
            Move();
            FinalMove();
        }
        SetInputs();
    }

    public void SetInputs()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].CheckInput();
        }
    }

    public void FinalMove()
    {
        if (cc == null)
        {
            cc = transform.root.GetComponent<CharacterController>();
        }
        cc.Move(moveV3 * Time.deltaTime);
    }

    public virtual void StartMove()
    {
        bool doStuff = false;
        if (canInterupt == false)
        {
            if (IsInvoking("StopMove") == false && IsInvoking("IgnoreInput") == false)
            {
                doStuff = true;
            }
        }
        else if (IsInvoking("IgnoreInput") == false)
        {
            doStuff = true;
        }

        if (doStuff == true)
        {
            CancelInvoke("StopMove");
            CancelInvoke("IgnoreInput");
            Invoke("StopMove", moveTime);
            Invoke("IgnoreInput", inputIgnoreTime);
            cc = transform.root.GetComponent<CharacterController>();
            BaseMove[] moves = transform.root.GetComponentsInChildren<BaseMove>();
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] != this)
                {
                    moves[i].enabled = false;
                }
                else
                {
                    moves[i].enabled = true;
                }
            }
            transform.root.GetComponentInChildren<Animator>().PlayInFixedTime(animationName, 0, 0);
            startEvent.Invoke();
            float totalTime = 0;
            for (int i = 0; i < timedEvents.Length; i++)
            {
                totalTime += timedEvents[i].nextEvent;
                StartCoroutine(TimedEvents(timedEvents[i].curEvent, totalTime));
            }

        }
    }

    IEnumerator TimedEvents(UnityEvent ev, float time)
    {
        yield return new WaitForSeconds(time);
        ev.Invoke();
    }

    void StopMove()
    {
        endEvent.Invoke();
    }

    void IgnoreInput()
    {
        //empry because of invoking. I use it to check if its invoking, if so, ignore input
    }
}

[System.Serializable]
public class EventArray
{
    public float nextEvent = 1;
    public UnityEvent curEvent;
}
