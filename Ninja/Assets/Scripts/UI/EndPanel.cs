using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime;

    public void ReplayButton()
    {
        StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex));
    }

    public void MenuButton()
	{
        FindObjectOfType<LevelLoader>().LoadMenu();
	}

    private IEnumerator Transition(int levelIndex)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
