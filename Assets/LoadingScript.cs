using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class LoadingScript : MonoBehaviour
    {
        [Header("menu Screens")]
        [SerializeField] private GameObject LoadingScreen;
        [SerializeField] private GameObject MainMenuCanvas;
       
        public void LoadLevelBtn(string LevelToLoad)
        {
            MainMenuCanvas.SetActive(false);
            LoadingScreen.SetActive(true);

            StartCoroutine(LoadLevelASync(LevelToLoad));
        }

        public IEnumerator LoadLevelASync(string LevelToLoad)
        {
            AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(LevelToLoad);
            while (!LoadOperation.isDone)
            {
                float progressiveValue = Mathf.Clamp01(LoadOperation.progress / 0.9f);
                //LoadingSlider.value = progressiveValue;
                yield return null;
            }
        }
    }
}
