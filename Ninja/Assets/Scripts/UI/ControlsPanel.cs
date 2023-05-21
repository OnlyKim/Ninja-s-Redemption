using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPanel : MonoBehaviour
{
	[SerializeField] private GameObject controlsPanel;
	[SerializeField] private GameObject pausePanel;

	public void ReturnToPause()
	{
		controlsPanel.SetActive(false);
		pausePanel.SetActive(true);
	}
}
