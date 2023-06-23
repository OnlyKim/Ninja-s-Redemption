using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime;
    private const int menuIndex = 0;

    public void LoadNextLevel()
	{
        StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex + 1));
	}

    public void LoadMenu()
	{
        Time.timeScale = 1f;
        StartCoroutine(Transition(menuIndex));
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
}
