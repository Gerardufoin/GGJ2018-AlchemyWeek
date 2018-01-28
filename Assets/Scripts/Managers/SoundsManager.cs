using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_question;
    [SerializeField]
    private AudioClip m_experience;

    [SerializeField]
    private AudioClip m_gameOver;
    [SerializeField]
    private float m_gameOverVolume = 0.5f;
    [SerializeField]
    private float m_fadeOutTimer = 2.0f;

    private AudioSource m_audioSource;

	void Start ()
    {
        m_audioSource = GetComponent<AudioSource>();
	}

    public void PlayQuestion()
    {
        m_audioSource.PlayOneShot(m_question);
    }

    public void PlayExperience()
    {
        m_audioSource.PlayOneShot(m_experience);
    }

    public void GameOver()
    {
        StartCoroutine(coFadeOutBGM());
    }

    public void FadeOutBGM()
    {
        StartCoroutine(coFadeOutBGM());
    }

    private IEnumerator coFadeOutBGM()
    {
        float timer = m_fadeOutTimer;
        float startVolume = m_audioSource.volume;
        while ((timer -= Time.deltaTime) > 0)
        {
            m_audioSource.volume = Mathf.Clamp01(timer / m_fadeOutTimer) * startVolume;
            yield return new WaitForFixedUpdate();
        }
        m_audioSource.volume = 0;
    }
}
