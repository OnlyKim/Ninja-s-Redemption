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

	public int enemyHP;
	public int enemyMaxPosture;
    public int punchDamage;
    public bool isPunching = false;
    public bool isBlocking = false;
	public bool wasDamaged = false;
	public Animator animator;

	[HideInInspector] public int enemyCurrentHP;
	[HideInInspector] public int enemyCurrentPosture;

	private int rand;
	private float actionPeriod;
	private bool attack = false;
	private bool defense = false;
	private bool enemyActionAvaliable;

	IEnumerator Start()
	{
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

			if (attack == false && FindAnyObjectByType<NinjaController>().isPunching == true && FindAnyObjectByType<GamerManager>().stopGame == false && enemyActionAvaliable)
			{
				defense = true;
				enemyActionAvaliable = false;
				StartCoroutine(EnemyBlock());
			}

			yield return new WaitForSeconds(actionPeriod);

			if (defense == false && Random.Range(0, 100) > attackProbability && FindAnyObjectByType<GamerManager>().stopGame == false && enemyActionAvaliable)
			{
				attack = true;
				enemyActionAvaliable = false;
				StartCoroutine(EnemyAttack());
			}
		}
	}

	IEnumerator EnemyAttack()
	{
		attackWarning.SetActive(true);
		yield return new WaitForSeconds(0.35f);
		isPunching = true;
		animator.SetBool("isPunching", true);
		if (FindObjectOfType<NinjaController>().isDodging == false && FindObjectOfType<NinjaController>().isBlocking == false && wasDamaged == false && FindAnyObjectByType<GamerManager>().stopGame == false)
		{
			FindObjectOfType<NinjaController>().TakeDamage(10);
			FindObjectOfType<NinjaController>().animator.SetBool("wasDamaged", true);
			FindObjectOfType<NinjaController>().wasDamaged = true;
			DecreasePosture(4);
			yield return new WaitForSeconds(0.4f);
			FindObjectOfType<NinjaController>().animator.SetBool("wasDamaged", false);
			FindObjectOfType<NinjaController>().wasDamaged = false;
		}
		else if(FindObjectOfType<NinjaController>().isBlocking)
			FindObjectOfType<NinjaController>().IncreasePosture(5);


		yield return new WaitForSeconds(0.25f);
		isPunching = false;
		attack = false;
		enemyActionAvaliable = true;
		animator.SetBool("isPunching", false);
		attackWarning.SetActive(false);
	}

	IEnumerator EnemyBlock()
	{
		isBlocking = true;
		animator.SetBool("isBlocking", true);
		yield return new WaitForSeconds(0.21f);
		isBlocking = false;
		animator.SetBool("isBlocking", false);
		defense = false;
		enemyActionAvaliable = true;
	}

	public void EnemyTakeDamage(int damage)
	{
		enemyActionAvaliable = false;
		enemyCurrentHP -= damage;
		enemyHealthBar.SetHealth(enemyCurrentHP);
		StartCoroutine(EnemyDamageDelay(0.8f));
	}

	public void EnemyIncreasePosture(int value)
	{
		enemyCurrentPosture += value;
		enemyPostureBar.SetPosture(enemyCurrentPosture);
	}

	void DecreasePosture(int value)
	{
		enemyCurrentPosture -= value;
		enemyPostureBar.SetPosture(enemyCurrentPosture);
	}

	IEnumerator EnemyDamageDelay(float time)
	{
		yield return new WaitForSeconds(time);
		enemyActionAvaliable = true;
	}




	//IEnumerator Action()
	//{
	//	yield return new WaitForSeconds(actionPeriod);

	//	if (FindObjectOfType<NinjaController>().isPunching == true)
	//		StartCoroutine(EnemyBlock());

	//	bool attack = Random.Range(0, 100) < attackProbability;

	//}

	//IEnumerator EnemyBlock()
	//{
	//	bool defense = Random.Range(0, 10) > defenseProbability;

	//	if (defense)
	//	{
	//		isBlocking = true;
	//		animator.SetBool("isBlocking", true);
	//		yield return new WaitForSeconds(0.25f);
	//		animator.SetBool("isBlocking", false);
	//		isBlocking = false;
	//	}
	//}




	//void Start()
	//{
	//    animator = GetComponent<Animator>();
	//}


	//void Update()
	//{

	//}
}
