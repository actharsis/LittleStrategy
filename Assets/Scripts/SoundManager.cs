using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    private AudioSource _infantryAttackChannel;
    public AudioClip InfantryAttackClip;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _infantryAttackChannel = gameObject.AddComponent<AudioSource>();
        _infantryAttackChannel.volume = 0.2f;
        _infantryAttackChannel.playOnAwake = true;
    }

    public void PlayInfantryAttackSound()
    {
        if (!_infantryAttackChannel.isPlaying)
        {
            //currently disabled
            //_infantryAttackChannel.PlayOneShot(InfantryAttackClip);
        }
    }
}
