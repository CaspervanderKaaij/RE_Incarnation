using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseMove : MonoBehaviour
{

    [Header("BaseMove")]
    [SerializeField] float speed = 30;
    [HideInInspector] public CharacterController cc;
    [HideInInspector] public Vector3 moveV3 = Vector3.zero;
    [SerializeField] float moveTime = 0.3f;
    [SerializeField] string animationName = "Roll";
    [SerializeField] UnityEvent endEvent;
    [SerializeField] InputEvent[] inputs;
    [SerializeField] float inputIgnoreTime = 0.3f;
    [SerializeField] UnityEvent startEvent;
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
            cc = GetComponent<CharacterController>();
        }
        cc.Move(moveV3 * Time.deltaTime);
    }

    public virtual void StartMove()
    {
        if (IsInvoking("StopMove") == false && IsInvoking("IgnoreInput") == false)
        {
            Invoke("StopMove", moveTime);
            cc = GetComponent<CharacterController>();
            BaseMove[] moves = GetComponents<BaseMove>();
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
            GetComponentInChildren<Animator>().Play(animationName);
            startEvent.Invoke();
        }
    }

    void StopMove()
    {
        endEvent.Invoke();
        Invoke("IgnoreInput",inputIgnoreTime);
    }

    void IgnoreInput(){
        //empry because of invoking. I use it to check if its invoking, if so, ignore input
    }
}
