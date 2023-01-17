using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public RandomAudioGroup[] audioGroups;
    public float pitchRange = 0.2f;

    public void PlayRandomSfx(int currentGroup)
    {
        audioGroups[currentGroup].PlayRandomAudio(audioSource);
    }

    public void PlaySfx(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlaySfxWithPitch(AudioClip audioClip)
    {
        audioSource.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange);
        audioSource.PlayOneShot(audioClip);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }


    [System.Serializable]
    public class RandomAudioGroup
    {
        public string groupName;
        public AudioClip[] audioClips;
        public bool randomizePitch = false;
        public float pitchRange = 0.2f;

        [HideInInspector]
        public AudioClip audio;

        public void PlayRandomAudio(AudioSource audioSource)
        {
            if (randomizePitch) audioSource.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange);

            audio = audioClips[Random.Range(0, audioClips.Length)];

            audioSource.PlayOneShot(audio);
        }
    }
}
