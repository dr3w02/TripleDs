using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class MainMenu : MonoBehaviour{

        public void Update()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;

        }
        public void PlayGame ()
        {
            SceneManager.LoadScene(1);
            Debug.Log("Start");
        }

        public void QuitGame ()
        {
            Debug.Log ("QUIT!");
            Application.Quit();
        }
    }
}
