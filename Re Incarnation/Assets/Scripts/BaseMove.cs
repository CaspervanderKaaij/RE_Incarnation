using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseMove : MonoBehaviour
{

    [SerializeField] string reminderInputName = "Set name of move here";
    [Header("BaseMove")]
    [SerializeField] float speed = 30;
    public CharacterController cc;
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
    [SerializeField] bool mustBeGrounded = true;
    [SerializeField] bool disableWhenNotActive = true;
    [SerializeField] float rotateSpeed = 0;
    NormalWalking normalWalking;
    public virtual void Move()
    {
        moveV3.x = transform.TransformDirection(0, 0, 1).x * speed;
        moveV3.z = transform.TransformDirection(0, 0, 1).z * speed;

        Vector2 inputV2 = new Vector2(Input.GetAxis(normalWalking.vertInputName), Input.GetAxis(normalWalking.horInputName));
        Vector3 rotGoal = cc.transform.localRotation.eulerAngles;
        // print(cc.transform.parent.localEulerAngles.y);
        if (Vector2.SqrMagnitude(inputV2) != 0)
        {
            if (cc.transform.parent != null)
            {
                rotGoal = new Vector3(0, cc.transform.parent.InverseTransformDirection(0, Mathf.Atan2(inputV2.y, inputV2.x) * Mathf.Rad2Deg, 0).y - cc.transform.parent.eulerAngles.y, 0);
            }
            else
            {
                rotGoal = new Vector3(0, Mathf.Atan2(inputV2.y, inputV2.x) * Mathf.Rad2Deg, 0);
            }
            rotGoal.y += Camera.main.transform.eulerAngles.y;
        }
        normalWalking.rotGoal = rotGoal;
        cc.transform.localRotation = Quaternion.Lerp(cc.transform.localRotation, Quaternion.Euler(rotGoal), Time.deltaTime * rotateSpeed);
    }

    public void SetRotSpeed(float newSpeed)
    {
        rotateSpeed = newSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void LateUpdate()
    {
        if (IsInvoking("StopMove") == true)
        {
            Move();
            FinalMove();
            //  print(transform.name);
        }
        else if (disableWhenNotActive == true)
        {
            this.enabled = false;
        }
        if (IsInvoking("IgnoreInput") == false)
        {
            SetInputs();
        }
    }

    public void SetInputs()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].gameObject.activeSelf == true)
            {
                inputs[i].CheckInput();
            }
        }
    }

    public void FinalMove()
    {
        cc.Move(moveV3 * Time.deltaTime);
    }

    public virtual void StartMove()
    {
        normalWalking = cc.gameObject.GetComponentInChildren<NormalWalking>();
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

        if (mustBeGrounded == true && cc.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Fall") == true)
        {
            doStuff = false;
        }

        if (doStuff == true)
        {
            CancelInvoke("StopMove");
            CancelInvoke("IgnoreInput");
            Invoke("StopMove", moveTime);
            Invoke("IgnoreInput", inputIgnoreTime);
            // cc = transform.root.GetComponent<CharacterController>();
            BaseMove[] moves = cc.GetComponentsInChildren<BaseMove>();
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] != this)
                {
                    if (moves[i].enabled == true)
                    {
                        moves[i].endEvent.Invoke();
                    }
                    moves[i].enabled = false;
                }
                else
                {
                    moves[i].enabled = true;
                }
            }
            cc.GetComponentInChildren<Animator>().PlayInFixedTime(animationName, 0, 0);
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
        if (this.enabled == true)
        {
            endEvent.Invoke();
        }
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
