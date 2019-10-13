using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI highscoreText;
    [SerializeField] public TextMeshProUGUI stageText;
    [SerializeField] public TextMeshProUGUI heartsText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.onScore += UpdateScore;
        GameManager.onPlayerLifeChange += UpdateLife;
    }

    void UpdateScore(int stage, int score, int highscore)
    {
        if(stageText != null) {
            stageText.SetText(stage.ToString("00"));
        }
        if(scoreText != null) {
            scoreText.SetText(score.ToString("000000"));
        }
        if(highscoreText != null) {
            highscoreText.SetText(highscore.ToString("000000"));
        }
    }

    void UpdateLife(int lives)
    {
        string s = "";
        for(int i = 0; i < lives; i++) {
            s += "💀";
        }
        if(heartsText != null) {
            heartsText.SetText(s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
