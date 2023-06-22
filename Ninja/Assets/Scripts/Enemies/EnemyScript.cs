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
	[SerializeField] private float timer;
	private bool attack = false;
	private bool defense = false;
	private bool enemyActionAvaliable;
	private bool enemyPostureReduction = false;
	private bool enemyPostureLimit = false;

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
	[SerializeField] public float enemyCurrentPosture;

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

			if(!gameManager.stopGame)
			{
				if (!attack && ninjaPlayer.isPunching && enemyActionAvaliable)
				{
					defense = true;
					enemyActionAvaliable = false;
					StartCoroutine(EnemyBlock());
				}

				yield return new WaitForSeconds(actionPeriod);

				if (defense == false && Random.Range(0, 100) > attackProbability && enemyActionAvaliable)
				{
					attack = true;
					enemyActionAvaliable = false;
					StartCoroutine(EnemyAttack());
				}
			}

			if (enemyPostureReduction)
			{
				timer -= Time.deltaTime;

				if (timer < 0)
				{
					if (enemyPostureLimit)
					{
						timer = 0;
						enemyCurrentPosture -= 8f * Time.deltaTime;
						enemyPostureBar.SetPosture(enemyCurrentPosture);

						if (enemyCurrentPosture <= 78)
							enemyPostureLimit = false;
					}
					else
					{
						timer = 0;
						enemyCurrentPosture -= 5.5f * Time.deltaTime;
						enemyPostureBar.SetPosture(enemyCurrentPosture);
					}
				}
			}
		}
	}

	private IEnumerator EnemyAttack() //Inimigo Golpe Fraco
	{
		attackWarning.SetActive(true);
		yield return new WaitForSeconds(0.47f);
		enemyPostureReduction = false;
		timer = 1.3f;
		isPunching = true;
		animator.SetBool("isPunching", true);
		if (!ninjaPlayer.isDodging && !ninjaPlayer.isBlocking && !wasDamaged && !gameManager.stopGame)
		{
			enemyPostureReduction = false;
			ninjaPlayer.TakeDamage(10);
			ninjaPlayer.animator.SetBool("wasDamaged", true);
			ninjaPlayer.wasDamaged = true;
			EnemyIncreasePosture(10);
			yield return new WaitForSeconds(0.4f);
			ninjaPlayer.animator.SetBool("wasDamaged", false);
			ninjaPlayer.wasDamaged = false;
		}
		else if (ninjaPlayer.isBlocking)
		{
			ninjaPlayer.IncreasePosture(6);
			enemyPostureReduction = true;
		}
			
		else if (ninjaPlayer.isDodging)
		{
			EnemyIncreasePosture(10);
			enemyPostureReduction = true;
		}
			
		yield return new WaitForSeconds(0.25f);
		isPunching = false;
		attack = false;
		enemyActionAvaliable = true;
		animator.SetBool("isPunching", false);
		attackWarning.SetActive(false);
	}

	private IEnumerator EnemyBlock() //Inimigo Bloqueio
	{
		EnemyIncreasePosture(6);
		enemyPostureReduction = false;
		timer = 1.3f;

		isBlocking = true;
		animator.SetBool("isBlocking", true);
		yield return new WaitForSeconds(0.21f);
		enemyPostureReduction = true;
		isBlocking = false;
		animator.SetBool("isBlocking", false);
		defense = false;
		enemyActionAvaliable = true;
	}

	//Soco toma dano
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
