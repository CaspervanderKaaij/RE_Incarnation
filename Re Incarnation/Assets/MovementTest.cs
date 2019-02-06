using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour {

CharacterController cc;
[SerializeField] float speed = 10;

	void Start () {
		cc = GetComponent<CharacterController>();
	}
	
	void Update () {
		Vector3 toMove = Vector3.zero;
		toMove.x = Input.GetAxis("Horizontal");
		toMove.z = Input.GetAxis("Vertical");
		cc.Move(toMove * speed * Time.deltaTime);
	}
}
