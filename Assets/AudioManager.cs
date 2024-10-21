using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public GameObject SFXPrefab;

        private void Start()
        {
            instance = this;
        }

        public void PlayAudioSFX(AudioClip clip, Vector3 pos)
        {
            GameObject obj = Instantiate(SFXPrefab, pos, Quaternion.identity);
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(obj, audioSource.clip.length);
        }
    }
}
