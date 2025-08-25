using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI[] topScoreTexts; // 3 Text objesi ba�la

    private int[] topScores = new int[3];
    private static ScoreManager instance;

    void Awake()
    {
        // Singleton pattern - sadece bir ScoreManager �rne�i olmal�
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // LastScore'u al ve topScores'a ekle
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        // Sadece pozitif skorlar� ekle (0'dan b�y�kse)
        if (lastScore > 0)
        {
            AddScore(lastScore);
            // LastScore'u s�f�rla ki bir daha eklenmesin
            PlayerPrefs.SetInt("LastScore", 0);
            PlayerPrefs.Save();
        }

        DisplayScores();
    }

    public void AddScore(int score)
    {
        LoadScores();

        // Yeni skoru ekle ve s�rala (b�y�kten k����e)
        topScores = topScores.Append(score).ToArray();
        topScores = topScores.OrderByDescending(s => s).Take(3).ToArray();

        // PlayerPrefs'e kaydet
        for (int i = 0; i < topScores.Length; i++)
        {
            PlayerPrefs.SetInt("TopScore" + i, topScores[i]);
        }
        PlayerPrefs.Save();

        DisplayScores();
    }

    void LoadScores()
    {
        for (int i = 0; i < 3; i++)
            topScores[i] = PlayerPrefs.GetInt("TopScore" + i, 0);
    }

    void DisplayScores()
    {
        if (topScoreTexts == null || topScoreTexts.Length == 0)
            return;

        for (int i = 0; i < topScoreTexts.Length; i++)
        {
            if (topScoreTexts[i] != null)
                topScoreTexts[i].text = (i + 1) + ". " + topScores[i];
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetScores()
    {
        for (int i = 0; i < 3; i++)
            PlayerPrefs.SetInt("TopScore" + i, 0);

        PlayerPrefs.Save();
        LoadScores();
        DisplayScores();
    }

    // UI elementleri de�i�ti�inde �a�r�l�r (yeni sahnede)
    public void UpdateUIReferences(TextMeshProUGUI[] newTopScoreTexts)
    {
        topScoreTexts = newTopScoreTexts;
        DisplayScores();
    }
}