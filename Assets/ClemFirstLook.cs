using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClemFirstLook : MonoBehaviour
{

    public AudioSource Grone;
    public GameObject Clem;
    public GameObject DoorClose;
    [SerializeField] private Animator myAnimationController;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in trigger");
        if (other.CompareTag("Player"))
        {
            Grone.Play();
            Clem.SetActive(true);
            DoorClose.SetActive(true);
            myAnimationController.SetBool("ClemMove", true);

            StartCoroutine(Waiting());

        }

    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(5.2f);

        EndClem();
    }

    public void EndClem()
    {
      


        Clem.SetActive(false);
        Debug.Log("ByeClem");
        DoorClose.SetActive(false);

        myAnimationController.SetBool("ClemMove", false);
    }


}
