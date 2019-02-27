using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeImg : MonoBehaviour
{
    [SerializeField] Color goalColor;
    [SerializeField] Color startColor;
    Image image;
    bool scaledTime = false;
    [SerializeField] float speed;
    bool started = false;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = startColor;
        started = false;
    }

    void Update()
    {
        if (started == false)
        {
            if (scaledTime == true)
            {
                image.color = Color.Lerp(image.color, goalColor, Time.deltaTime * speed);
            }
            else
            {
                image.color = Color.Lerp(image.color, goalColor, Time.unscaledDeltaTime * speed);
            }
        }
        else
        {
            started = true;
        }
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void ChangeGoalColor(Color newGoal)
    {
        goalColor = newGoal;
    }
    public void ChangeGoalAlpha(float value)
    {
        ChangeGoalColor(new Color(image.color.r, image.color.g, image.color.b, value));
    }
}
