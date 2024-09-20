using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CreditsEnd : MonoBehaviour
    {
        public void LoadNewScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title Screen");
        }
    }
}
