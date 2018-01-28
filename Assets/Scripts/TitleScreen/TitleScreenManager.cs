using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField]
    private Transition m_transition;

    private SoundsManager m_sManager;

    private void Start()
    {
        m_sManager = GameObject.FindObjectOfType<SoundsManager>();
    }

    public void StartGame()
    {
        m_transition.gameObject.SetActive(true);
        m_transition.SetCallback(() =>
        {
            SceneManager.LoadSceneAsync("Classroom");
        });
        m_transition.SimpleFadeOut();
        m_sManager.FadeOutBGM();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
