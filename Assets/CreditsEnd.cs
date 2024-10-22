using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CreditsEnd : MonoBehaviour
    {
        public void Update()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;
        }
        public void LoadNewScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title Screen");
        }
    }
}
