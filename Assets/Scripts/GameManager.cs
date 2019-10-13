using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LCDForceKind {
    Normal,
    Off,
    On,
}

enum GameState {
    Start,
    Boot,
    NewGame,
    BunnyStage,
    BossStage,
    Ending,
}

public struct Row {
    [SerializeField] public GameObject player;
    [SerializeField] public List<GameObject> bunnies;
    [SerializeField] public GameObject boss;
}

public class GameManager : MonoBehaviour {
    [SerializeField] public GameObject bunny_prefab;
    [SerializeField] public GameObject player_prefab;
    [SerializeField] public GameObject boss_prefab;
    [SerializeField] public List<Row> rows;

    [SerializeField] float row_spacing = 2f;
    [SerializeField] float bunny_spacing = 3f;
    [SerializeField] float boss_spacing = 1f;
    [SerializeField] float player_spacing = 1f;

    [SerializeField] float top = 5f;
    [SerializeField] float left = -13f;

    [SerializeField] public float BunnyTick = 0.25f;
    [SerializeField] public float PlayerBulletTick = 0.125f;
    [SerializeField] public float BossTick = 0.2f;
    [SerializeField] public float BossBulletTick = 0.05f;

    public const int N_ROWS = 3;
    public const int N_BUNNIES = 4;
    [SerializeField] public float BOOT_DELAY = 2.0f;
    [SerializeField] public float GAME_START_DELAY = 1.0f;    
    [SerializeField] public int N_LIVES = 5;    
    
    [SerializeField] public static Color32 NORMAL_COLOR = new Color32(0, 0, 0, 255);
    [SerializeField] public static Color32 OFF_COLOR = new Color32(255, 0, 0, 0);

    float Step_Timer = 0.0f;
    float Step_Time = 1.0f;
    public static GameManager _instance;

    private GameState state = GameState.Start;
    public bool state_first_time = true;
    public float delay_timer = 0.0f;


    public float bunny_timer;
    public float player_bullet_timer;
    public float boss_timer;
    public float boss_bullet_timer;


    public int score = 0;
    public int highscore = 0;
    public int stage = 0;
    public int lives = 0;

    private void Awake()
    {
        if(_instance != null)
        {
            Debug.LogWarning("Only one instance of GameManager is allowed");
            Destroy(gameObject);
            return;
        }
        _instance = this;
        SwitchState(GameState.Start);
        ResetGame();
    }

    public void ResetGame() {
        bunny_timer = BunnyTick;
        player_bullet_timer = PlayerBulletTick;
        boss_timer = BossTick;
        boss_bullet_timer = BossBulletTick;
        highscore = 0;
        CreateRows();
        ClearGame();
    }
    public void ClearGame() {
        score = 0;
        stage = 0;
        lives = N_LIVES;
        if(onPlayerLifeChange != null) { onPlayerLifeChange(lives); }
    }
    private void SwitchState(GameState new_state) {
        state_first_time = true;
        state = new_state;
    }

    string oldState = "";

    private void HandleState() {
        if(state.ToString() != oldState) {
            oldState = state.ToString();
            Debug.Log("State " + state.ToString());
            Debug.Log(state_first_time.ToString());
            state_first_time = true;
        }
        switch(state) {
            case GameState.Start:
                //onLCDForce(LCDForceKind.Off);
                SwitchState(GameState.Boot);
                break;
            case GameState.Boot:
                if(state_first_time) {
                    // Turn everything on
                    onLCDForce(LCDForceKind.On);
                    delay_timer = BOOT_DELAY;
                }
                if(delay_timer <= 0.0f) {
                    SwitchState(GameState.NewGame);
                }
                break;
            case GameState.NewGame:
                if(state_first_time) {
                    onLCDForce(LCDForceKind.Normal);
                    Debug.Log("Going Normal");
                    delay_timer = GAME_START_DELAY;
                    ClearGame();
                    onScore(stage, score, highscore);
                }
                if(delay_timer <= 0.0f) {
                    SwitchState(GameState.BunnyStage);
                }
                break;
            case GameState.BunnyStage:
                if(state_first_time) {
                    if(onBeginBunny != null) {
                        onBeginBunny();
                    }
                }
                //if bunny over, switch to boss
                break;
            case GameState.BossStage:
                if(state_first_time) {
                    if(onBeginBoss != null) {
                        onBeginBoss();
                    }
                }
                //if boss over, switch to bunny
                break;
            case GameState.Ending:
                // we get here if we lose
                // shows scores
                // press button to move on
                break;
        }
        state_first_time = false;
    }

