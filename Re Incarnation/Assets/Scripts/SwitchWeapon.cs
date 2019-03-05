using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchWeapon : MonoBehaviour
{

    [SerializeField] GameObject[] weapons;
    int curWeapon = 0;
    [SerializeField] string inputName = "Attack_P1";
    [SerializeField] string[] uiNames;
    [SerializeField] Text uiTxt;

    void Start()
    {
        Active();
        SetUI();
    }

    void Update()
    {
        if (Input.GetAxis(inputName) != 0)
        {
            Switch((int)Input.GetAxis(inputName));
            Active();
            SetUI();
        }
    }

    void Switch(int by)
    {
        curWeapon += by;
        if (curWeapon < 0)
        {
            curWeapon += weapons.Length;
        }
        if (curWeapon > weapons.Length - 1)
        {
            curWeapon -= weapons.Length;
        }
    }

    void Active()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[curWeapon].SetActive(true);
    }

    void SetUI(){
        uiTxt.text = uiNames[curWeapon];
    }
}
