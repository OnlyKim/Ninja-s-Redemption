using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioClip sfxDamage, sfxBlock, sfxTakingDamage;

	public GameObject soundObject;

	private void Awake()
	{
		instance = this;
	}

	public void PlaySFX(string sfxName)
	{
		switch (sfxName)
		{
			case "Damage":
				SoundObjectCreation(sfxDamage);
				break;
			case "Block":
				SoundObjectCreation(sfxBlock);
				break;
			case "TakingDamage":
				SoundObjectCreation(sfxTakingDamage);
				break;
			default:
				break;
		}
	}

	private void SoundObjectCreation(AudioClip clip)
	{
		GameObject newObject = Instantiate(soundObject, transform);

		newObject.GetComponent<AudioSource>().clip = clip;

		newObject.GetComponent<AudioSource>().Play();
	}

}
