using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private PostureBar postureBar;
    [SerializeField] private GameObject playerSPunchIndicator;
    [SerializeField] private float startTime;

    //Targets
    [SerializeField] private GameObject positionTarget;
    [SerializeField] private GameObject punchRightTarget;
    [SerializeField] private GameObject punchLeftTarget;
    [SerializeField] private GameObject dodgeRightTarget;
    [SerializeField] private GameObject dodgeLeftTarget;
    [SerializeField] private GameObject blockRightTarget;
    [SerializeField] private GameObject blockLeftTarget;

    private bool gamePaused;
    private EnemyScript enemy;
    private GamerManager gameManager;


    public int playerHP;
    public int playerMaxPosture;
    public bool isPunching = false;
    public bool isPunchingStrong = false;
    public bool isDodging = false;
    public bool isBlocking = false;
    public bool wasDamaged = false;

    public Animator animator;

    [HideInInspector] public int currentHP;
    [SerializeField] public float currentPosture;

    private bool actionAvaliable = true;

    public bool leftBlock = false;
    public bool rightBlock = false;
    private bool leftPunch = false;
    private bool rightPunch = false;
    public bool leftDodge = false;
    public bool rightDodge = false;
    private bool leftSPunch = false;
    private bool rightSPunch = false;

    private bool postureReduction = false;
    private bool postureLimit = false;

    private SpriteRenderer sprite;

    [SerializeField] private float timer;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHP = playerHP;
        currentPosture = 0;
        healthBar.SetMaxHealth(playerHP);
        postureBar.SetMinPosture(0);
        playerSPunchIndicator.SetActive(false);

        gameManager = FindAnyObjectByType<GamerManager>();
        enemy = FindAnyObjectByType<EnemyScript>();
    }

    private void Update()
    {
        playerSPunchIndicator.transform.position = new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z);

        if (currentPosture >= playerMaxPosture)
            postureLimit = true;

        if (!gameManager.stopGame && !postureLimit)
		{
            //Soco
            if (Input.GetKeyDown(KeyCode.E) && actionAvaliable && !wasDamaged) //Soco esquerda
			{
                leftPunch = true;
                isPunching = true;
                StartCoroutine(Punch());
            }
            else if (Input.GetKeyDown(KeyCode.R) && actionAvaliable && !wasDamaged) //Soco direita
			{
                rightPunch = true;
                isPunching = true;
                StartCoroutine(Punch());
            }


            //Soco Forte
            else if (Input.GetKeyDown(KeyCode.S) && actionAvaliable && !wasDamaged) //SocoF esquerda
            {
                leftSPunch = true;
                StartCoroutine(PunchStrong());
            }
            else if (Input.GetKeyDown(KeyCode.G) && actionAvaliable && !wasDamaged) //SocoF direita
            {
                rightSPunch = true;
                StartCoroutine(PunchStrong());
            }


            //Esquiva
            else if (Input.GetKeyDown(KeyCode.F) && actionAvaliable && !wasDamaged) //Esquiva direita
			{
                rightDodge = true;
                StartCoroutine(Dodge());
            }
            else if (Input.GetKeyDown(KeyCode.D) && actionAvaliable && !wasDamaged) //Esquiva esquerda
            {
                leftDodge = true;
                StartCoroutine(Dodge());
            }


            //Bloqueio
            else if (Input.GetKeyDown(KeyCode.V) && !wasDamaged && !leftBlock) //Bloqueio esquerda
            {
                rightBlock = true;
                StartCoroutine(Block());
            }


            //Bloqueio esquerda
            else if (Input.GetKeyDown(KeyCode.C) && !wasDamaged && !rightBlock) //Bloqueio direita
            {
                leftBlock = true;
                StartCoroutine(Block());
            }
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

    //Movimentos
    private IEnumerator Punch() //Soco normal
    {
        actionAvaliable = false;
        isPunching = true;
        postureReduction = false;
        timer = 1.2f;

        yield return new WaitForSeconds(0.12f);
        AudioManager.instance.PlaySFX("Damage");
        animator.SetBool("isPunching", true);
        

        if(leftPunch && !enemy.isBlocking) //Se o soco for para a esquerda...
		{
            transform.position = Vector2.MoveTowards(transform.position, punchRightTarget.transform.position, movementSpeed);
            IncreasePosture(10);
            enemy.EnemyTakeDamage(10);
            enemy.animator.SetBool("wasDamaged", true);
            AudioManager.instance.PlaySFX("TakingDamage");
            enemy.wasDamaged = true;
            yield return new WaitForSeconds(0.4f);
            postureReduction = true;
            enemy.animator.SetBool("wasDamaged", false);
            enemy.wasDamaged = false;
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            leftPunch = false;
        }
        else if(rightPunch && !enemy.isBlocking) //Se o soco for para a direita...
		{
            sprite.flipX = true;
            transform.position = Vector2.MoveTowards(transform.position, punchLeftTarget.transform.position, movementSpeed);
            IncreasePosture(10);
            enemy.EnemyTakeDamage(10);
            enemy.animator.SetBool("wasDamaged", true);
            AudioManager.instance.PlaySFX("TakingDamage");
            enemy.wasDamaged = true;
            yield return new WaitForSeconds(0.4f);
            postureReduction = true;
            enemy.animator.SetBool("wasDamaged", false);
            enemy.wasDamaged = false;
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            sprite.flipX = false;
            rightPunch = false;
        }

        else if (leftPunch && enemy.isBlocking) //Se soco for esquerda enquanto o inimigo estiver bloqueando...
		{
            postureReduction = true;
            enemy.EnemyIncreasePosture(15);
            AudioManager.instance.PlaySFX("Block");
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            leftPunch = false;
        }
        else if (rightPunch && enemy.isBlocking) //Se soco for direita enquanto o inimigo estiver bloqueando
        {
            postureReduction = true;
            enemy.EnemyIncreasePosture(15);
            AudioManager.instance.PlaySFX("Block");
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            sprite.flipX = false;
            rightPunch = false;
        }
        StartCoroutine(Delay(0.23f));
    }

    
    private IEnumerator PunchStrong() //Soco forte
    {
        actionAvaliable = false;
        postureReduction = false;
        timer = 1.2f;
        playerSPunchIndicator.SetActive(true);

        yield return new WaitForSeconds(0.10f);
        animator.SetBool("isPunching", true);

        if (leftSPunch && !enemy.isBlocking) //Se o soco for para a esquerda...
        {
            transform.position = Vector2.MoveTowards(transform.position, punchRightTarget.transform.position, movementSpeed);
            IncreasePosture(10);
            enemy.EnemyTakeDamage(10);
            enemy.animator.SetBool("wasDamaged", true);
            enemy.wasDamaged = true;
            yield return new WaitForSeconds(0.4f);
            postureReduction = true;
            enemy.animator.SetBool("wasDamaged", false);
            enemy.wasDamaged = false;
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            leftSPunch = false;
            playerSPunchIndicator.SetActive(false);
        }
        else if (rightSPunch && !enemy.isBlocking) //Se o soco for para a direita...
        {
            sprite.flipX = true;
            transform.position = Vector2.MoveTowards(transform.position, punchLeftTarget.transform.position, movementSpeed);
            IncreasePosture(10);
            enemy.EnemyTakeDamage(10);
            enemy.animator.SetBool("wasDamaged", true);
            enemy.wasDamaged = true;
            yield return new WaitForSeconds(0.4f);
            postureReduction = true;
            enemy.animator.SetBool("wasDamaged", false);
            enemy.wasDamaged = false;
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            sprite.flipX = false;
            rightSPunch = false;
            playerSPunchIndicator.SetActive(false);
        }



        else if (leftSPunch && enemy.isBlocking) //Se soco for esquerda enquanto o inimigo estiver bloqueando...
        {
            AudioManager.instance.PlaySFX("Block");
            postureReduction = true;
            enemy.EnemyIncreasePosture(15);
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            leftSPunch = false;
            playerSPunchIndicator.SetActive(false);
        }
        else if (rightSPunch && enemy.isBlocking) //Se soco for direita enquanto o inimigo estiver bloqueando
        {
            AudioManager.instance.PlaySFX("Block");
            postureReduction = true;
            enemy.EnemyIncreasePosture(15);
            yield return new WaitForSeconds(0.05f);
            animator.SetBool("isPunching", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isPunching = false;
            sprite.flipX = false;
            rightSPunch = false;
            playerSPunchIndicator.SetActive(false);
        }
        StartCoroutine(Delay(0.23f));
    }

    private IEnumerator Dodge()
    {
        IncreasePosture(3);
        postureReduction = false;
        timer = 1.2f;

        if(rightDodge) //Se esquiva for direita...
		{
            actionAvaliable = false;
            isDodging = true;
            transform.position = Vector2.MoveTowards(transform.position, dodgeRightTarget.transform.position, movementSpeed);
            sprite.flipX = true;
            //animator.SetBool("isDodging", true);
            yield return new WaitForSeconds(0.25f);
            postureReduction = true;
            //animator.SetBool("isDodging", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            sprite.flipX = false;
            isDodging = false;
            rightDodge = false;

            StartCoroutine(Delay(0.23f));
        }
        else if(leftDodge) //Se esquiva for esquerda...
        {
            actionAvaliable = false;
            isDodging = true;
            transform.position = Vector2.MoveTowards(transform.position, dodgeLeftTarget.transform.position, movementSpeed);
            //animator.SetBool("isDodging", true);
            yield return new WaitForSeconds(0.25f);
            postureReduction = true;
            //animator.SetBool("isDodging", false);
            transform.position = Vector2.MoveTowards(transform.position, positionTarget.transform.position, movementSpeed);
            isDodging = false;
            leftDodge = false;

            StartCoroutine(Delay(0.23f));
        }
    }

    private IEnumerator Block()
	{
        IncreasePosture(6);
        postureReduction = false;
        timer = 1.2f;
        
        if (leftBlock) //Bloquear para a esquerda
		{
            sprite.flipX = true;
            actionAvaliable = false;
            isBlocking = true;
            animator.SetBool("isBlocking", true);
            yield return new WaitForSeconds(0.8f);
            postureReduction = true;
            actionAvaliable = true;
            isBlocking = false;
            animator.SetBool("isBlocking", false);
            sprite.flipX = false;
            leftBlock = false;
            StartCoroutine(Delay(0.23f));
        }
        else if (rightBlock) //Bloquear para a direita
		{
            actionAvaliable = false;
            isBlocking = true;
            animator.SetBool("isBlocking", true);
            yield return new WaitForSeconds(0.8f);
            postureReduction = true;
            actionAvaliable = true;
            isBlocking = false;
            animator.SetBool("isBlocking", false);
            rightBlock = false;
            StartCoroutine(Delay(0.23f));
        }
    }


    //Delay
    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        actionAvaliable = true;
    }

    //Tomar dano
    public void TakeDamage(int damage)
	{
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
	}

    //Aumentar postura
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
