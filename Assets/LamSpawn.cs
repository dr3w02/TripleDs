using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class LamSpawn : MonoBehaviour
    {

        // List to store GameObjects
        public List<GameObject> gameObjects;


        void Start()
        {
       
            ActivateRandomObject();

        }

        public void ActivateRandomObject()
        {
         
            if (gameObjects.Count == 0)
            {
                Debug.LogError("No GameObjects in list");
                return;
            }

         
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }

            int randomIndex = Random.Range(0, gameObjects.Count);

     
            gameObjects[randomIndex].SetActive(true);


            gameObjects.RemoveAt(randomIndex);

            if (gameObjects.Count == 0)
            {
                Debug.Log("Winner!");
            }

        }
    }
}
