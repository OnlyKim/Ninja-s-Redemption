using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	[SerializeField] private GameObject attackWarning;
	[SerializeField] private GameObject enemyPositionTarget;
    [SerializeField] private GameObject enemyPunchTarget;
    [SerializeField] private GameObject enemyDodgeTarget;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float minActionPeriod;
	[SerializeField] private float maxActionPeriod;
	[SerializeField] private int attackProbability;
	[SerializeField] private int defenseProbability;
	[SerializeField] private HealthBar enemyHealthBar;
	[SerializeField] private PostureBar enemyPostureBar;

	private int rand;
	private float actionPeriod;
	private bool attack = false;
	private bool defense = false;
	private bool enemyActionAvaliable;

	private NinjaController ninjaPlayer;
	private GamerManager gameManager;

	public int enemyHP;
	public int enemyMaxPosture;
    public int punchDamage;
    public bool isPunching = false;
    public bool isBlocking = false;
	public bool wasDamaged = false;
	public Animator animator;

	[HideInInspector] public int enemyCurrentHP;
	[HideInInspector] public int enemyCurrentPosture;

	private IEnumerator Start()
	{
		ninjaPlayer = FindAnyObjectByType<NinjaController>();
		gameManager = FindAnyObjectByType<GamerManager>();
		enemyCurrentHP = enemyHP;
		enemyActionAvaliable = true;
		enemyCurrentPosture = 0;

		attackWarning.SetActive(false);
		if (enemyCurrentHP <= 0) //Inicia a animação do inimigo derrubado caso a vida dele chegue a 0
			animator.SetBool("isDown", true);
		while (true)
		{
			attack = false;
			actionPeriod = Random.Range(minActionPeriod, maxActionPeriod);

			if (!attack && ninjaPlayer.isPunching && !gameManager.stopGame && enemyActionAvaliable)
			{
				defense = true;
				enemyActionAvaliable = false;
				StartCoroutine(EnemyBlock());
			}

			yield return new WaitForSeconds(actionPeriod);

			if (defense == false && Random.Range(0, 100) > attackProbability && !gameManager.stopGame && enemyActionAvaliable)
			{
				attack = true;
				enemyActionAvaliable = false;
				StartCoroutine(EnemyAttack());
			}
		}
	}

	//Ataque do inimigo
	private IEnumerator EnemyAttack()
	{
		attackWarning.SetActive(true);
		yield return new WaitForSeconds(0.47f);
		isPunching = true;
		animator.SetBool("isPunching", true);
		if (!ninjaPlayer.isDodging && !ninjaPlayer.isBlocking && !wasDamaged && !gameManager.stopGame)
		{
			ninjaPlayer.TakeDamage(10);
			ninjaPlayer.animator.SetBool("wasDamaged", true);
			ninjaPlayer.wasDamaged = true;
			DecreasePosture(4);
			yield return new WaitForSeconds(0.4f);
			ninjaPlayer.animator.SetBool("wasDamaged", false);
			ninjaPlayer.wasDamaged = false;
		}
		else if(ninjaPlayer.isBlocking)
			ninjaPlayer.IncreasePosture(5);


		yield return new WaitForSeconds(0.25f);
		isPunching = false;
		attack = false;
		enemyActionAvaliable = true;
		animator.SetBool("isPunching", false);
		attackWarning.SetActive(false);
	}

	//Bloqueio
	private IEnumerator EnemyBlock()
	{
		isBlocking = true;
		animator.SetBool("isBlocking", true);
		yield return new WaitForSeconds(0.21f);
		isBlocking = false;
		animator.SetBool("isBlocking", false);
		defense = false;
		enemyActionAvaliable = true;
	}

	//Soco do inimigo
	public void EnemyTakeDamage(int damage)
	{
		enemyActionAvaliable = false;
		enemyCurrentHP -= damage;
		enemyHealthBar.SetHealth(enemyCurrentHP);
		StartCoroutine(EnemyDamageDelay(0.8f));
	}

	//Aumento de postura do inimigo
	public void EnemyIncreasePosture(int value)
	{
		enemyCurrentPosture += value;
		enemyPostureBar.SetPosture(enemyCurrentPosture);
	}

	//Diminuição da postura
	private void DecreasePosture(int value)
	{
		enemyCurrentPosture -= value;
		enemyPostureBar.SetPosture(enemyCurrentPosture);
	}

	//Delay depois de tomar dano
	private IEnumerator EnemyDamageDelay(float time)
	{
		yield return new WaitForSeconds(time);
		enemyActionAvaliable = true;
	}
}
