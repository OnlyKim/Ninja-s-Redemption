using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	//[SerializeField] private GameObject attackWarning;
	[SerializeField] private GameObject rightAttackWarning;
	[SerializeField] private GameObject leftAttackWarning;
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
	private bool rightPrevious = false;
	private bool postureLimit = false;
	private bool enemyPostureReduction = false;
	private float timer;

	private int punchSideDecision;
	private int rightPunchSideDecision = 50;
	//private int leftPunchSideDecision = 100;

	private NinjaController ninjaPlayer;
	private GamerManager gameManager;
	private SpriteRenderer enemySprite;

	public int enemyHP;
	public int enemyMaxPosture;
    public int punchDamage;
    public bool isPunching = false;
    public bool isBlocking = false;
	public bool wasDamaged = false;
	public bool isCharging = false;

	public bool rightPunch = false;
	public bool leftPunch = false;

	public Animator animator;

	[HideInInspector] public int enemyCurrentHP;
	[HideInInspector] public float enemyCurrentPosture;

	private void Awake()
	{
		enemySprite = GetComponent<SpriteRenderer>();
		ninjaPlayer = FindAnyObjectByType<NinjaController>();
		gameManager = FindAnyObjectByType<GamerManager>();
		enemyCurrentHP = enemyHP;
		enemyActionAvaliable = true;
		enemyCurrentPosture = 0;

		rightAttackWarning.SetActive(false);
		leftAttackWarning.SetActive(false);

		StartCoroutine(EnemyRoutine());
	}

	private IEnumerator EnemyRoutine()
	{
		if (enemyCurrentPosture >= enemyMaxPosture)
		{
			postureLimit = true;
			enemyCurrentPosture = enemyMaxPosture;
		}


		if (!gameManager.stopGame && !postureLimit)
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
		if (enemyPostureReduction)
		{
			timer -= Time.deltaTime;

			if (timer < 0)
			{
				if (postureLimit)
				{
					timer = 0;
					enemyCurrentPosture -= 8f * Time.deltaTime;
					enemyPostureBar.SetPosture(enemyCurrentPosture);

					if (enemyCurrentPosture <= 78)
						postureLimit = false;
				}
				else
				{
					timer = 0;
					enemyCurrentPosture -= 5.5f * Time.deltaTime;
					enemyPostureBar.SetPosture(enemyCurrentPosture);
				}
			}
		}

		StartCoroutine(EnemyRoutine());
	}

	//Ataque do inimigo
	private IEnumerator EnemyAttack()
	{
		punchSideDecision = Random.Range(0, 100);
		enemyPostureReduction = false;
		timer = 1.2f;

		if (punchSideDecision <= rightPunchSideDecision) //Ataque direita
		{
			rightPunchSideDecision -= 5;
			isCharging = true;
			rightAttackWarning.SetActive(true);
			yield return new WaitForSeconds(0.48f);
			isCharging = false;
			rightAttackWarning.SetActive(false);
			isPunching = true;
			animator.SetBool("isPunching", true);

			if (!ninjaPlayer.isDodging && !ninjaPlayer.isBlocking && !ninjaPlayer.rightBlock && !wasDamaged && !gameManager.stopGame)
			{
				rightPunch = true;
				EnemyIncreasePosture(10);
				enemySprite.flipX = true;
				ninjaPlayer.TakeDamage(10);
				ninjaPlayer.animator.SetBool("wasDamaged", true);
				ninjaPlayer.wasDamaged = true;
				DecreasePosture(4);

				yield return new WaitForSeconds(0.32f);

				ninjaPlayer.animator.SetBool("wasDamaged", false);
				ninjaPlayer.wasDamaged = false;

				yield return new WaitForSeconds(0.25f);

				enemyPostureReduction = true;
				enemySprite.flipX = false;
				isPunching = false;
				rightPunch = false;
				enemyActionAvaliable = true;
				animator.SetBool("isPunching", false);
				attack = false;
			}
			else if (ninjaPlayer.isBlocking && ninjaPlayer.rightBlock)
			{
				rightPunch = true;
				EnemyIncreasePosture(10);
				enemySprite.flipX = true;
				ninjaPlayer.IncreasePosture(5);
				AudioManager.instance.PlaySFX("Block");

				yield return new WaitForSeconds(0.25f);

				enemyPostureReduction = true;
				enemySprite.flipX = false;
				isPunching = false;
				rightPunch = false;
				enemyActionAvaliable = true;
				animator.SetBool("isPunching", false);
				attack = false;
			}
			else if (ninjaPlayer.isBlocking && ninjaPlayer.leftBlock)
			{
				rightPunch = true;
				EnemyIncreasePosture(10);
				enemySprite.flipX = true;
				ninjaPlayer.TakeDamage(10);
				AudioManager.instance.PlaySFX("TakingDamage");
				ninjaPlayer.animator.SetBool("isBlocking", false);
				ninjaPlayer.animator.SetBool("wasDamaged", true);

				yield return new WaitForSeconds(0.25f);

				enemyPostureReduction = true;
				ninjaPlayer.animator.SetBool("wasDamaged", false);
				enemySprite.flipX = false;
				isPunching = false;
				rightPunch = false;
				enemyActionAvaliable = true;
				animator.SetBool("isPunching", false);
				attack = false;
			}
		}


		else if(punchSideDecision > rightPunchSideDecision) //Ataque esquerda
		{
			rightPunchSideDecision += 5;
			isCharging = true;
			leftAttackWarning.SetActive(true);
			yield return new WaitForSeconds(0.48f);
			isCharging = false;
			leftAttackWarning.SetActive(false);
			isPunching = true;
			animator.SetBool("isPunching", true);

			if (!ninjaPlayer.isDodging && !ninjaPlayer.isBlocking && !ninjaPlayer.leftBlock && !wasDamaged && !gameManager.stopGame)
			{
				leftPunch = true;
				EnemyIncreasePosture(10);
				ninjaPlayer.TakeDamage(10);
				ninjaPlayer.animator.SetBool("wasDamaged", true);
				ninjaPlayer.wasDamaged = true;
				DecreasePosture(4);

				yield return new WaitForSeconds(0.35f);

				ninjaPlayer.animator.SetBool("wasDamaged", false);
				ninjaPlayer.wasDamaged = false;

				yield return new WaitForSeconds(0.25f);

				enemyPostureReduction = true;
				isPunching = false;
				leftPunch = false;
				enemyActionAvaliable = true;
				animator.SetBool("isPunching", false);
				attack = false;
			}
			else if (ninjaPlayer.isBlocking && ninjaPlayer.leftBlock)
			{
				leftPunch = true;
				EnemyIncreasePosture(10);
				ninjaPlayer.IncreasePosture(5);
				AudioManager.instance.PlaySFX("Block");

				yield return new WaitForSeconds(0.5f);

				enemyPostureReduction = true;
				isPunching = false;
				leftPunch = false;
				enemyActionAvaliable = true;
				animator.SetBool("isPunching", false);
				attack = false;
			}
			else if (ninjaPlayer.isBlocking && ninjaPlayer.rightBlock)
			{
				leftPunch = true;
				EnemyIncreasePosture(10);
				ninjaPlayer.TakeDamage(10);
				AudioManager.instance.PlaySFX("TakingDamage");
				ninjaPlayer.animator.SetBool("isBlocking", false);
				ninjaPlayer.animator.SetBool("wasDamaged", true);

				yield return new WaitForSeconds(0.25f);

				enemyPostureReduction = true;
				ninjaPlayer.animator.SetBool("wasDamaged", false);
				isPunching = false;
				leftPunch = false;
				enemyActionAvaliable = true;
				animator.SetBool("isPunching", false);
				attack = false;
			}
		}
	}

	//Bloqueio
	private IEnumerator EnemyBlock()
	{
		enemyPostureReduction = false;
		timer = 1.2f;
		isBlocking = true;
		animator.SetBool("isBlocking", true);
		yield return new WaitForSeconds(0.21f);
		enemyPostureReduction = true;
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
		StartCoroutine(EnemyDamageDelay(1.2f));
	}

	//Aumento de postura do inimigo
	public void EnemyIncreasePosture(int value)
	{
		enemyCurrentPosture += value;
		enemyPostureBar.SetPosture(enemyCurrentPosture);

		if (enemyCurrentPosture == enemyMaxPosture)
		{
			postureLimit = true;
		}
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
