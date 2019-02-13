using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : BaseMove
{

    [SerializeField] float speed = 30;

    void DuringMove()
    {
        // base.DuringMove();
        Move();
        FinalMove();
    }

    void Move()
    {
        moveV3.x = transform.TransformDirection(0, 0, 1).x * speed;
        moveV3.z = transform.TransformDirection(0, 0, 1).z * speed;
    }

    void Update()
    {
        DuringMove();
        // if (IsInvoking("StopRoll") == true)
        // {
        // }
    }

    public void StartRoll()
    {
        if (IsInvoking("StopRoll") == false)
        {
            Invoke("StopRoll", 1);
         //   cc = GetComponent<CharacterController>();
          //  GetComponent<NormalWalking>().ResetAcceleration();
          //  GetComponent<NormalWalking>().enabled = false;
        }
    }

    void StopRoll()
    {
        GetComponent<NormalWalking>().enabled = true;
    }
}
