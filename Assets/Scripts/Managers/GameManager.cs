using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const int MAX_DAYS = 5;

    public int m_currentDay;
    [SerializeField]
    private GameObject m_teacher;
    [SerializeField]
    private GameObject m_studentPrefab;
    [SerializeField]
    private Transition m_transition;
    [SerializeField]
    private Text m_text; 
    public List<string> m_days = new List<string>();

    private SoundsManager m_sManager;
    private QuestionManager m_qManager;
    private ScoreManager m_scoreManager;
    private Stack<Student> m_studentsList = new Stack<Student>();

    private QuestionManager.ChemistryData m_currentExercise = null;

	void Start ()
    {
        m_qManager = GameObject.FindObjectOfType<QuestionManager>();
        m_sManager = GameObject.FindObjectOfType<SoundsManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        m_currentDay = Mathf.Clamp(m_currentDay, 1, MAX_DAYS);
        m_text.text = m_days[m_currentDay - 1];
        m_transition.FadeIn();
        NewRound();
	}
	
    private void NewRound()
    {
        m_qManager.NewDay(m_currentDay);
        m_currentExercise = m_qManager.NextExercise();
        SpawnStudents();
        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        yield return new WaitForSeconds(1.5f);
        int studentAlive = m_studentsList.Count;
        while (m_studentsList.Count > 0)
        {
            yield return new WaitForSeconds(1.5f);
            Exercise exercise = Instantiate(m_currentExercise.Explanation, m_teacher.transform);
            m_sManager.PlayExperience();
            yield return new WaitForSeconds(m_currentExercise.DisplayTime);
            exercise.gameObject.SetActive(false);
            yield return new WaitForSeconds(2.0f);
            for (int i = 0; i < m_currentExercise.Questions.Count && m_studentsList.Count > 0; ++i)
            {
                Question question = Instantiate(m_currentExercise.Questions[i], m_studentsList.Peek().transform);
                m_sManager.PlayQuestion();
                question.Init(exercise.m_elements);
                while (!question.IsAnswered())
                {
                    yield return new WaitForFixedUpdate();
                }
                Student student = m_studentsList.Pop();
                bool rightAnswer = question.IsAnswerRight();
                student.CraftPotion(rightAnswer);
                GameObject.Destroy(question.gameObject);
                yield return new WaitForSeconds((rightAnswer ? 1.5f : 2.8f));
                if (!rightAnswer)
                {
                    studentAlive -= 1;
                    m_scoreManager.KillStudent();
                }
                else
                {
                    m_scoreManager.SaveStudent(question.GetTimeLeft());
                }
                yield return new WaitForSeconds(2.0f);
            }
            GameObject.Destroy(exercise.gameObject);
            if (m_studentsList.Count > 0)
            {
                m_currentExercise = m_qManager.NextExercise();
            }
        }
        if (studentAlive <= 0)
        {
            m_transition.FadeToGameOver();
        }
        else if (++m_currentDay > MAX_DAYS)
        {
            m_transition.FadeToVictory();
        }
        else
        {
            m_text.text = m_days[m_currentDay - 1];
            m_transition.SetCallback(() => {
                DeleteStudents();
                NewRound();
            });
            m_transition.FadeOut();
            m_transition.FadeIn();
        }
    }

    private void DeleteStudents()
    {
        GameObject[] students = GameObject.FindGameObjectsWithTag("Student");
        for (int i = 0; i < students.Length; ++i)
        {
            GameObject.Destroy(students[i].gameObject);
        }
    }

    private void SpawnStudents()
    {
        m_currentDay = Mathf.Clamp(m_currentDay, 1, MAX_DAYS);
        m_studentsList.Clear();
        GameObject[] seatsArray = GameObject.FindGameObjectsWithTag("StudentSlot");
        List<GameObject> seats = new List<GameObject>(seatsArray);
        int count = (int)(((float)m_currentDay / MAX_DAYS) * seats.Count) - m_currentDay;
        for (int i = 0; i < count; ++i)
        {
            int rand = Random.Range(0, seats.Count - 1);
            GameObject student = Instantiate(m_studentPrefab, seats[rand].transform);
            m_studentsList.Push(student.GetComponentInChildren<Student>());
            seats.RemoveAt(rand);
        }
    }
}
