using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    const float ANSWER_TIME = 5f;

    public enum E_QuestionType
    {
        COLOR,
        SHAPE,
        COLOR_SHAPE,
        RUNE,
        NONE
    }

    [System.Serializable]
    private class QuestionData
    {
        public int ExerciseElementIndex = 0;
        public SpriteRenderer Renderer = null;
        public E_QuestionType Type = 0;
    }

    [SerializeField]
    private List<QuestionData> m_questionLink = new List<QuestionData>();

    private QuestionManager m_qManager;
    private bool m_answered;
    private bool m_success;

    private float m_timer;
    private Renderer m_timeBarRenderer;

    private void Start()
    {
        m_timeBarRenderer = GameObject.FindGameObjectWithTag("Teacher").transform.Find("TimeBar").GetComponent<Renderer>();
        m_timeBarRenderer.gameObject.SetActive(true);
        m_timeBarRenderer.material.SetFloat("_Fill", 1);
    }

    private void OnDestroy()
    {
        m_timeBarRenderer.gameObject.SetActive(false);
    }

    public void Init(List<Exercise.ExerciseElement> elements)
    {
        m_qManager = GameObject.FindObjectOfType<QuestionManager>();
        for (int i = 0; i < m_questionLink.Count; ++i)
        {
            m_questionLink[i].Renderer.material = elements[m_questionLink[i].ExerciseElementIndex].Renderer.material;
            m_questionLink[i].Renderer.color = elements[m_questionLink[i].ExerciseElementIndex].Renderer.color;
            m_questionLink[i].Renderer.sprite = elements[m_questionLink[i].ExerciseElementIndex].Renderer.sprite;
        }
        if (m_questionLink.Count > 1)
        {
            List<int> questionIndexes = new List<int>();
            for (int i = 0; i < m_questionLink.Count; ++i)
            {
                if (m_questionLink[i].Type != E_QuestionType.NONE)
                {
                    questionIndexes.Add(i);
                }
            }
            if (questionIndexes.Count < 1)
            {
                questionIndexes.Add(0);
            }
            int forgotElem = questionIndexes[Random.Range(0, questionIndexes.Count)];
            m_questionLink[forgotElem].Renderer.sprite = m_qManager.m_questionHoleData.Sprite;
            m_questionLink[forgotElem].Renderer.color = m_qManager.m_questionHoleData.Color;
            m_questionLink[forgotElem].Renderer.material = m_qManager.m_questionHoleData.Material;
            m_qManager.CreateQCMAnswer(this, elements[m_questionLink[forgotElem].ExerciseElementIndex].Renderer, m_questionLink[forgotElem].Type);
        }
        else
        {
            m_questionLink[0].Renderer.color = m_qManager.GetRandomColor();
            m_qManager.CreateValidationAnswer(this, elements[m_questionLink[0].ExerciseElementIndex].Renderer, m_questionLink[0].Renderer);
        }
    }

    public void Update()
    {
        if (!m_answered && (m_timer += Time.deltaTime) > ANSWER_TIME)
        {
            m_answered = true;
            m_success = false;
            GameObject[] answers = GameObject.FindGameObjectsWithTag("Answer");
            for (int i = 0; i < answers.Length; ++i)
            {
                answers[i].SetActive(false);
            }
        }
        else
        {
            m_timeBarRenderer.material.SetFloat("_Fill", 1 - Mathf.Clamp01(m_timer / ANSWER_TIME));
        }
    }

    public bool IsAnswered()
    {
        return m_answered;
    }

    public void Answer(bool success)
    {
        m_answered = true;
        m_success = success;
    }

    public bool IsAnswerRight()
    {
        return m_success;
    }

    public float GetTimeLeft()
    {
        return ANSWER_TIME - m_timer;
    }
}
