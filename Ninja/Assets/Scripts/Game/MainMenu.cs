using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void PlayGame()
	{
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		FindAnyObjectByType<LevelLoader>().LoadNextLevel();
	}

	public void ReturnMenu()
	{
		FindAnyObjectByType<LevelLoader>().LoadMenu();
	}

	public void RestartFight()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitGame()
	{
		Debug.Log("Quit");
		Application.Quit();
	}


}
