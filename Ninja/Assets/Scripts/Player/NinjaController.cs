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
	[SerializeField] private HealthBar healthBar;
    [SerializeField] private PostureBar postureBar;


    public int playerHP;
    public int playerMaxPosture;
    public bool isPunching = false;
    public bool isDodging = false;
    public bool isBlocking = false;

    [HideInInspector] public int currentHP;
    [HideInInspector] public int currentPosture;

    private bool actionAvaliable = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = playerHP;
        currentPosture = 0;
        healthBar.SetMaxHealth(playerHP);
        postureBar.SetMinPosture(0); 
    }

    void Update()
    {
        //Punch
        if(Input.GetKeyDown(KeyCode.R) && actionAvaliable)
            StartCoroutine(Punch());

        //Dodge
        if(Input.GetKeyDown(KeyCode.F) && actionAvaliable)
            StartCoroutine(Dodge());

        //Block
        if(Input.GetKey(KeyCode.V))
		{
            Block();
        }
        else if(Input.GetKeyUp(KeyCode.V))
		{
            StartCoroutine(Delay(0.15f));
        }

    }

    IEnumerator Punch()
    {
        actionAvaliable = false;
        isPunching = true;

        yield return new WaitForSeconds(0.15f);

        transform.position = Vector2.MoveTowards(transform.position, punchTarget.transform.position, movementSpeed);
        animator.SetBool("isPunching", true);

        if (FindObjectOfType<EnemyScript>().isBlocking == false)
            FindObjectOfType<EnemyScript>().EnemyTakeDamage(10);
        else if(FindObjectOfType<EnemyScript>().isBlocking == true)
                FindObjectOfType<EnemyScript>().EnemyIncreasePosture(5);



        yield return new WaitForSeconds(0.25f);
        animator.SetBool("isPunching", false);
        transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
        isPunching = false;

        StartCoroutine(Delay(0.23f));

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

        StartCoroutine(Delay(0.23f));
    }

    void Block()
	{
		actionAvaliable = false;
		isBlocking = true;
		animator.SetBool("isBlocking", true);
	}

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        actionAvaliable = true;
        animator.SetBool("isBlocking", false);
    }

    public void TakeDamage(int damage)
	{
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
	}

    public void IncreasePosture(int value)
	{
        currentPosture += value;
        postureBar.SetPosture(currentPosture);
	}


   
    
}
