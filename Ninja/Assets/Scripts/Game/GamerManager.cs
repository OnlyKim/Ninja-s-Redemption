using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GamerManager : MonoBehaviour
{
    public static GamerManager Instance; 
    // Start is called before the first frame update
    //void Start()
    //{

    //}

	private void Awake()
	{
        Instance = this;
	}

	// Update is called once per frame
	void Update()
    {
        if (FindObjectOfType<EnemyScript>().enemyHP <= 0)
        {
            Restart();
        }
    }

    public void Restart()
    {
        StartCoroutine(Delay());
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
