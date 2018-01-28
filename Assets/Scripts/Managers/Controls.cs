using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    private float ESC_TIMER = 2.0f;

    public bool m_ReturnToMenuOnClick;

    private float m_timer;

    void Update ()
    {
		if (m_ReturnToMenuOnClick && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadSceneAsync("TitleScreen");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_timer = 0;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            if ((m_timer += Time.deltaTime) >= ESC_TIMER)
            {
                SceneManager.LoadScene("TitleScreen");
            }
        }
	}
}
