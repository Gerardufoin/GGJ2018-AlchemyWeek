using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField]
    private bool m_disableOnStart = false;

    public delegate void Callback();
    private Callback m_fadeOutCallback;

    public float m_fadeInTime;
    public float m_fadeOutTime;

    private bool m_fadeIn;
    private float m_timer;
    private Image m_renderer;
    private Animator m_animator;

    private Controls m_cManager;
    private SoundsManager m_sManager;

    private void Start()
    {
        m_sManager = GameObject.FindObjectOfType<SoundsManager>();
        m_cManager = GameObject.FindObjectOfType<Controls>();
        m_renderer = GetComponent<Image>();
        m_animator = GetComponent<Animator>();
        if (m_disableOnStart)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void AnimFadeOut()
    {
        m_fadeIn = false;
        m_timer = m_fadeOutTime;
        StartCoroutine(CoTransition());
    }

    public void AnimFadeIn()
    {
        m_fadeIn = true;
        m_timer = m_fadeInTime;
        StartCoroutine(CoTransition());
    }

    public void AnimHide()
    {
        m_renderer.material.SetFloat("_Opacity", 0.0f);
    }

    public void AnimShow()
    {
        m_renderer.material.SetFloat("_Opacity", 1.0f);
    }

    public void FadeIn()
    {
        m_animator.SetTrigger("FadeIn");
    }

    public void SimpleFadeOut()
    {
        m_animator.SetTrigger("SimpleFadeOut");
    }

    public void FadeOut()
    {
        m_animator.SetTrigger("FadeOut");
    }

    public void FadeToGameOver()
    {
        m_animator.SetTrigger("GameOver");
        m_sManager.GameOver();
    }

    public void FadeToVictory()
    {
        m_animator.SetTrigger("Victory");
    }

    public void BackToMenuCallback()
    {
        m_cManager.m_ReturnToMenuOnClick = true;
    }

    public void SetCallback(Callback cb)
    {
        m_fadeOutCallback += cb;
    }

    public void TriggerCallback()
    {
        if (m_fadeOutCallback != null)
        {
            m_fadeOutCallback();
            m_fadeOutCallback = null;
        }
    }

    private IEnumerator CoTransition()
    {
        while ((m_timer -= Time.deltaTime) > 0)
        {
            float value = m_timer / (m_fadeIn ? m_fadeInTime : m_fadeOutTime);
            m_renderer.material.SetFloat("_Opacity", (m_fadeIn ? value : 1 - value));
            yield return new WaitForFixedUpdate();
        }
    }
}
