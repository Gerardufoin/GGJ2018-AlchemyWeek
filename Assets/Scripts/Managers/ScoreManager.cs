using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    const float PRIVATE_SCORE_INC = 1000.0f;

    [SerializeField]
    private Text m_scoreText;
    [SerializeField]
    private Text m_comboText;
    [SerializeField]
    private Text m_finalScoreText;

    private int m_displayScore;
    private int m_score;
    private int m_combo = 1;

    public void SaveStudent(float timeleft)
    {
        m_score += 500 + (int)(timeleft * m_combo * 100);
        m_combo++;
        UpdateCombo();
        UpdateFinalScore();
    }

    public void KillStudent()
    {
        m_score -= 500;
        if (m_score < 0) m_score = 0;
        m_combo = 1;
        UpdateCombo();
        UpdateFinalScore();
    }

    private void UpdateCombo()
    {
        m_comboText.text = "COMBO x" + m_combo.ToString("00");
    }

    private void UpdateFinalScore()
    {
        m_finalScoreText.text = "FINAL SCORE   " + m_score.ToString("000000");
    }

    private void Update()
    {
        if (m_displayScore != m_score)
        {
            m_displayScore = Mathf.Clamp(m_displayScore + (int)(PRIVATE_SCORE_INC * Time.deltaTime) * (m_displayScore > m_score ? -1 : 1), 0, m_score);
            m_scoreText.text = "SCORE  " + m_displayScore.ToString("000000");
        }
    }
}
