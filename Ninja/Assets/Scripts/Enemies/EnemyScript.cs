using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Animator animator;

	[SerializeField] private GameObject attackWarning;
	[SerializeField] private GameObject enemyPositionTarget;
    [SerializeField] private GameObject enemyPunchTarget;
    [SerializeField] private GameObject enemyDodgeTarget;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float minActionPeriod;
	[SerializeField] private float maxActionPeriod;
	[SerializeField] private int attackProbability;
	[SerializeField] private int defenseProbability;
	[SerializeField] private HealthBar healthBar;

	public int enemyHP;
    public int punchDamage;
    public bool isPunching = false;
    public bool isBlocking = false;

	[HideInInspector] public int enemyCurrentHP;

	private int rand;
	private float actionPeriod;
	private bool attack = false;
	private bool defense = false;

	//   void Start()
	//{
	//       animator = GetComponent<Animator>();
	//}

	IEnumerator Start()
	{
		enemyCurrentHP = enemyHP;
		attackWarning.SetActive(false);
		while (true)
		{
			actionPeriod = Random.Range(minActionPeriod, maxActionPeriod);
			yield return new WaitForSeconds(actionPeriod);

			if (enemyHP <= 0) //Inicia a anima��o do inimigo derrubado caso a vida dele chegue a 0
				animator.SetBool("isDown", true);
			else if (attack == false && FindAnyObjectByType<NinjaController>().isPunching == true)
			{
				defense = true;
				StartCoroutine(EnemyBlock());
			}
			else if (defense == false && Random.Range(0, 100) > attackProbability)
			{
				attack = true;
				StartCoroutine(EnemyAttack());
			}
				

			//bool attack = Random.Range(0, 100) > attackProbability;

			


			//else if (attack == true)
			//	StartCoroutine(EnemyAttack());
		}
	}


	//void Update()
	//{
	//	StartCoroutine(Rand());
	//	StartCoroutine(Punch());
	//	StartCoroutine(Block());
	//}

	IEnumerator EnemyAttack()
	{
		attackWarning.SetActive(true);
		yield return new WaitForSeconds(0.32f);
		isPunching = true;
		animator.SetBool("isPunching", true);
		if (FindObjectOfType<NinjaController>().isDodging == false)
			FindObjectOfType<NinjaController>().TakeDamage(10);
		yield return new WaitForSeconds(0.25f);
		isPunching = false;
		animator.SetBool("isPunching", false);
		attackWarning.SetActive(false);
	}

	//IEnumerator Punch()
	//{
	//	isPunching = true;
	//	animator.SetBool("isPunching", true);
	//	yield return new WaitForSeconds(0.25f);
	//	isPunching = false;
	//	animator.SetBool("isPunching", false);
	//}

	IEnumerator EnemyBlock()
	{
		isBlocking = true;
		animator.SetBool("isBlocking", true);
		yield return new WaitForSeconds(0.25f);
		isBlocking = false;
		animator.SetBool("isBlocking", false);
		defense = false;
	}

	public void EnemyTakeDamage(int damage)
	{
		enemyCurrentHP -= damage;
		healthBar.SetHealth(enemyCurrentHP);
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