using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaSpriteAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ninja;
    [SerializeField] private Animator animator;

    [SerializeField] private bool isLeft;
    //[SerializeField] private bool punchLeft;
    [SerializeField] private bool punchRight;
    [SerializeField] private bool dodge;
    //[SerializeField] private bool blockLeft;
    [SerializeField] private bool blockRight;

	private void Start()
	{
        if (isLeft)
            ninja.flipX = true;
	}

	private void Update()
	{
        if (punchRight)
        {
            animator.SetBool("isPunching", true);
            StartCoroutine(AnimationDelay());

        }
        else if (dodge)
		{
            animator.SetBool("isDodging", true);
            StartCoroutine(AnimationDelay());
        }
        else if (blockRight)
		{
            animator.SetBool("isBlocking", true);
            StartCoroutine(AnimationDelay());
		}
    }

	IEnumerator AnimationDelay()
	{
        yield return new WaitForSeconds(2f);

        if (punchRight)
        {
            animator.SetBool("isPunching", false);

        }
        else if (dodge)
        {
            animator.SetBool("isDodging", false);
        }
        else if (blockRight)
        {
            animator.SetBool("isBlocking", false);
        }

        yield return new WaitForSeconds(2f);
    }
}
