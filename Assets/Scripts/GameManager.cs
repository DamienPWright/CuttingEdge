using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] public static float BunnyTick;
    [SerializeField] public static float PlayerBulletTick;
    [SerializeField] public static float BossTick;
    [SerializeField] public static float BossBulletTick;

    public const int N_ROWS = 3;
    public const int N_BUNNIES = 4;
    public const float BOOT_DELAY = 1.0f;
    public const float GAME_START_DELAY = 1.0f;    

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
        CreateRows();
    }

    private void SwitchState(GameState new_state) {
        state_first_time = true;
        state = new_state;
    }

    private void HandleState() {
        switch(state) {
            case GameState.Start:
                SwitchState(GameState.Boot);
                break;
            case GameState.Boot:
                if(state_first_time) {
                    // Turn everything on
                    delay_timer = BOOT_DELAY;
                }
                if(delay_timer <= 0.0f) {
                    SwitchState(GameState.NewGame);
                }
                break;
            case GameState.NewGame:
                if(state_first_time) {
                    
                    delay_timer = GAME_START_DELAY;
                }
                if(delay_timer <= 0.0f) {
                    SwitchState(GameState.BunnyStage);
                }
                break;
            case GameState.BunnyStage:
                //if bunny over, switch to boss
                break;
            case GameState.BossStage:
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
        Debug.Log("Update GM");
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
    }



    public delegate void ScoreEvent();
    public static event ScoreEvent onScore;

    public delegate void PlayerBulletTickEvent(List<Row> rows);
    public static event PlayerBulletTickEvent onPlayerBulletTick;

    
    public delegate void BunnyTickEvent(List<Row> rows);
    public static event BunnyTickEvent onBunnyTick;


    public delegate void BossTickEvent(List<Row> rows);
    public static event BossTickEvent onBossTick;

    public delegate void BossBulletTickEvent(List<Row> rows);
    public static event BossBulletTickEvent onBossBulletTick;




}

