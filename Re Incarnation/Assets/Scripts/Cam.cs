using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
	[Header("Camera")]
    [SerializeField] Vector3 offset;
    [SerializeField] float speed = 10;
    Camera cam;
	[Header("Field Of View")]
	[SerializeField] float minFOV = 20;
	[SerializeField] float maxFOV = 70;
	[SerializeField] float fovDistanceMuliplier = 5;
	[SerializeField] float fovLerpSpeed = 3;
    void Start()
    {
        cam = GetComponent<Camera>();
        transform.position = GetCenterOfObjects(players.ToArray()) + offset;
    }

    void Update()
    {
        GetPlayers();
        transform.position = Vector3.Lerp(transform.position, GetCenterOfObjects(players.ToArray()) + offset, Time.deltaTime * speed);
        SetFOV();
    }

    void GetPlayers()
    {
        players.Clear();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    Vector3 GetCenterOfObjects(GameObject[] objects)
    {
        Vector3 toReturn = Vector3.zero;
        if (objects.Length == 0)
        {
            return toReturn;
        }
        for (int i = 0; i < objects.Length; i++)
        {
            toReturn += objects[i].transform.position;
        }
        toReturn /= objects.Length;
        return toReturn;
    }

    void SetFOV()
    {
        float distance = 0;
        for (int i2 = 0; i2 < players.Count; i2++)
        {
            for (int i = 1; i < players.Count; i++)
            {
                distance = Mathf.Max(distance, Vector3.Distance(players[i - 1].transform.position, players[i2].transform.position));
            }
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Mathf.Min(maxFOV, Mathf.Max(minFOV, distance * fovDistanceMuliplier)), Time.deltaTime * fovLerpSpeed);
    }
}
