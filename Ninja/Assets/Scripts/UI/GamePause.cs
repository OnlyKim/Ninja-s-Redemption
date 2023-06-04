using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject controlsPanel;

	[SerializeField] private bool gamePaused = false;


	//C�digo do MENU PAUSA
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}

	//Verifica se o jogo est� no modo PAUSA
	void Pause()
	{
		if(gamePaused == false)
		{
			Time.timeScale = 0f;
			pausePanel.SetActive(true);
			gamePaused = true;
		}
		else
		{
			Resume();
		}
	}

	public void Resume() //Botao RESUME
	{
		Time.timeScale = 1f;
		pausePanel.SetActive(false);
		gamePaused = false;
	}

	public void ReturnToMenu() //Bot�o MENU
	{
		FindObjectOfType<LevelLoader>().LoadMenu();
	}

	public void ControlsMenu() //Bot�o RETURN no menu de controles
	{
		pausePanel.SetActive(false);		
		controlsPanel.SetActive(true);
	}

	public void Quit() //Bot�o QUIT
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}
