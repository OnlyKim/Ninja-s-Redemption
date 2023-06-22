using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
<<<<<<< Updated upstream
	[SerializeField] GameObject mainPanel;
	[SerializeField] GameObject optionsPanel;

	//Menu principal
	public void PlayGame()
	{
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		FindAnyObjectByType<LevelLoader>().LoadNextLevel();
	}
=======
	[SerializeField] private Animator transitionAnimator;
	[SerializeField] private float transitionTime;
>>>>>>> Stashed changes

	[SerializeField] private GameObject mainMenuPanel;
	[SerializeField] private GameObject playControlsPanel;
	//public void PlayGame()
	//{
	//	mainMenuPanel.SetActive(false);

	//	StartCoroutine(MenuTransition());
	//	playControlsPanel.SetActive(true);
	//}

	//public void ReturnMenu()
	//{
	//	FindAnyObjectByType<LevelLoader>().LoadMenu();
	//}

	//public void RestartFight()
	//{
	//	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	//}

	//public void QuitGame()
	//{
	//	Debug.Log("Quit");
	//	Application.Quit();
	//}

	//public void ControlsReturn()
	//{
	//	playControlsPanel.SetActive(false);
	//	StartCoroutine(MenuTransition());
	//	mainMenuPanel.SetActive(true);
	//}

	//public void ControlsContinue()
	//{
	//	FindAnyObjectByType<LevelLoader>().LoadNextLevel();
	//}

	//private IEnumerator MenuTransition()
	//{
	//	transitionAnimator.SetTrigger("Start");

	//	yield return new WaitForSeconds(transitionTime);
	//}


}
