using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Student : MonoBehaviour
{
    [System.Serializable]
    class StudentAsset
    {
        public Sprite Body = null;
        public Sprite Hand = null;
    }

    [SerializeField]
    private List<StudentAsset> m_studentSprites = new List<StudentAsset>();
    [SerializeField]
    private SpriteRenderer m_leftHand;
    [SerializeField]
    private SpriteRenderer m_rightHand;

    [SerializeField]
    private ParticleSystem m_explosion;
    [SerializeField]
    private GameObject m_trash;

    private SpriteRenderer m_renderer;
    private Animator m_animator;

	void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponent<SpriteRenderer>();
        if (m_studentSprites.Count > 0)
        {
            int rand = Random.Range(0, m_studentSprites.Count);
            m_renderer.sprite = m_studentSprites[rand].Body;
            m_leftHand.sprite = m_studentSprites[rand].Hand;
            m_rightHand.sprite = m_studentSprites[rand].Hand;
        }
	}

    public void CraftPotion(bool success)
    {
        m_animator.SetTrigger("Crafting");
        if (success)
        {
            m_animator.SetTrigger("Success");
        }
        else
        {
            m_animator.SetTrigger("Explosion");
        }
    }

    public void AnimExplosion()
    {
        Transform desk = transform;
        while (desk != null && !desk.name.Contains("Desk"))
        {
            desk = desk.parent;
        }
        transform.parent.SetParent(null);
        if (desk)
        {
            desk.gameObject.SetActive(false);
        }
        m_explosion.Play();
        m_trash.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].enabled = false;
        }
    }
}
