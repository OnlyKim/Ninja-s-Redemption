using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playControlsPanel;

    public void LoadNextLevel()
	{
        StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex + 1));
	}

    public void LoadMenu()
	{
        Time.timeScale = 1f;
        StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void RestartFight()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Transition(int levelIndex)
	{
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
	}
    //public void PlayGame()
    //{
    //    mainMenuPanel.SetActive(false);

    //    //StartCoroutine(MenuTransition());
    //    playControlsPanel.SetActive(true);
    //}

    //public void ControlsReturn()
    //{
    //    playControlsPanel.SetActive(false);
    //   // StartCoroutine(MenuTransition());
    //    mainMenuPanel.SetActive(true);
    //}

    //public void ControlsContinue()
    //{
    //    FindAnyObjectByType<LevelLoader>().LoadNextLevel();
    //}

    //private IEnumerator MenuTransition()
    //{
    //    transitionAnimator.SetTrigger("Start");

    //    yield return new WaitForSeconds(transitionTime);
    //}
}
