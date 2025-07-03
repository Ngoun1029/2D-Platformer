using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public TMP_Text levelText;
    [SerializeField] string level;

    public GameObject mathPanel;
    private TMP_Text mathQuestionText;
    private Button yesButton;
    private Button noButton;

    [SerializeField] private PlayerMovement player;
    [SerializeField] private Health playerHealth;
    public TMP_Text resultText;
    public Button restartButton;
    public Button quitButton;
    public Button nextButton;
    public bool hasNext = true;


    private bool isCorrect;
    private int correctCount = 0;

    private void Start()
    {

        if (level == "0")
        {
            nextButton.gameObject.SetActive(true);
            nextButton.gameObject.GetComponentInChildren<TMP_Text>().text = "Start Game";
            quitButton.onClick.AddListener(QuitGame);

            levelText.text = "Welcome To Math Game";
        }
        else
        {
            playerHealth = player.GetComponent<Health>();
            noButton = mathPanel.transform.Find("ButtonNo").GetComponent<Button>();
            yesButton = mathPanel.transform.Find("ButtonYes").GetComponent<Button>();
            mathQuestionText = mathPanel.transform.Find("Question").GetComponent<TMP_Text>();


            // Set level text and start the Coroutine to hide it
            if (hasNext)
            {
                levelText.text = "Level " + level;
            }
            else
            {
                levelText.text = "Final Level";
            }
            StartCoroutine(HideLevelTextAfterSeconds(3));

            nextButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            yesButton.onClick.AddListener(AnswerYes);
            noButton.onClick.AddListener(AnswerNo);
        }



        // Make the panel invisible by default
        mathPanel.SetActive(false);
        resultText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);


        // Attach the methods to the buttons' onClick events
        restartButton.onClick.AddListener(RestartGame);
        nextButton.onClick.AddListener(NextLevel);
    }

    private IEnumerator HideLevelTextAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        levelText.gameObject.SetActive(false);
    }

    public void AnswerYes()
    {
        AnswerQuestion(isCorrect);
    }

    public void AnswerNo()
    {
        AnswerQuestion(!isCorrect);
    }

    public void RestartGame()
    {
        // Reloads the current scene, effectively restarting the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // Quits the game. Note that this will not work in the Unity Editor, so to test it you will need to build and run your game
        Application.Quit();
    }

    public void NextLevel()
    {
        // Load next level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void MainMenu()
    {
        // Load main menu
        SceneManager.LoadScene(0);
    }

    public void ShowMathQuestion(string question, bool correct)
    {
        mathQuestionText.text = question;
        isCorrect = correct;
        mathPanel.SetActive(true);
    }

    private void AnswerQuestion(bool wasCorrect)
    {
        if (wasCorrect)
        {
            correctCount++;
            if (correctCount >= 3)
            {
                //player.EnableMovement();
                mathPanel.SetActive(false);
                quitButton.gameObject.GetComponentInChildren<TMP_Text>().text = "Main Menu";
                quitButton.onClick.AddListener(MainMenu);
                quitButton.gameObject.SetActive(true);
                if (hasNext)
                {
                    nextButton.gameObject.SetActive(true);
                    resultText.gameObject.SetActive(true);
                    resultText.text = "You won!";
                }
                else
                {
                    resultText.gameObject.SetActive(true);
                    resultText.text = "You Cleared All Levels!";
                }
                return;
            }
        }
        else
        {
            playerHealth.TakeDamage(1f);
        }
        if(playerHealth.currentHealth > 0)
        {
            // Neither win nor lose condition was met, ask another question
            GenerateNewQuestion();
        }

    }

    public void LoseScreen()
    {
        player.DisableMovement();
        mathPanel.SetActive(false);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.GetComponentInChildren<TMP_Text>().text = "Main Menu";
        quitButton.onClick.AddListener(MainMenu);
        quitButton.gameObject.SetActive(true);
        resultText.gameObject.SetActive(true);
        resultText.text = "You lost!";
        return;
    }

    private void GenerateNewQuestion()
    {
        int maxNum = 10 * Int32.Parse(level);
        // Generate two random numbers
        int number1 = UnityEngine.Random.Range(1, maxNum);
        int number2 = UnityEngine.Random.Range(1, maxNum);

        // Randomly decide between addition and subtraction
        if (UnityEngine.Random.value < 0.5)
        {
            // Addition
            int randomOffset = UnityEngine.Random.Range(-1, 2);
            ShowMathQuestion($"{number1} + {number2} = {number1 + number2 + randomOffset}", randomOffset == 0);
        }
        else
        {
            // Subtraction, ensure number1 is not smaller than number2
            if (number1 < number2)
            {
                int temp = number1;
                number1 = number2;
                number2 = temp;
            }
            int randomOffset = UnityEngine.Random.Range(-1, 2);
            ShowMathQuestion($"{number1} - {number2} = {number1 - number2 + randomOffset}", randomOffset == 0);
        }
    }


}
