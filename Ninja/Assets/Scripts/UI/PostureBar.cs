using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostureBar : MonoBehaviour
{
	[SerializeField] float startTime;
	[SerializeField] float timeDecreaseAmount;

	[SerializeField] NinjaController player;
	[SerializeField] EnemyScript enemy;
	
	//Informa na Unity se � inimigo ou player. S� um deve ser true
	[SerializeField] bool isPlayer;
	[SerializeField] bool isEnemy;

	public Slider postureSlider;

	//[HideInInspector] public float currentTime;

	
	public void SetMinPosture(int posture)
	{
		postureSlider.minValue = posture;
		postureSlider.value = posture;
	}
	public void SetPosture(float newPosture)
	{
		postureSlider.value = newPosture;
	}
}