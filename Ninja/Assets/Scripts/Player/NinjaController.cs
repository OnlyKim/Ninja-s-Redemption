using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject positionTarget;
    [SerializeField] private GameObject punchTarget;
    [SerializeField] private GameObject dodgeTarget;
    [SerializeField] private float movementSpeed;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private PostureBar postureBar;
    [SerializeField] private float startTime;

    private bool gamePaused;
    private EnemyScript enemy;
    private GamerManager gameManager;


    public int playerHP;
    public int playerMaxPosture;
    public bool isPunching = false;
    public bool isDodging = false;
    public bool isBlocking = false;
    public bool wasDamaged = false;

    public Animator animator;

    [HideInInspector] public int currentHP;
    [HideInInspector] public float currentPosture;

    private bool actionAvaliable = true;
    private bool leftBlock = false;
    private bool rightBlock = false;

    private float coef;
    private bool postureReduction = false;
    private bool postureLimit = true;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHP = playerHP;
        currentPosture = 0;
        healthBar.SetMaxHealth(playerHP);
        postureBar.SetMinPosture(0);

        gameManager = FindAnyObjectByType<GamerManager>();
        enemy = FindAnyObjectByType<EnemyScript>();
    }

    private void Update()
    {
        if(!gameManager.stopGame && !postureLimit)
		{
            //Punch
            if (Input.GetKeyDown(KeyCode.R) && actionAvaliable && !wasDamaged) //Soco
                StartCoroutine(Punch());

            //Dodge
            if (Input.GetKeyDown(KeyCode.F) && actionAvaliable && !wasDamaged) //Esquiva
                StartCoroutine(Dodge());

            //Bloqueio direita
            if (Input.GetKey(KeyCode.V) && !wasDamaged && !leftBlock) //Bloqueio
            {
                rightBlock = true;
                Block();
            }
            else if (Input.GetKeyUp(KeyCode.V) && !leftBlock) //Delay do Bloqueio ****
            {
                StartCoroutine(Delay(0.15f));
            }

            //Bloqueio esquerda
            if (Input.GetKey(KeyCode.C) && !wasDamaged && !rightBlock) //Bloqueio
            {
                leftBlock = true;
                Block();
            }
            else if (Input.GetKeyUp(KeyCode.C) && !rightBlock) //Delay do Bloqueio ****
            {
                StartCoroutine(Delay(0.15f));
            }
        }
        
    }

    private IEnumerator Punch()
    {
        actionAvaliable = false;
        isPunching = true;

        yield return new WaitForSeconds(0.10f);

        transform.position = Vector2.MoveTowards(transform.position, punchTarget.transform.position, movementSpeed);
        animator.SetBool("isPunching", true);

        if (!enemy.isBlocking) //******PASSAR PARA O INIMIGO ******
        {
            postureReduction = false;
            IncreasePosture(10);
            enemy.EnemyTakeDamage(10);
            enemy.animator.SetBool("wasDamaged", true);
            enemy.wasDamaged = true;
            postureReduction = true;
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(DecreasePosture(4f));
            enemy.animator.SetBool("wasDamaged", false);
            enemy.wasDamaged = false;
        }

        else if (enemy.isBlocking)
            enemy.EnemyIncreasePosture(15);

        yield return new WaitForSeconds(0.05f);
        animator.SetBool("isPunching", false);
        transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
        isPunching = false;

        StartCoroutine(Delay(0.23f));

    }

    private IEnumerator Dodge()
    {
        IncreasePosture(2);
        postureReduction = false;
        actionAvaliable = false;
        isDodging = true;
        transform.position = Vector2.MoveTowards(transform.position, dodgeTarget.transform.position, movementSpeed);
        animator.SetBool("isDodging", true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("isDodging", false);
        transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
        isDodging = false;
        StartCoroutine(DecreasePosture(1f));

        StartCoroutine(Delay(0.23f));
    }

    private void Block()
	{
        if (rightBlock)
		{
            postureReduction = false;
            actionAvaliable = false;
            isBlocking = true;
            animator.SetBool("isBlocking", true);
        }
        else if (leftBlock)
		{
            postureReduction = false;
            sprite.flipX = true;
            actionAvaliable = false;
            isBlocking = true;
            animator.SetBool("isBlocking", true);
        }
    }

    private IEnumerator Delay(float time)
    {
        if(leftBlock)
		{
            yield return new WaitForSeconds(time);
            actionAvaliable = true;
            isBlocking = false;
            animator.SetBool("isBlocking", false);
            sprite.flipX = false;
            leftBlock = false;
            StartCoroutine(DecreasePosture(2f));
        }
        else
		{
            yield return new WaitForSeconds(time);
            actionAvaliable = true;
            isBlocking = false;
            animator.SetBool("isBlocking", false);
            rightBlock = false;
            StartCoroutine(DecreasePosture(2f));
        }
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

    private IEnumerator DecreasePosture(float value)
	{
        yield return new WaitForSeconds(2f);
        postureReduction = true;
        while (postureReduction && currentPosture > 0f)
		{
            yield return new WaitForSeconds(0.9f);
            currentPosture -= value;
            postureBar.SetPosture(currentPosture);
        }
    }

 //   IEnumerator DecreasePosture2(int value)
	//{
 //       yield return new WaitForSeconds(1.4f);

 //       currentPosture -= value;
 //       postureBar.SetPosture(currentPosture);

 //   }




}
