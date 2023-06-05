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
    [SerializeField] public float currentPosture;

    private bool actionAvaliable = true;
    private bool leftBlock = false;
    private bool rightBlock = false;

    private float coef;
    private bool postureReduction = false;
    private bool postureLimit = false;
    [SerializeField] private float timer;

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
        if (currentPosture >= playerMaxPosture)
            postureLimit = true;

        if (!gameManager.stopGame && !postureLimit)
		{
            //Punch
            if (Input.GetKeyDown(KeyCode.R) && actionAvaliable && !wasDamaged) //Soco
                StartCoroutine(Punch());

            //Dodge
            if (Input.GetKeyDown(KeyCode.F) && actionAvaliable && !wasDamaged) //Esquiva
                StartCoroutine(Dodge());

            //Bloqueio direita
            if (Input.GetKeyDown(KeyCode.V) && !wasDamaged && !leftBlock) //Bloqueio
            {
                rightBlock = true;
                StartCoroutine(Block());
            }

            //else if (Input.GetKeyUp(KeyCode.V) && !leftBlock) //Delay do Bloqueio ****
            //{
            //    StartCoroutine(Delay(0.15f));
            //}

            //Bloqueio esquerda
            if (Input.GetKeyDown(KeyCode.C) && !wasDamaged && !rightBlock) //Bloqueio
            {
                leftBlock = true;
                StartCoroutine(Block());
            }

            //else if (Input.GetKeyUp(KeyCode.C) && !rightBlock) //Delay do Bloqueio ****
            //{
            //    StartCoroutine(Delay(0.15f));
            //}
        }
        if (postureReduction)
        {

            timer -= Time.deltaTime;

            if (timer < 0)
            {
                if(postureLimit)
				{
                    timer = 0;
                    currentPosture -= 8f * Time.deltaTime;
                    postureBar.SetPosture(currentPosture);

                    if (currentPosture <= 78)
                        postureLimit = false;
                }
                else
				{
                    timer = 0;
                    currentPosture -= 5.5f * Time.deltaTime;
                    postureBar.SetPosture(currentPosture);
                }
                
            }
        }
    }

    private IEnumerator Punch()
    {
        actionAvaliable = false;
        isPunching = true;
        postureReduction = false;
        timer = 1.2f;

        yield return new WaitForSeconds(0.10f);

        transform.position = Vector2.MoveTowards(transform.position, punchTarget.transform.position, movementSpeed);
        animator.SetBool("isPunching", true);

        if (!enemy.isBlocking) //******PASSAR PARA O INIMIGO ******
        {
            IncreasePosture(10);
            enemy.EnemyTakeDamage(10);
            enemy.animator.SetBool("wasDamaged", true);
            enemy.wasDamaged = true;
            yield return new WaitForSeconds(0.4f);
            postureReduction = true;
            enemy.animator.SetBool("wasDamaged", false);
            enemy.wasDamaged = false;
        }

        else if (enemy.isBlocking)
            postureReduction = true;
            enemy.EnemyIncreasePosture(15);

        yield return new WaitForSeconds(0.05f);
        animator.SetBool("isPunching", false);
        transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
        isPunching = false;

        StartCoroutine(Delay(0.23f));

    }

    private IEnumerator Dodge()
    {
        IncreasePosture(3);
        postureReduction = false;
        timer = 1.2f;

        actionAvaliable = false;
        isDodging = true;
        transform.position = Vector2.MoveTowards(transform.position, dodgeTarget.transform.position, movementSpeed);
        animator.SetBool("isDodging", true);
        yield return new WaitForSeconds(0.25f);
        postureReduction = true;
        animator.SetBool("isDodging", false);
        transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
        isDodging = false;

        StartCoroutine(Delay(0.23f));
    }

    private IEnumerator Block()
	{
        IncreasePosture(6);
        postureReduction = false;
        timer = 1.2f;
        
        if (rightBlock)
		{
            actionAvaliable = false;
            isBlocking = true;
            animator.SetBool("isBlocking", true);
            yield return new WaitForSeconds(0.6f);
            postureReduction = true;
            actionAvaliable = true;
            isBlocking = false;
            animator.SetBool("isBlocking", false);
            rightBlock = false;
            StartCoroutine(Delay(0.23f));

        }
        else if (leftBlock)
		{
            sprite.flipX = true;
            actionAvaliable = false;
            isBlocking = true;
            animator.SetBool("isBlocking", true);
            yield return new WaitForSeconds(0.6f);
            postureReduction = true;
            actionAvaliable = true;
            isBlocking = false;
            animator.SetBool("isBlocking", false);
            sprite.flipX = false;
            leftBlock = false;
            StartCoroutine(Delay(0.23f));
        }
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        actionAvaliable = true;
        //      if(leftBlock)
        //{
        //          yield return new WaitForSeconds(time);
        //          actionAvaliable = true;
        //          isBlocking = false;
        //          animator.SetBool("isBlocking", false);
        //          sprite.flipX = false;
        //          leftBlock = false;
        //          StartCoroutine(DecreasePosture(2f));
        //      }
        //      else
        //{
        //          yield return new WaitForSeconds(time);
        //          actionAvaliable = true;
        //          isBlocking = false;
        //          animator.SetBool("isBlocking", false);
        //          rightBlock = false;
        //          StartCoroutine(DecreasePosture(2f));
        //      }

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

        if(currentPosture == playerMaxPosture)
		{
            postureLimit = true;
		}
	}
}
