using UnityEngine;

namespace DontFall
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySound : MonoBehaviour
    {
        public void Play(AudioClip clip)
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(clip);
        }

        public void ToggleMute()
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.pitch = 1 - audioSource.pitch;
        }
    }
}
