using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YayEffect : MonoBehaviour
{

    public ParticleSystem particle;
    new public AudioSource audio;

    public void Play()
    {
        particle.Play();
        audio.Play();
    }

}
