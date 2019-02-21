using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpawner : MonoBehaviour
{

    [Header("VFX")]
    public Effect[] effects;

    public void SpawnVFX()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].localRot == false)
            {
                Instantiate(effects[i].particle, transform.position + effects[i].posOffset, Quaternion.Euler(effects[i].angle));
            }
            else
            {
                GameObject t = Instantiate(effects[i].particle, transform.position + effects[i].posOffset, transform.rotation);
                t.transform.Rotate(effects[i].angle);
            }
        }
    }
}

[System.Serializable]
public class Effect
{
    public GameObject particle;
    public Vector3 angle;
    public bool localRot;
	public Vector3 posOffset;
}
