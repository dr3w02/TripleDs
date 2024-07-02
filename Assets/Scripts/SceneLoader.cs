using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public List<string> sceneNames = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        LoadScenes();
    }

    private void LoadScenes()
    {
        foreach (string sceneName in sceneNames)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
