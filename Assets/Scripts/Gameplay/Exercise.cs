using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise : MonoBehaviour
{
    public enum E_Elements
    {
        POTION,
        RANDOM_POTION,
        UNIQUE_POTION,
        RANDOM_RUNE,
        PREV_RESULT
    }

    [System.Serializable]
    public class ExerciseElement
    {
        public E_Elements Type;
        public SpriteRenderer Renderer;
        public bool IsResult;
    }

    public List<ExerciseElement> m_elements = new List<ExerciseElement>();

    private QuestionManager m_qManager;

    private class Potion
    {
        public Color Color;
        public Sprite Sprite;
    }
    private List<Potion> m_usedPotions = new List<Potion>();

    private Material m_lastMat;
    private Color m_lastColor;
    private Sprite m_lastSprite;

	void Start ()
    {
        m_qManager = GameObject.FindObjectOfType<QuestionManager>();
        for (int i = 0; i < m_elements.Count; ++i)
        {
            switch(m_elements[i].Type)
            {
                case E_Elements.POTION:
                    SetPotion(m_elements[i].Renderer);
                    break;
                case E_Elements.RANDOM_POTION:
                    SetRandomPotion(m_elements[i].Renderer);
                    break;
                case E_Elements.UNIQUE_POTION:
                    SetUniquePotion(m_elements[i].Renderer);
                    break;
                case E_Elements.RANDOM_RUNE:
                    SetRandomRune(m_elements[i].Renderer);
                    break;
                case E_Elements.PREV_RESULT:
                    SetPrevResult(m_elements[i].Renderer);
                    break;
            }
            if (m_elements[i].IsResult)
            {
                SetResult(m_elements[i].Renderer);
            }
        }
	}

    private void AddToUsed(SpriteRenderer renderer)
    {
        Potion data = new Potion();
        data.Color = renderer.color;
        data.Sprite = renderer.sprite;
        m_usedPotions.Add(data);
    }

    private void SetPotion(SpriteRenderer renderer, bool used = true)
    {
        renderer.color = m_qManager.GetRandomColor();
        if (used)
        {
            AddToUsed(renderer);
        }
    }

    private void SetRandomPotion(SpriteRenderer renderer, bool used = true)
    {
        QuestionManager.PotionData data = m_qManager.GetRandomPotion();
        if (data != null)
        {
            renderer.sprite = data.Sprite;
            renderer.material = data.Material;
        }
        SetPotion(renderer, used);
    }

    private bool IsUnique(SpriteRenderer renderer)
    {
        for (int i = 0; i < m_usedPotions.Count; ++i)
        {
            if (renderer.sprite == m_usedPotions[i].Sprite && renderer.color == m_usedPotions[i].Color)
            {
                return (false);
            }
        }
        return (true);
    }

    private void SetUniquePotion(SpriteRenderer renderer)
    {
        int limit = 100;
        do
        {
            SetRandomPotion(renderer, false);
        } while (!IsUnique(renderer) && limit-- > 0);
        AddToUsed(renderer);
    }

    private void SetRandomRune(SpriteRenderer renderer)
    {
        renderer.sprite = m_qManager.GetRandomRune();
    }

    private void SetPrevResult(SpriteRenderer renderer)
    {
        renderer.sprite = m_lastSprite;
        renderer.material = m_lastMat;
        renderer.color = m_lastColor;
    }

    private void SetResult(SpriteRenderer renderer)
    {
        m_lastSprite = renderer.sprite;
        m_lastMat = renderer.material;
        m_lastColor = renderer.color;
    }
}
