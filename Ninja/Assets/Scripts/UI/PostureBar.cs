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
	
	//Informa na Unity se é inimigo ou player. Só um deve ser true
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

	//void PostureTimer()
	//{
	//	currentTime = startTime; 
	//	currentTime -= timeDecreaseAmount * Time.deltaTime;

	//	//if(currentTime <= 0)
	//	//{
	//	//	do
	//	//	{
	//	//		if (isPlayer && !isEnemy)
	//	//		{
	//	//			yield return new WaitForSeconds(1f);
	//	//		}
					
	//	//		else if (isEnemy && !isPlayer)
	//	//		{
	//	//			yield return new WaitForSeconds(1f);
	//	//		}
	//	//	} while (FindAnyObjectByType<NinjaController>().isPunching == true);
	//	//}
	//}

}