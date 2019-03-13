using UnityEngine;
using System.Collections;

public class MaterialMove : MonoBehaviour
{
	public float SpeedX = 0.5f;
	public float SpeedY = 0.5f;
	public float Loop = 0f;
	public Renderer render;

	void Start ()
	{
		render = GetComponent<Renderer> ();
	}

	void Update ()
	{
		render.material.mainTextureOffset = new Vector2 (Time.time * SpeedX, Time.time * SpeedY);
		if (Loop != 0f) {
			if (render.material.mainTextureOffset.x >= Loop) {
				render.material.mainTextureOffset = new Vector2 (0f, render.material.mainTextureOffset.y);
			} 
			if (render.material.mainTextureOffset.y >= Loop) {
				render.material.mainTextureOffset = new Vector2 (render.material.mainTextureOffset.x, 0f);
			}
		}
	}
}
