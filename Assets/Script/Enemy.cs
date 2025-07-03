using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerMovement player;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        // Get the Rigidbody2D component from the player GameObject
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // generate a math question
            int a = Random.Range(1, 10);
            int b = Random.Range(1, 10);
            int result = a - b;

            // randomly decide whether to give the correct answer or not
            bool correct = (Random.value > 0.5f);
            if (!correct)
            {
                // deliberately give a wrong result
                result += Random.Range(1, 3);  // add 1 or 2 to the result
            }

            string question = string.Format("{0} - {1} = {2}", a, b, result);

            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            player.DisableMovement();
            // send the question and the correctness of the answer to the UIManager
            uiManager.ShowMathQuestion(question, correct);
        }
    }
}
