using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

List<GameObject> players = new List<GameObject>();
[SerializeField] Vector3 offset;
[SerializeField] float speed = 10;
	void Start () {
		
	}
	
	void Update () {
		GetPlayers();
		transform.position = Vector3.Lerp(transform.position,GetCenterOfObjects(players.ToArray()) + offset,Time.deltaTime * speed);
	}

	void GetPlayers(){
		players.Clear();
		players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
	}

	Vector3 GetCenterOfObjects(GameObject[] objects){
		Vector3 toReturn = Vector3.zero;
		if(objects.Length == 0){
			return toReturn;
		}
		for (int i = 0; i < objects.Length; i++)
		{
			toReturn += objects[i].transform.position;
		}
		toReturn /= objects.Length;
		return toReturn;
	}
}
