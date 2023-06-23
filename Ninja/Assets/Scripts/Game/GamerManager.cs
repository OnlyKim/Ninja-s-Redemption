using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GamerManager : MonoBehaviour
{
    public static GamerManager Instance;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    private EnemyScript enemy;
    private NinjaController ninja;
    private bool victory = false;

    [HideInInspector] public bool stopGame = false;

	private void Start()
	{
        enemy = FindAnyObjectByType<EnemyScript>();
        ninja = FindObjectOfType<NinjaController>();
        //victoryPanel.SetActive(false);
        //defeatPanel.SetActive(false);
    }

	// Update is called once per frame
	void Update()
    {
        //Verifica se a vida do inimigo chegou a zero (jogador ganhou)
        if (enemy.enemyCurrentHP == 0)
        {
            stopGame = true;
            enemy.animator.SetBool("isDown", true);
            StartCoroutine(Delay());
            victoryPanel.SetActive(true);
        }

        //Verifica se a vida do jogador chegou a zero (jogador perdeu)
        if(ninja.currentHP == 0)
		{
            stopGame = true;
            ninja.animator.SetBool("isDown", true);
            StartCoroutine(Delay());
            defeatPanel.SetActive(true);
        }
    }

    public void LoadMenu()
	{
        FindObjectOfType<LevelLoader>().LoadMenu();
	}

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);

        //if (victory)
        //    victoryPanel.SetActive(true);
        //else
        //    defeatPanel.SetActive(true);
    }
}
