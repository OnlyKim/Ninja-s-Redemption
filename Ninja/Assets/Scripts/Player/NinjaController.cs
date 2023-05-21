using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField] private GameObject positionTarget;
    [SerializeField] private GameObject punchTarget;
    [SerializeField] private GameObject dodgeTarget;
    [SerializeField] private float movementSpeed;
	[SerializeField] private HealthBar healthBar;
    [SerializeField] private PostureBar postureBar;
	[SerializeField] private float startTime;


    public int playerHP;
    public int playerMaxPosture;
    public bool isPunching = false;
    public bool isDodging = false;
    public bool isBlocking = false;
    public bool wasDamaged = false;

    public Animator animator;

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
        if(Input.GetKeyDown(KeyCode.R) && actionAvaliable && wasDamaged == false && FindAnyObjectByType<GamerManager>().stopGame == false)
            StartCoroutine(Punch());

        //Dodge
        if(Input.GetKeyDown(KeyCode.F) && actionAvaliable && wasDamaged == false && FindAnyObjectByType<GamerManager>().stopGame == false)
            StartCoroutine(Dodge());

        //Block
        if(Input.GetKey(KeyCode.V) && wasDamaged == false && FindAnyObjectByType<GamerManager>().stopGame == false)
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

        if (FindObjectOfType<EnemyScript>().isBlocking == false )
		{
            FindObjectOfType<EnemyScript>().EnemyTakeDamage(10);
            FindObjectOfType<EnemyScript>().animator.SetBool("wasDamaged", true);
            FindObjectOfType<EnemyScript>().wasDamaged = true;
            DecreasePosture(2);
            yield return new WaitForSeconds(0.4f);
            FindObjectOfType<EnemyScript>().animator.SetBool("wasDamaged", false);
            FindObjectOfType<EnemyScript>().wasDamaged = false;
            
        }
            
        else if(FindObjectOfType<EnemyScript>().isBlocking == true)
                FindObjectOfType<EnemyScript>().EnemyIncreasePosture(15);



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
        isBlocking = false;
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

 //   public void DecreasePosture(int value)
	//{
 //       currentPosture -= value;
 //       postureBar.SetPosture(currentPosture);
 //   }

    public void DecreasePosture(int value)
	{
        currentPosture -= value;
        postureBar.SetPosture(currentPosture);
    }

 //   IEnumerator DecreasePosture2(int value)
	//{
 //       yield return new WaitForSeconds(1.4f);

 //       currentPosture -= value;
 //       postureBar.SetPosture(currentPosture);

 //   }




}
