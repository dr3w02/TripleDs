using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClemFirstLook : MonoBehaviour
{

    public AudioSource Grone;
    public GameObject Clem;

    [SerializeField] private Animator myAnimationController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Grone.Play();
            Clem.SetActive(true);

            myAnimationController.SetBool("ClemMove", true);
        }
      

    }

    IEnumerator EndClem()
    {
        yield return new WaitForSeconds(2.03f);

        Clem.SetActive(false);

        myAnimationController.SetBool("ClemMove", false);

    }

}
