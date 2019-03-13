using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BunnyChild : MonoBehaviour
{

    [SerializeField] Transform target;
    float speed = 10;
    [SerializeField] float walkSpeed = 4;
    [SerializeField] float canSeeDistance = 30;
    [SerializeField] float stopDistance = 2;
    Animator animator;
    AudioSource source;
    float baseAnimSpeed = 0;
    bool isWalking = true;
    [SerializeField] float waitTime = 0.5f;
    [SerializeField] Vector3 latePos;
    [SerializeField] float happyParticleTime;
    [SerializeField] GameObject particleObject;


    void Start()
    {
        target = FindObjectOfType<PlayerParenter>().transform;
        source = GetComponentInChildren<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        baseAnimSpeed = animator.speed;
        latePos = target.position;
    }

    void Update()
    {
        latePos = Vector3.Lerp(latePos, target.position, Time.deltaTime);
        transform.LookAt(new Vector3(latePos.x, transform.position.y, latePos.z));
        transform.position += transform.forward * Time.deltaTime * speed;

        if (Vector3.Distance(transform.position, target.position) > canSeeDistance)
        {
            isWalking = false;
            CancelInvoke("StartWalking");
        }
        if (Vector3.Distance(transform.position, target.position) < canSeeDistance)
        {
            if (isWalking == false && IsInvoking("StartWalking") == false)
            {
                CancelInvoke("StartWalking");
                Invoke("StartWalking", waitTime);
            }
        }
        if (Vector3.Distance(transform.position, target.position) < stopDistance)
        {
            isWalking = false;
            CancelInvoke("StartWalking");
            Invoke("WaitForEvent", happyParticleTime);
        }
        else
        {
            particleObject.SetActive(false);
            CancelInvoke("WaitForEvent");
        }

        if (isWalking == true)
        {
            Walk();
        }
        else
        {
            Idle();
        }
        animator.speed = (speed / walkSpeed) * baseAnimSpeed;
    }

    void Walk()
    {
        speed = walkSpeed;
        if (source.isPlaying == false)
        {
            source.Play();
        }
    }

    void Idle()
    {
        speed = 0;
        source.Stop();
        animator.PlayInFixedTime("ChildWalk", 0, 0);
    }

    void StartWalking()
    {
        isWalking = true;
    }

    void WaitForEvent()
    {
        particleObject.SetActive(true);
    }
}
