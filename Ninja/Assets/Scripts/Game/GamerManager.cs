using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GamerManager : MonoBehaviour
{
    public static GamerManager Instance;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    [HideInInspector] public bool stopGame = false;

	private void Start()
	{
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

	// Update is called once per frame
	void Update()
    {
      

        if (FindObjectOfType<EnemyScript>().enemyCurrentHP <= 0)
        {
            stopGame = true;
            FindObjectOfType<EnemyScript>().animator.SetBool("isDown", true);
            StartCoroutine(Delay());
            victoryPanel.SetActive(true);
        }

        if(FindFirstObjectByType<NinjaController>().currentHP <= 0)
		{
            stopGame = true;
            FindObjectOfType<NinjaController>().animator.SetBool("isDown", true);
            StartCoroutine(Delay());
            defeatPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        StartCoroutine(Delay());
    }

    public void LoadMenu()
	{
        FindObjectOfType<LevelLoader>().LoadMenu();
	}

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
    }
}
