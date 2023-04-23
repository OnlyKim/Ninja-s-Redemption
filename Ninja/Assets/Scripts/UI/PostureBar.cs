using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostureBar : MonoBehaviour
{
	public Slider postureSlider;

	public void SetMinPosture(int posture)
	{
		postureSlider.minValue = posture;
		postureSlider.value = posture;
	}
	public void SetPosture(int newPosture)
	{
		postureSlider.value = newPosture;
	}
}