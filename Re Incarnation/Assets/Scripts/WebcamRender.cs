using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamRender : MonoBehaviour
{

    WebCamDevice camDevice;
    Renderer rend;
    public Texture test;
    WebCamTexture tex;
	RenderTexture rendTex;

    void Start()
    {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
		tex = new WebCamTexture(2000,2000,60);
		Invoke("GoGetThem",1);
    }

	void GoGetThem(){
        WebCamTexture webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
	//	rendTex = webcamTexture;
		// renderer.material.SetTexture("_Metallic",webcamTexture);
        webcamTexture.Play();

	}

    void Update()
    {
        //	rend.material.mainTexture = WebCamTexture();
    }
}
