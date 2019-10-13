using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    bool forced = false;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI highscoreText;
    [SerializeField] public TextMeshProUGUI stageText;
    [SerializeField] public TextMeshProUGUI heartsText;
    private List<TextMeshProUGUI> texts;
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<TextMeshProUGUI>();
        texts.Add(scoreText);
        texts.Add(highscoreText);
        texts.Add(stageText);
        texts.Add(heartsText);
        GameManager.onScore += UpdateScore;
        GameManager.onPlayerLifeChange += UpdateLife;
        GameManager.onLCDForce += ForceLCD;
        scoreText.SetText("");
        highscoreText.SetText("");
        stageText.SetText("");
        heartsText.SetText("");
    }

    void UpdateScore(int stage, int score, int highscore)
    {
        if(forced) { return; }
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

    void ForceLCD(LCDForceKind kind) {
        Debug.Log("Force: " + kind.ToString());
        switch(kind) {
            case LCDForceKind.Normal:
                foreach(var t in texts) { t.color = GameManager.NORMAL_COLOR; }
                forced = false;
                break;
            case LCDForceKind.On:
                foreach(var t in texts) { t.color = GameManager.NORMAL_COLOR; }
                scoreText.SetText("888888");
                highscoreText.SetText("888888");
                stageText.SetText("88");
                heartsText.SetText("💀💀💀💀💀");
                forced = true;
                break;
            case LCDForceKind.Off:
                scoreText.SetText("");
                highscoreText.SetText("");
                stageText.SetText("");
                heartsText.SetText("");
                foreach(var t in texts) { t.color = GameManager.OFF_COLOR; }
                forced = true;
                break;
        }
    }
}
