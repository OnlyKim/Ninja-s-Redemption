using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject controlsPanel;

	[SerializeField] private bool gamePaused = false;


	//Código do MENU PAUSA
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}

	//Verifica se o jogo está no modo PAUSA
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

	public void ReturnToMenu() //Botão MENU
	{
		FindObjectOfType<LevelLoader>().LoadMenu();
	}

	public void ControlsMenu() //Botão RETURN no menu de controles
	{
		pausePanel.SetActive(false);		
		controlsPanel.SetActive(true);
	}

	public void Quit() //Botão QUIT
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}
