using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWalking : BaseMove
{

    [HideInInspector] public Vector3 rotGoal;
    [Header("NormalWalking")]
    [SerializeField] float rotSpeed = 10;
    [SerializeField] float walkingSpeed = 10;
    [SerializeField] float accelerationSpeed = 3;
    [SerializeField] float decelerationSpeed = 4;
    [HideInInspector] public float currentAccDec = 0;
    [SerializeField] Transform angleInputDifference;
    float currentAngleAcceleration = 0;
    Vector2 lastXZPos = Vector2.zero;//this is used to check if the walk animation should play.
    public string horInputName = "Horizontal_P1";
    public string vertInputName = "Vertical_P1";
    Animator anim;
    [SerializeField] float gravityStrength = -9.81f;
    [SerializeField] float gravityPullSpeed = 10;
    bool rotateHelp = false;
    Transform lastParent;

    void Start()
    {
        anim = cc.GetComponentInChildren<Animator>();
        InvokeRepeating("FallAnimChecker", 0, 0.1f);
    }
    void Update()
    {

        SetRotation();
        MoveForward();
        lastXZPos = new Vector2(transform.position.x, transform.position.z);//used to check if walking animation should play.
        FinalMove();
        SetAnimWalkIdle(anim, Vector2.Distance(lastXZPos, new Vector2(transform.position.x, transform.position.z)));
        Gravity();
        SetInputs();
    }

    public void ResetAcceleration()
    {
        currentAccDec = 0;
        moveV3 = Vector3.zero;
        currentAngleAcceleration = 0;
        anim.SetInteger("NextMove", 0);
    }

    void SetRotation()
    {
        Vector2 inputV2 = new Vector2(Input.GetAxis(vertInputName), Input.GetAxis(horInputName));
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


        //used for when parented, ignore moving toward rotation, until you update it with an input, otherwise the player would look to the wrong direction after getting parented.
        if (rotateHelp == true && Vector2.SqrMagnitude(inputV2) != 0)
        {
            rotateHelp = false;
        }
        if (cc.transform.parent != lastParent && Vector2.SqrMagnitude(inputV2) == 0)
        {
            rotateHelp = true;
        }
        lastParent = cc.transform.parent;
        if (rotateHelp == false)
        {
            cc.transform.localRotation = Quaternion.Lerp(cc.transform.localRotation, Quaternion.Euler(rotGoal), Time.deltaTime * rotSpeed);
            angleInputDifference.rotation = Quaternion.Euler(rotGoal + new Vector3(0, 180, 0));
        }
    }

    void MoveForward()
    {
        Vector2 inputV2 = new Vector2(Input.GetAxis(horInputName), Input.GetAxis(vertInputName));
        float sqrInputV2 = Vector2.SqrMagnitude(inputV2);

        //acceleration and deceleration stuff
        if (sqrInputV2 != 0)
        {
            currentAccDec = Mathf.Lerp(currentAccDec, 1, Time.deltaTime * accelerationSpeed);
        }
        else
        {
            currentAccDec = Mathf.Lerp(currentAccDec, 0, Time.deltaTime * decelerationSpeed);
        }

        //Makes you go slower when turning around

        float floatDifferenceAngle = Quaternion.Angle(cc.transform.localRotation, angleInputDifference.rotation) / 180;
        if (rotateHelp == true)
        {
            floatDifferenceAngle = 1;//ignore when just parented, and no input pressed yet
        }
        if (floatDifferenceAngle < 0.5f)
        {
            //  floatDifferenceAngle = 0;
            currentAccDec *= floatDifferenceAngle;
            currentAccDec = Mathf.Max(0.2f, currentAccDec);
        }

        moveV3.x = transform.TransformDirection(0, 0, currentAccDec).x * walkingSpeed;
        moveV3.z = transform.TransformDirection(0, 0, currentAccDec).z * walkingSpeed;

    }

    void Gravity()
    {
        moveV3.y = Mathf.MoveTowards(moveV3.y, gravityStrength, Time.deltaTime * gravityPullSpeed);
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, LayerMask.GetMask("Default")) == false && cc.isGrounded == false)
        {
            if (anim.GetInteger("NextMove") != 3 && IsInvoking("FallAnim") == false)
            {
                Invoke("FallAnim", 0.05f);
            }
            if (IsInvoking("FallAnim") == true && anim.GetCurrentAnimatorStateInfo(0).IsTag("Roll") == true)
            {
                moveV3.y = gravityStrength / 3;
                if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Roll") == true)
                {
                    CancelInvoke("FallAnim");
                }
            }
        }
        else
        {
            CancelInvoke("FallAnim");
        }
    }

    void FallAnim()
    {
        if (this.enabled == true)
        {
            anim.SetInteger("NextMove", 3);
            anim.Play("Fall", 0);
        }
    }

    void FallAnimChecker()
    {
        if (this.enabled == false)
        {
            anim.SetInteger("NextMove", 0);
            CancelInvoke("FallAnim");
        }
    }

    void SetAnimWalkIdle(Animator anim, float walkSpeed)
    {
        //function primaily used for walk, idle and fall animation
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, LayerMask.GetMask("Default")) == true || cc.isGrounded == true)
        {
            anim.SetInteger("NextMove", 0);
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle") == true)
            {
                if (walkSpeed > 0.05f)
                {
                    anim.SetInteger("NextMove", 1);
                }
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Walk") == true)
            {
                if (walkSpeed < 0.05f)
                {
                    anim.SetInteger("NextMove", 1);
                }
            }
        }
    }
}
