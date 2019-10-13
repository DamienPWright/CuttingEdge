using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI highscoreText;
    [SerializeField] public TextMeshProUGUI stageText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.onScore += UpdateScore;
    }

    void UpdateScore(int stage, int score, int highscore)
    {
        if(stageText != null) {
            stageText.SetText(stage.ToString());
        }
        if(scoreText != null) {
            scoreText.SetText(score.ToString());
        }
        if(highscoreText != null) {
            highscoreText.SetText(highscore.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
