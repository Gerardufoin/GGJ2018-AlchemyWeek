using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StudentAnimationSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_explosion;
    [SerializeField]
    private AudioClip m_whistle;
    [SerializeField]
    private AudioClip m_success;

    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlayExplosion()
    {
        m_audioSource.Stop();
        m_audioSource.PlayOneShot(m_explosion);
    }

    public void PlayWhistle()
    {
        m_audioSource.clip = m_whistle;
        m_audioSource.Play();
    }

    public void PlaySuccess()
    {
        m_audioSource.PlayOneShot(m_success);
    }
}
