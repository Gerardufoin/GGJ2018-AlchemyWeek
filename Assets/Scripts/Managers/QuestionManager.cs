using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public class ElementData
    {
        public Sprite Sprite;
        public Color Color;
        public Material Material;
    }

    [System.Serializable]
    public class PotionData
    {
        public Sprite Sprite;
        public Material Material;
    }

    [System.Serializable]
    public class ChemistryData
    {
        public Exercise Explanation;
        public float DisplayTime = 3.0f;
        public List<Question> Questions;
    }

    [System.Serializable]
    public class DailyChemistryData
    {
        public List<ChemistryData> DailyDatas;
    }

    public ElementData m_questionHoleData;
    public ElementData m_wrongAnswerData;
    public ElementData m_rightAnswerData;
    public List<SpriteRenderer> m_answerItems = new List<SpriteRenderer>();
    public List<Color> m_colors = new List<Color>();
    public List<PotionData> m_potions = new List<PotionData>();
    public List<Sprite> m_runes = new List<Sprite>();

    public List<DailyChemistryData> m_dailyChemistryDatas = new List<DailyChemistryData>();

    private int m_index = 0;
    private int m_currentDay = 1;

    private Question m_currentQuestion;
    private int m_currentAnswerIdx;

    private List<Color> m_currentExcludedColor = new List<Color>();
    private List<PotionData> m_currentExcludedPotion = new List<PotionData>();
    private List<Sprite> m_currentExcludedSprite = new List<Sprite>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(position);
            if (hit)
            {
                Answer answer = hit.GetComponent<Answer>();
                if (answer && m_currentQuestion)
                {
                    m_currentQuestion.Answer(answer.ID == m_currentAnswerIdx);
                    for (int i = 0; i < m_answerItems.Count; ++i)
                    {
                        m_answerItems[i].transform.parent.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void NewDay(int day)
    {
        m_currentDay = day;
        m_index = 0;
    }

    public ChemistryData NextExercise()
    {
        if (m_index >= m_dailyChemistryDatas[m_currentDay - 1].DailyDatas.Count)
        {
            m_index = 0;
            Debug.Log("Not enough question on day " + m_currentDay + ". Looping.");
        }
        return m_dailyChemistryDatas[m_currentDay - 1].DailyDatas[m_index++];
    }

    public Color GetRandomColor()
    {
        return GetRandomColor(new List<Color>());
    }

    public Color GetRandomColor(List<Color> excludedColors)
    {
        if (m_colors.Count > 0 && m_colors.Count > excludedColors.Count)
        {
            Color c;
            do
            {
                c = m_colors[Random.Range(0, m_colors.Count)];
            } while (excludedColors.Contains(c));
            return c;
        }
        Debug.Log("No color set in QuestionManager.");
        return Color.black;
    }

    private bool ContainPotion(PotionData data, List<PotionData> excluded)
    {
        for (int i = 0; i < excluded.Count; ++i)
        {
            if (/*data.Material == excluded[i].Material && */data.Sprite == excluded[i].Sprite)
            {
                return true;
            }
        }
        return false;
    }

    public PotionData GetRandomPotion()
    {
        return GetRandomPotion(new List<PotionData>());
    }

    public PotionData GetRandomPotion(List<PotionData> excludedPotions)
    {
        if (m_potions.Count > 0 && m_potions.Count > excludedPotions.Count)
        {
            PotionData p;
            do
            {
                p = m_potions[Random.Range(0, m_potions.Count)];
            } while (ContainPotion(p, excludedPotions));
            return p;
        }
        Debug.Log("No potion materials set in QuestionManager.");
        return null;
    }

    public Sprite GetRandomRune()
    {
        return GetRandomRune(new List<Sprite>());
    }

    public Sprite GetRandomRune(List<Sprite> excludedRunes)
    {
        if (m_runes.Count > 0 && m_runes.Count > excludedRunes.Count)
        {
            Sprite s;
            do
            {
                s = m_runes[Random.Range(0, m_runes.Count)];
            } while (excludedRunes.Contains(s));
            return s;
        }
        Debug.Log("No rune sprites set in QuestionManager.");
        return null;
    }

    public void CreateQCMAnswer(Question question, SpriteRenderer answer, Question.E_QuestionType type)
    {
        int nbAnswer = (int)Mathf.Ceil((float)m_currentDay / 2) + 1;
        m_currentAnswerIdx = Random.Range(0, nbAnswer);
        m_currentQuestion = question;
        m_currentExcludedColor.Clear();
        m_currentExcludedPotion.Clear();
        m_currentExcludedSprite.Clear();
        ExcludeAnswer(answer);
        for (int i = 0; i < nbAnswer; ++i)
        {
            SetAnswer(m_answerItems[i], answer);
            if (i != m_currentAnswerIdx)
            {
                switch(type)
                {
                    case Question.E_QuestionType.COLOR:
                        SetAnswerColor(m_answerItems[i]);
                        break;
                    case Question.E_QuestionType.SHAPE:
                        SetAnswerShape(m_answerItems[i]);
                        break;
                    case Question.E_QuestionType.COLOR_SHAPE:
                        SetAnswerShapeColor(m_answerItems[i]);
                        break;
                    case Question.E_QuestionType.RUNE:
                        SetAnswerRune(m_answerItems[i]);
                        break;
                }
            }
            m_answerItems[i].transform.parent.GetComponent<Answer>().ID = i;
            m_answerItems[i].transform.parent.gameObject.SetActive(true);
        }
    }

    public void ExcludeAnswer(SpriteRenderer answer)
    {
        m_currentExcludedColor.Add(answer.color);
        m_currentExcludedSprite.Add(answer.sprite);
        PotionData data = new PotionData();
        data.Material = answer.material;
        data.Sprite = answer.sprite;
        m_currentExcludedPotion.Add(data);
    }

    public void CreateValidationAnswer(Question question, SpriteRenderer answer, SpriteRenderer proposition)
    {
        ApplyElementDataToRenderer(m_rightAnswerData, m_answerItems[0]);
        m_answerItems[0].transform.parent.GetComponent<Answer>().ID = 0;
        m_answerItems[0].transform.parent.gameObject.SetActive(true);
        ApplyElementDataToRenderer(m_wrongAnswerData, m_answerItems[1]);
        m_answerItems[1].transform.parent.GetComponent<Answer>().ID = 1;
        m_answerItems[1].transform.parent.gameObject.SetActive(true);
        m_currentAnswerIdx = (answer.sprite == proposition.sprite && answer.color == proposition.color ? 0 : 1);
        m_currentQuestion = question;
    }

    private void SetAnswer(SpriteRenderer bubbleObj, SpriteRenderer answer)
    {
        bubbleObj.material = answer.material;
        bubbleObj.sprite = answer.sprite;
        bubbleObj.color = answer.color;
    }

    private void SetAnswerColor(SpriteRenderer bubbleObj)
    {
        m_currentExcludedColor.Add((bubbleObj.color = GetRandomColor(m_currentExcludedColor)));
    }

    private void SetAnswerShape(SpriteRenderer bubbleObj)
    {
        PotionData data = GetRandomPotion(m_currentExcludedPotion);
        bubbleObj.sprite = data.Sprite;
        bubbleObj.material = data.Material;
        m_currentExcludedPotion.Add(data);
    }

    private bool CheckShapeColor(Color c, PotionData data)
    {
        for (int i = 0; i < m_currentExcludedColor.Count && i < m_currentExcludedPotion.Count; ++i)
        {
            if (m_currentExcludedColor[i] == c && m_currentExcludedPotion[i].Sprite == data.Sprite)
            {
                return false;
            }
        }
        return true;
    }

    private void SetAnswerShapeColor(SpriteRenderer bubbleObj)
    {
        int limit = 100;
        Color c;
        PotionData data;
        do
        {
            c = GetRandomColor();
            data = GetRandomPotion();
        } while (!CheckShapeColor(c, data) && limit-- > 0);
        if (limit < 0)
        {
            Debug.Log("SetAnswerShapeColor: Limit reached while unable to create unique ShapeColor.");
        }
        bubbleObj.color = c;
        bubbleObj.sprite = data.Sprite;
        bubbleObj.material = data.Material;
        m_currentExcludedColor.Add(c);
        m_currentExcludedPotion.Add(data);
    }

    private void SetAnswerRune(SpriteRenderer bubbleObj)
    {
        m_currentExcludedSprite.Add((bubbleObj.sprite = GetRandomRune(m_currentExcludedSprite)));
    }

    public void ApplyElementDataToRenderer(ElementData data, SpriteRenderer renderer)
    {
        renderer.sprite = data.Sprite;
        renderer.color = data.Color;
        renderer.material = data.Material;
    }
}
