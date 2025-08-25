using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI[] topScoreTexts; // Ana men�deki skor textleri

    void Start()
    {
        // ScoreManager'� bul ve UI referanslar�n� g�ncelle
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.UpdateUIReferences(topScoreTexts);
        }
    }

    // Oyna butonuna ba�la
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // oyun sahnesi
    }

    // Skor butonuna ba�la
    public void OpenScore()
    {
        SceneManager.LoadScene("ScoreScene"); // skor sahnesi
    }

    // ��k�� butonuna ba�la
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // editor'de durdur
#else
        Application.Quit(); // ger�ek cihazda ��k��
#endif
    }

    // Skorlar� s�f�rla butonu
    public void ResetScores()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetScores();
        }
    }
}