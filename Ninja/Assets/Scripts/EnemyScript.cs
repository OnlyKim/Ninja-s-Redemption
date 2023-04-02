using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject enemyPositionTarget;
    [SerializeField] private GameObject enemyPunchTarget;
    [SerializeField] private GameObject enemyDodgeTarget;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float actionPeriod;
    [SerializeField] private int attackProbability;
	[SerializeField] private int defenseProbability;
	
	public int enemyHP;

    public int punchDamage;

	private int rand;
    public bool isPunching = false;
    public bool isBlocking = false;
	bool attack;
	bool defense;

	//   void Start()
	//{
	//       animator = GetComponent<Animator>();
	//}

	IEnumerator Start()
	{
		while (true)
		{
				yield return new WaitForSeconds(actionPeriod);

				bool attack = Random.Range(0, 100) > attackProbability;

				if (enemyHP <= 0)
					animator.SetBool("isDown", true);
				else if (attack)
					StartCoroutine(Rand());
		}
	}


	//void Update()
	//{
	//	StartCoroutine(Rand());
	//	StartCoroutine(Punch());
	//	StartCoroutine(Block());
	//}

	IEnumerator Rand()
	{
		
		
		int rand = Random.Range(0, 100);
		if (rand < 50)
		{
			isPunching = true;
			animator.SetBool("isPunching", true);
			if (FindObjectOfType<NinjaController>().isDodging == false)
				FindObjectOfType<NinjaController>().playerHP -= 5;
			yield return new WaitForSeconds(0.25f);
			isPunching = false;
			animator.SetBool("isPunching", false);
		}
		else
		{
			isBlocking = true;
			animator.SetBool("isBlocking", true);
			yield return new WaitForSeconds(0.25f);
			isBlocking = false;
			animator.SetBool("isBlocking", false);
		}
	}

	//IEnumerator Punch()
	//{
	//	isPunching = true;
	//	animator.SetBool("isPunching", true);
	//	yield return new WaitForSeconds(0.25f);
	//	isPunching = false;
	//	animator.SetBool("isPunching", false);
	//}

	//IEnumerator Block()
	//{
	//	isBlocking = true;
	//	animator.SetBool("isBlocking", true);
	//	yield return new WaitForSeconds(0.25f);
	//	isBlocking = false;
	//	animator.SetBool("isBlocking", false);
	//}




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
