using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject positionTarget;
    [SerializeField] private GameObject punchTarget;
    [SerializeField] private GameObject dodgeTarget;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float delayTime;
	[SerializeField] private HealthBar healthBar;


    public int playerHP;
    public bool isPunching = false;
    public bool isDodging = false;

    [HideInInspector] public int currentHP;
    private bool actionAvaliable = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = playerHP;
        healthBar.SetMaxHealth(playerHP);
    }

    void Update()
    {
        //Punch
        if(Input.GetKeyDown(KeyCode.R) && !isDodging && actionAvaliable)
            StartCoroutine(Punch());


        //Block
        //if (Input.GetKeyDown(KeyCode.F))
        //    animator.SetBool("isBlocking", true);

        //Dodge
        if(Input.GetKeyDown(KeyCode.F) && !isPunching && actionAvaliable)
            StartCoroutine(Dodge());

        if(Input.GetKeyDown(KeyCode.V) && !isDodging)
		{
            Block();
		}

    }

    IEnumerator Punch()
    {
        actionAvaliable = false;
        isPunching = true;
        transform.position = Vector2.MoveTowards(transform.position, punchTarget.transform.position, movementSpeed);
        animator.SetBool("isPunching", true);
        if (FindObjectOfType<EnemyScript>().isBlocking == false)
            FindObjectOfType<EnemyScript>().EnemyTakeDamage(10);
            
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("isPunching", false);
        transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
        isPunching = false;

        StartCoroutine(Delay());

    }

    IEnumerator Dodge()
    {
        actionAvaliable = false;
        isDodging = true;
        transform.position = Vector2.MoveTowards(transform.position, dodgeTarget.transform.position, movementSpeed);
        animator.SetBool("isDodging", true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("isDodging", false);
        transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
        isDodging = false;

        StartCoroutine(Delay());
    }

    void Block()
	{

	}

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        actionAvaliable = true;
    }

    public void TakeDamage(int damage)
	{
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
	}


   
    
}
