using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public enum State
    {
        Normal,
        AtTrans
    }
    [SerializeField] State curState = State.Normal;
    List<GameObject> players = new List<GameObject>();
    [Header("Normal State")]
    [Space]
    [Header("   Camera")]
    [SerializeField] Vector3 offset;
    [SerializeField] float speed = 10;
    Camera cam;
    [Header("   Field Of View")]
    [SerializeField] float minFOV = 20;
    [SerializeField] float maxFOV = 70;
    [SerializeField] float fovDistanceMuliplier = 5;
    [SerializeField] float fovLerpSpeed = 3;
    [Header("At Transform")]
    [SerializeField] Transform pos;
    [SerializeField] float toPosSpeed = 2;
    [SerializeField] float toPosRotSpeed = 2;
    void Start()
    {
        cam = GetComponent<Camera>();
        transform.position = GetCenterOfObjects(players.ToArray()) + offset;
    }

    void Update()
    {
        switch (curState)
        {
            case State.Normal:
                GetPlayers();
                transform.position = Vector3.Lerp(transform.position, GetCenterOfObjects(players.ToArray()) + offset, Time.deltaTime * speed);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(25.347f, 0, 0), Time.deltaTime * toPosRotSpeed);
                SetFOV();
                break;
            case State.AtTrans:
                MoveTowardsTrans();
                break;
        }
    }

    void MoveTowardsTrans()
    {
        transform.position = Vector3.Lerp(transform.position, pos.position, Time.deltaTime * toPosSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, pos.rotation, Time.deltaTime * toPosRotSpeed);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Time.deltaTime * 10);
    }

    public void SetTransMoveSpeed(float newSpeed)
    {
        toPosSpeed = newSpeed;
    }

    public void SetTransRotSpeed(float newSpeed)
    {
        toPosRotSpeed = newSpeed;
    }

    public void ChangeState(int newState)
    {
        switch (newState)
        {
            case 0:
                curState = State.Normal;
                break;
            case 1:
                curState = State.AtTrans;
                break;
        }
    }

    public void SetLookTransform(Transform newTrans)
    {
        pos = newTrans;
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
