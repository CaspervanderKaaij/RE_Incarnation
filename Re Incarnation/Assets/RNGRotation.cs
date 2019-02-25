using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGRotation : MonoBehaviour
{

    [SerializeField] Vector3[] rots;
    int curRot = 0;
    [SerializeField] float repeatTime = 0.4f;

    void Start()
    {
        InvokeRepeating("SetRotation", repeatTime, repeatTime);
    }


    void SetRotation()
    {
		//it chooses a new rotation, could have been better but eh.
        List<Vector3> rotsL = new List<Vector3>();
        rotsL.AddRange(rots);
        int chosenInt = Random.Range(0, rotsL.Count - 1);
        Vector3 oldAngle = transform.eulerAngles;
        transform.eulerAngles = rotsL[chosenInt];
        curRot = chosenInt;
        if (transform.eulerAngles == oldAngle)
        {
            if (rotsL[chosenInt + 1] != null)
            {
                transform.eulerAngles = rotsL[chosenInt + 1];
                curRot = chosenInt + 1;
            }
            else
            {
                transform.eulerAngles = rotsL[0];
                curRot = 0;
            }
        }
    }
}