    private void CreateRows()
    {
        int n = N_ROWS;
        int bunnies = N_BUNNIES;
        float x = left;
        float y = top;
        float z = 1.0f;

        List<Row> l = new List<Row>();
        for (int i = 0; i < n; i++)
        {
            Row r = new Row();
            x = left;
            var b = Instantiate(player_prefab, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
            b.GetComponent<LCD_Gameobject>().row = i;
            r.player = b;
            x += player_spacing;
            x += bunny_spacing;
            r.bunnies = new List<GameObject>();
            for (int j = 0; j < bunnies; j++)
            {
                b = Instantiate(bunny_prefab, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                b.GetComponent<LCD_Gameobject>().row = i;
                b.GetComponent<BunnyController>().col = j;
                r.bunnies.Add(b);
                x += bunny_spacing;
            }
            x += player_spacing;
            b = Instantiate(boss_prefab, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                b.GetComponent<LCD_Gameobject>().row = i;
            r.boss = b;
            y -= row_spacing;
            l.Add(r);
        }
        rows = l;


    }
    //Update is called every frame.
    void Update()
    {
        delay_timer -= Time.deltaTime;
        if(onPlayerLifeChange != null) { onPlayerLifeChange(lives); }

        bunny_timer -= Time.deltaTime;
        player_bullet_timer -= Time.deltaTime;
        boss_timer -= Time.deltaTime;
        boss_bullet_timer -= Time.deltaTime;
        if(bunny_timer <= 0) {
            bunny_timer = BunnyTick;
            if(onBunnyTick != null) {
                onBunnyTick(rows);
            }
        }
        if(player_bullet_timer <= 0) {
            player_bullet_timer = PlayerBulletTick;
            if(onPlayerBulletTick != null) {
                onPlayerBulletTick(rows);
            }
        }
        if(boss_timer <= 0) {
            boss_timer = BossTick;
            if(onBossTick != null) {
                onBossTick(rows);
            }
        }
        if(boss_bullet_timer <= 0) {
            boss_bullet_timer = BossBulletTick;
            if(onBossBulletTick != null) {
                onBossBulletTick(rows);
            }
        }

        if(Random.value < 0.1) {
            score += 1;
            highscore += 2;
            stage += 1;
            onScore(stage, score, highscore);
        }
        HandleState();
    }



    public delegate void ScoreEvent(int stage, int score, int highscore);
    public static event ScoreEvent onScore;

    public delegate void PlayerBulletTickEvent(List<Row> rows);
    public static event PlayerBulletTickEvent onPlayerBulletTick;

    
    public delegate void BunnyTickEvent(List<Row> rows);
    public static event BunnyTickEvent onBunnyTick;


    public delegate void BossTickEvent(List<Row> rows);
    public static event BossTickEvent onBossTick;

    public delegate void BossBulletTickEvent(List<Row> rows);
    public static event BossBulletTickEvent onBossBulletTick;

    public delegate void BeginBossEvent();
    public static event BeginBossEvent onBeginBoss;

    public delegate void BeginBunnyEvent();
    public static event BeginBunnyEvent onBeginBunny;

    public delegate void PlayerLifeChangeEvent(int lives);
    public static event PlayerLifeChangeEvent onPlayerLifeChange;

    public delegate void PlayerShootEvent();
    public static event PlayerShootEvent onPlayerShoot;

    public delegate void BossShootEvent();
    public static event BossShootEvent onBossShoot;

    public delegate void LCDForceEvent(LCDForceKind kind);
    public static event LCDForceEvent onLCDForce;

}

