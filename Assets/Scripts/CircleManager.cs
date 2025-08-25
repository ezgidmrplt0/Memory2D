using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CircleManager : MonoBehaviour
{
    public GameObject[] circles;
    private Color[] originalColors;

    private List<int> currentSequence = new List<int>();
    private int playerIndex = 0;
    private int wrongCount = 0;
    private int correctCount = 0; // Tur bazlı doğru
    private int totalCorrectCount = 0; // Tüm oyun boyunca toplam doğru

    [Header("UI")]
    public TextMeshProUGUI wrongText;
    public TextMeshProUGUI correctText;

    [Header("Zorluk Ayarları")]
    public float baseDelay = 0.5f;
    public float minDelay = 0.2f;
    private float currentDelay;

    // input kilidi
    [HideInInspector] public bool canClick = false;

    void Start()
    {
        currentDelay = baseDelay;

        originalColors = new Color[circles.Length];
        for (int i = 0; i < circles.Length; i++)
        {
            originalColors[i] = circles[i].GetComponent<SpriteRenderer>().color;
            circles[i].GetComponent<SpriteRenderer>().color = Color.gray;
        }

        UpdateUI();
        GenerateNewSequence();
        StartCoroutine(ShowSequence(currentSequence.ToArray()));
    }

    void GenerateNewSequence()
    {
        currentSequence.Clear();

        int length = Random.Range(3, circles.Length + 1);
        for (int i = 0; i < length; i++)
        {
            int rand = Random.Range(0, circles.Length);
            currentSequence.Add(rand);
        }
    }

    IEnumerator ShowSequence(int[] order)
    {
        canClick = false; // tıklama kilitli

        for (int i = 0; i < circles.Length; i++)
            circles[i].GetComponent<SpriteRenderer>().color = Color.gray;

        yield return new WaitForSeconds(1f);

        foreach (int index in order)
        {
            circles[index].GetComponent<SpriteRenderer>().color = originalColors[index];
            yield return new WaitForSeconds(currentDelay);

            circles[index].GetComponent<SpriteRenderer>().color = Color.gray;
            yield return new WaitForSeconds(currentDelay);
        }

        for (int i = 0; i < circles.Length; i++)
            circles[i].GetComponent<SpriteRenderer>().color = originalColors[i];

        playerIndex = 0;
        UpdateUI();

        canClick = true; // sıra gösterildi, artık tıklanabilir
    }

    public void CheckPlayerChoice(int index)
    {
        if (!canClick) return; // sırada tıklama engeli
        if (playerIndex >= currentSequence.Count) return;

        if (index == currentSequence[playerIndex])
        {
            playerIndex++;

            if (playerIndex >= currentSequence.Count)
            {
                correctCount++;
                totalCorrectCount++; // Tüm oyun boyunca toplam doğru sayıyı arttır

                // ScoreManager'a toplam doğru sayıyı gönder
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                    scoreManager.AddScore(totalCorrectCount);

                // Her 5 doğru sonrasında hızlan
                if (correctCount % 5 == 0 && currentDelay > minDelay)
                {
                    currentDelay -= 0.1f;
                    if (currentDelay < minDelay)
                        currentDelay = minDelay;
                }

                UpdateUI();
                StartCoroutine(NewRound());
            }
        }
        else
        {
            wrongCount++;
            UpdateUI();
            SaveScore(); // Oyun bitti, skoru kaydet

            playerIndex = 0;
            StartCoroutine(ShowSequence(currentSequence.ToArray()));
        }
    }

    IEnumerator NewRound()
    {
        yield return new WaitForSeconds(1f);

        GenerateNewSequence();
        yield return StartCoroutine(ShowSequence(currentSequence.ToArray()));
    }

    void UpdateUI()
    {
        if (wrongText != null)
            wrongText.text = "Yanlış: " + wrongCount;

        if (correctText != null)
            correctText.text = "Doğru: " + correctCount;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("LastScore", totalCorrectCount);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        // Oyun bittiğinde veya sahne değiştiğinde skoru kaydet
        SaveScore();
    }

    public void GoToMainMenu()
    {
        SaveScore();
        SceneManager.LoadScene("MainMenu");
    }
}