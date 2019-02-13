using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPSlide : MonoBehaviour
{

    List<Transform> children = new List<Transform>();
    public int curSelected = 0;
    [SerializeField] Vector3 selectedScale = Vector3.one * 2;
    [SerializeField] Vector3 otherScale = Vector3.one / 2;
	[SerializeField] float scaleSpeed = 10;

    void Start()
    {
        GetChildren();
    }

    void Update()
    {
        ScaleSelected();
        ChangeSelected();
    }

    void ChangeSelected()
    {
        if (Input.GetButtonDown("Horizontal") == true)
        {
            curSelected += (int)Input.GetAxis("Horizontal");
			if(curSelected > children.Count - 1){
				curSelected = 0;
			}
			if(curSelected < 0){
				curSelected = children.Count - 1;
			}
        }
    }

    void ScaleSelected()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (i == curSelected)
            {
                children[i].localScale = Vector3.MoveTowards(children[i].localScale, selectedScale, Time.deltaTime * scaleSpeed);
            }
            else
            {
                children[i].localScale = Vector3.MoveTowards(children[i].localScale, otherScale, Time.deltaTime * scaleSpeed);
            }
        }
    }

    void GetChildren()
    {
        children.Clear();
        children.AddRange(transform.GetComponentsInChildren<Transform>());
		children.Remove(transform);
		for (int i = 0; i < children.Count; i++)
		{
			if(children[i].gameObject.layer == 2){
				children.Remove(children[i]);
				i--;
			}
		}
    }
}
