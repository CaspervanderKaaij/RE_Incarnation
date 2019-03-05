using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public void LoadLevelString(string str){
        SceneManager.LoadScene(str);
    }

    public void LoadLevelInt(int integer){
        SceneManager.LoadScene(integer);
    }
}
