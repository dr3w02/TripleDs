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

        private void Start()
        {
            PlayerDead.SetActive(false);
            BathroomDeathCam.SetActive(false);
            SetElectricSystemActive(true); // Start with the electric system active
        }

        private void Update()
        {
            if (!_electricWaterDestroyed && AreRequiredLeversDown())
            {
                Debug.Log("All required levers are down. Destroying electric water.");
                DestroyElectricWater();
            }
        }

        private void OnTriggerEnter(Collider ElectricColliderTrigger)
        {
            if (ElectricColliderTrigger.CompareTag("Player"))
            {
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

        private bool AreRequiredLeversDown()
        {
            bool lever4Down = lever4Script != null && lever4Script.IsLeverDown();
            bool lever1Down = lever1Script != null && lever1Script.IsLeverDown();
            bool lever5Down = lever5Script != null && lever5Script.IsLeverDown();

            Debug.Log($"Lever 4 Down: {lever4Down}");
            Debug.Log($"Lever 1 Down: {lever1Down}");
            Debug.Log($"Lever 5 Down: {lever5Down}");

            return lever4Down && lever1Down && lever5Down;
        }

        private void DestroyElectricWater()
        {
            if (electricWater != null)
            {
                Destroy(electricWater.gameObject);
                _electricWaterDestroyed = true; // Mark the electric water as destroyed
                Debug.Log("Electric water destroyed.");
            }

            // Optionally disable the collider if needed
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
