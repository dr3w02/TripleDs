using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Platformer
{
    public class Bathroom : MonoBehaviour
    {
        public ParticleSystem electricWater;
        public Collider ElectricColliderTrigger;
        public GameObject PlayerDead;
        public GameObject Character;
        public characterMovement characterMain;
        public GameObject BathroomDeathCam;

        public Levers lever1Script, lever2Script, lever3Script, lever4Script, lever5Script, lever6Script;

        private bool _electricWaterDestroyed = false;

        private Queue<Levers> _leverSequence = new Queue<Levers>(); // order of pulled levers

        public Respawn respawn;
        [SerializeField] private float WaitTime = 20f;
        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 2f;

        public AudioSource staticElectricity;
        public AudioSource electrocutedSound;

        private void Start()
        {
            PlayerDead.SetActive(false);
            BathroomDeathCam.SetActive(false);
            SetElectricSystemActive(true);
        }

        private void Update()
        {
            if (!_electricWaterDestroyed && CheckLeverSequence())
            {
                //Debug.Log("Levers pulled in correct order. Destroying electric water.");
                DestroyElectricWater();
            }
        }

        private void OnTriggerEnter(Collider ElectricColliderTrigger)
        {
            if (ElectricColliderTrigger.CompareTag("Player"))
            {
                if (_electricWaterDestroyed)
                {
                    //Debug.Log("player entered electric collider but electric water is off. No action taken.");
                    return; // If electric water is off, no further action is needed
                }


                // proceed with killing the player
                //Debug.Log("start fade");

                StartCoroutine(WaitBetweenFadeInOut());


                Character.SetActive(false);
                PlayerDead.SetActive(true);
                BathroomDeathCam.SetActive(true);
                ResetAllLevers();
                electrocutedSound.Play();

                if (characterMain != null)
                {
                    characterMain.enabled = false;
                }

            }
        }

        public void RegisterLeverPull(Levers lever)
        {
            if (_electricWaterDestroyed)
                return; // if the system is destroyed, no further action is needed

            // add the lever to the sequence queue
            _leverSequence.Enqueue(lever);
            //Debug.Log($"{lever.GetLeverType()} pulled down. Sequence length: {_leverSequence.Count}");

            // check if we have reached three levers in the sequence
            if (_leverSequence.Count == 3)
            {
                if (CheckLeverSequence())
                {
                    if (staticElectricity != null && staticElectricity.isPlaying)
                    {
                        staticElectricity.Stop(); // stop the sound
                    }
                    //Debug.Log("Correct lever sequence detected.");
                    DestroyElectricWater();
                }
                else
                {
                    //Debug.Log("Incorrect lever sequence. Resetting levers.");
                    ResetAllLevers();
                }
            }
        }

        private bool CheckLeverSequence()
        {
            // if less than 3 levers have been pulled, no need to check
            if (_leverSequence.Count < 3) return false;

            // extract the pulled levers
            Levers firstLever = _leverSequence.Dequeue();
            Levers secondLever = _leverSequence.Dequeue();
            Levers thirdLever = _leverSequence.Dequeue();

            // sequence is exactly Lever4 -> Lever1 -> Lever5
            if (firstLever == lever4Script && secondLever == lever1Script && thirdLever == lever5Script)
            {
                StopStaticSound();
                return true; // correct order
            }


            return false; // incorrect order
        }

        [ContextMenu("Static Debug")]
        public void StopStaticSound()
        {
            staticElectricity.Stop();
        }

        private void ResetAllLevers()
        {
            // reset all the levers to the up state
            if (lever4Script != null) lever1Script.ResetLeverState();
            if (lever1Script != null) lever2Script.ResetLeverState();
            if (lever5Script != null) lever3Script.ResetLeverState();
            if (lever4Script != null) lever4Script.ResetLeverState();
            if (lever1Script != null) lever5Script.ResetLeverState();
            if (lever5Script != null) lever6Script.ResetLeverState();

            // clear the sequence queue
            _leverSequence.Clear();
        }

        private void DestroyElectricWater()
        {
            if (electricWater != null)
            {
                electricWater.Stop();
                _electricWaterDestroyed = true; // electric water destroyed
                //Debug.Log("Electric water stopped.");
            }

            if (ElectricColliderTrigger != null)
            {
                ElectricColliderTrigger.enabled = false;
                //Debug.Log("Electric collider disabled.");
            }
        }

        private void SetElectricSystemActive(bool isActive)
        {
            if (electricWater != null)
            {
                electricWater.gameObject.SetActive(isActive);
            }

            if (ElectricColliderTrigger != null)
            {
                ElectricColliderTrigger.enabled = isActive;
            }
        }

        public IEnumerator WaitBetweenFadeInOut()
        {

            yield return new WaitForSeconds(WaitTime);

            fadeIn = true;

            while (fadeIn)
            {
                if (myUIGroup.alpha < 1)
                {
                    myUIGroup.alpha += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }
                else
                {
                    fadeIn = false;
                }
            }

            respawn.RespawnPlayer();

            // Wait for the specified amount of time
            yield return new WaitForSeconds(fadeWaitTime);

            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame
                    BathroomDeathCam.SetActive(false);
                    Character.SetActive(true);
                    if (characterMain != null)
                    {
                        characterMain.Enabled();
                    }
                    if (characterMain == null)
                    {
                        Debug.Log("DSFHDSHGFHKSDGHKFKHGDHGDSFHKKKG");
                    }
                   

                    ResetAllLevers();
                    PlayerDead.SetActive(false);
                }
                else
                {
                    fadeOut = false;
                }
            }
        }
    }
}