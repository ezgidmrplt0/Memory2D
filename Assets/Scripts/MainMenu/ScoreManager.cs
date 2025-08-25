using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI[] topScoreTexts; // 3 Text objesi baðla

    private int[] topScores = new int[3];
    private static ScoreManager instance;

    void Awake()
    {
        // Singleton pattern - sadece bir ScoreManager örneði olmalý
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

        // Sadece pozitif skorlarý ekle (0'dan büyükse)
        if (lastScore > 0)
        {
            AddScore(lastScore);
            // LastScore'u sýfýrla ki bir daha eklenmesin
            PlayerPrefs.SetInt("LastScore", 0);
            PlayerPrefs.Save();
        }

        DisplayScores();
    }

    public void AddScore(int score)
    {
        LoadScores();

        // Yeni skoru ekle ve sýrala (büyükten küçüðe)
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

    // UI elementleri deðiþtiðinde çaðrýlýr (yeni sahnede)
    public void UpdateUIReferences(TextMeshProUGUI[] newTopScoreTexts)
    {
        topScoreTexts = newTopScoreTexts;
        DisplayScores();
    }
}