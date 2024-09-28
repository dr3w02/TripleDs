using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;


public class ClemFirstLook : MonoBehaviour
{

    public AudioSource Grone;
    public GameObject Clem;
    public GameObject DoorClose;
    [SerializeField] private Animator myAnimationController;

    public Transform A;
    public Transform B;

    public float speed;
    public float rotationSpeed;
    private void Start()
    {

        myAnimationController.SetBool("ClemMove", false);

        Clem.transform.rotation = A.transform.rotation;
        Clem.transform.position = A.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("in trigger");
        if (other.CompareTag("Player"))
        {

           
            DoorClose.SetActive(true);
         

            Grone.Play();

            moving = true;



        }

    }
    public bool moving;

    public void Update()
    {
        if (moving)
        {
            ClemMove();
        }
        if (!moving)
        {
            myAnimationController.SetBool("ClemMove", false);
            myAnimationController.SetBool("ClemIdel", true);
        }

    }
    public void ClemMove()
    {

        Vector3 destination = B.position;

        var direction = (B.transform.position - Clem.transform.position).normalized;

        var targetRotation = Quaternion.LookRotation(direction);

        if (Vector3.Distance(Clem.transform.position, destination) != 0f)
        {

            myAnimationController.SetBool("ClemMove", true);

            myAnimationController.SetBool("ClemIdel", false);

            Clem.transform.position = Vector3.MoveTowards(Clem.transform.position, destination, speed * Time.deltaTime);
            Clem.transform.rotation = targetRotation;

        }

        else 
        {

            myAnimationController.SetBool("ClemMove", false);
            myAnimationController.SetBool("ClemIdel", true);
            Grone.Stop();

            Clem.SetActive(false);
            Debug.Log("ByeClem");
            DoorClose.SetActive(false);

            moving = false;
        }
    }



}



