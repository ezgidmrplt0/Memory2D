using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI[] topScoreTexts; // Ana menüdeki skor textleri

    void Start()
    {
        // ScoreManager'ý bul ve UI referanslarýný güncelle
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.UpdateUIReferences(topScoreTexts);
        }
    }

    // Oyna butonuna baðla
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // oyun sahnesi
    }

    // Skor butonuna baðla
    public void OpenScore()
    {
        SceneManager.LoadScene("ScoreScene"); // skor sahnesi
    }

    // Çýkýþ butonuna baðla
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // editor'de durdur
#else
        Application.Quit(); // gerçek cihazda çýkýþ
#endif
    }

    // Skorlarý sýfýrla butonu
    public void ResetScores()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetScores();
        }
    }
}