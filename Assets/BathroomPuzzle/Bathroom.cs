using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //public Respawn respawn;

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
                Debug.Log("Levers pulled in correct order. Destroying electric water.");
                DestroyElectricWater();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_electricWaterDestroyed)
                {
                    Debug.Log("Player entered electric collider but electric water is off. No action taken.");
                    return; // If electric water is off, no further action is needed
                }

                Debug.Log("Player entered electric collider while electric water is still active.");

                // proceed with killing the player
                Character.SetActive(false);
                PlayerDead.SetActive(true);
                BathroomDeathCam.SetActive(true);

                //StartCoroutine(WaitBetweenFadeInOut());
                ResetAllLevers();

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
            Debug.Log($"{lever.GetLeverType()} pulled down. Sequence length: {_leverSequence.Count}");

            // check if we have reached three levers in the sequence
            if (_leverSequence.Count == 3)
            {
                if (CheckLeverSequence())
                {
                    Debug.Log("Correct lever sequence detected.");
                    DestroyElectricWater();
                }
                else
                {
                    Debug.Log("Incorrect lever sequence. Resetting levers.");
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
                return true; // correct order
            }

            return false; // incorrect order
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
                Debug.Log("Electric water stopped.");
            }

            if (ElectricColliderTrigger != null)
            {
                ElectricColliderTrigger.enabled = false;
                Debug.Log("Electric collider disabled.");
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
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
 
        public Levers lever4Script, lever1Script, lever5Script;
 
        private bool _electricWaterDestroyed = false;
        private int _leverSequenceStep = 0; // Track lever pulling order
 
        private void Start()
        {
            PlayerDead.SetActive(false);
            BathroomDeathCam.SetActive(false);
            SetElectricSystemActive(true); // Start with the electric system active
        }
 
        private void Update()
        {
            if (!_electricWaterDestroyed && CheckLeverSequence())
            {
                Debug.Log("Levers pulled in correct order. Destroying electric water.");
                DestroyElectricWater();
            }
        }
 
        private void OnTriggerEnter(Collider ElectricColliderTrigger)
        {
            if (ElectricColliderTrigger.CompareTag("Player"))
            {
                if (_electricWaterDestroyed)
                {
                    Debug.Log("Player entered electric collider but electric water is off. No action taken.");
                    return; // If electric water is off, no further action is needed
                }
 
                Debug.Log("Player entered electric collider while electric water is still active.");
 
                // Proceed with killing the player
                Character.SetActive(false);
                PlayerDead.SetActive(true);
                BathroomDeathCam.SetActive(true);
 
                if (characterMain != null)
                {
                    characterMain.enabled = false;
                }
            }
        }
 
        private bool CheckLeverSequence()
        {
            if (lever4Script == null || lever1Script == null || lever5Script == null)
            {
                Debug.LogError("Lever scripts are not assigned.");
                return false;
            }
 
            // Step 1: Check if lever 4 is down first
            if (_leverSequenceStep == 0 && lever4Script.IsLeverDown())
            {
                Debug.Log("Lever 4 pulled down. Proceed to the next lever.");
                _leverSequenceStep++; // Move to the next step in the sequence
            }
            // Step 2: Check if lever 1 is down second
            else if (_leverSequenceStep == 1 && lever1Script.IsLeverDown())
            {
                Debug.Log("Lever 1 pulled down. Proceed to the next lever.");
                _leverSequenceStep++; // Move to the next step
            }
            // Step 3: Check if lever 5 is down last
            else if (_leverSequenceStep == 2 && lever5Script.IsLeverDown())
            {
                Debug.Log("Lever 5 pulled down. All levers down in the correct order.");
                return true; // All levers were pulled in the correct order
            }
 
            // If any lever is pulled out of order, reset the sequence and lever states
            if ((_leverSequenceStep == 0 && (lever1Script.IsLeverDown() || lever5Script.IsLeverDown())) ||
                (_leverSequenceStep == 1 && lever5Script.IsLeverDown()))
            {
                Debug.Log("Levers pulled out of order. Resetting sequence and lever states.");
                _leverSequenceStep = 0; // Reset the sequence
 
                // Reset all levers
                ResetAllLevers();
            }
 
            return false; // The sequence is not complete yet
        }
 
        private void ResetAllLevers()
        {
            if (lever4Script != null) lever4Script.ResetLeverState();
            if (lever1Script != null) lever1Script.ResetLeverState();
            if (lever5Script != null) lever5Script.ResetLeverState();
        }
 
        private void DestroyElectricWater()
        {
            if (electricWater != null)
            {
                electricWater.Stop();
                _electricWaterDestroyed = true; // Mark the electric water as destroyed
                Debug.Log("Electric water stopped.");
            }
 
            if (ElectricColliderTrigger != null)
            {
                ElectricColliderTrigger.enabled = false;
                Debug.Log("Electric collider disabled.");
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
    }
}
*/