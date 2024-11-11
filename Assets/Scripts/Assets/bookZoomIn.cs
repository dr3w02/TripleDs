using Cinemachine;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class bookZoomIn : MonoBehaviour, IInteractable
{
    [SerializeField] RBController characterMove;
    public string InteractionPrompt => "LookAtBook";

    public GameObject InteractionImagePrompt => BookPromptCanvas;

    //public GameObject character;
    public GameObject BookCam;
    public GameObject BookPromptCanvas;
    public GameObject Player;
    public RBController playerScript;
    public bool isZoomedIn;
 

    private void OnValidate()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            gameObject.AddComponent<BoxCollider>();
        }

    }
    
    Respawn respawn;

    private void Start()
    {
        BookCam.SetActive(false);
    }

    public bool Interact(Interactor interactor, GameObject player)
    {
        if(respawn == null)
            respawn = player.GetComponent<Respawn>();

        if (Player == null)
            Player = player;

        isZoomedIn = !isZoomedIn;
        

        if (!isZoomedIn)
        {
            
            characterMove.TurnOffMovement();
            ZoomInOnBook();
           
         
            return true;
        }
        else        {
            characterMove.Enabled();
            ZoomOut();
            return true;

        }
    
    }


    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);

        Player.SetActive(false);
    }

    private void ZoomInOnBook()
    {
        //isZoomedIn = true;
        BookCam.SetActive(true);
       
        
        StartCoroutine(wait());
        //change this to can move is false

    }

    private void ZoomOut()
    {
      
        BookCam.SetActive(false);
        Player.SetActive(true);
       
    }

    public void Update()
    {
        if (respawn != null)
        {
            if (respawn.resetbook)
            {
                characterMove.Enabled();
                ZoomOut();
                respawn.resetbook = false;
            }
        }
      

    }
}

