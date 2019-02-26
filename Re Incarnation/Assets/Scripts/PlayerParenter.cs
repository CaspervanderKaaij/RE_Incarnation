using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParenter : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), -Vector3.up * 0.3f, Color.red, Time.deltaTime);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), -Vector3.up, out hit, 0.3f))
        {
            if (hit.transform.tag == "PlayerParent")
            {
                transform.SetParent(hit.transform.parent);
            }
            else
            {
                transform.SetParent(null);
            }
        }
        else
        {
            transform.SetParent(null);
        }
    }
}
