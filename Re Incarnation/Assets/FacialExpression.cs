using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FacialExpression : MonoBehaviour
{
    Image img;
    [SerializeField] Sprite[] sprites;

    public void SetExpression(int ex)
    {
        if (img == null)
        {
            img = GetComponent<Image>();
        }
        img.sprite = sprites[ex];
    }
}
