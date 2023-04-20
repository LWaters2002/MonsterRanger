using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepSound : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip[] footstepSounds;

    public void PlayFootstep()
    {
        footstepSource.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        footstepSource.Play();
    }
}
