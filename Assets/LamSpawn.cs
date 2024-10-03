using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class LamSpawn : MonoBehaviour
    {


       
        public List<GameObject> objectsToSpawn;
        List<GameObject> itemLocations;


        void Start()
        {
            generateNewObj();
        }
       

        public void generateNewObj()
        {
            for (int i = itemLocations.Count; i > 0; i--)
            {
                GameObject newObject = Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Count)], itemLocations[i].transform.position, Quaternion.identity);

                Destroy(itemLocations[i]);
                newObject.SetActive(true);
            }
        }
    }
}
