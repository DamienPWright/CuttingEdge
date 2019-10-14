using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotKind {
    None,
    Shield,
    Sword,
    Magic,
}
public enum LCDForceKind {
    Normal,
    Off,
    On,
}

enum GameState {
    Start,
    Boot,
    NewGame,
    PreBunny,
    BunnyStage,
    PreBoss,
    BossStage,
    Ending,
}

public struct Row {
    [SerializeField] public GameObject player;
    [SerializeField] public List<GameObject> bunnies;
    [SerializeField] public GameObject boss;
}

public struct RowInfo {
    public List<BunnyState> bunnies;
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

    public int player_row = 1;
    public float BunnyTick = 0.25f;
    public float BunnyChance = 0.25f;
    public int BunnyCount = 10;
    public float PlayerBulletTick = 0.125f;
    public float BossTick = 0.2f;
    public float BossBulletTick = 0.05f;

    public const int N_ROWS = 3;
    public const int N_BUNNIES = 4;
    [SerializeField] public float BOOT_DELAY = 2.0f;
    [SerializeField] public float GAME_START_DELAY = 1.0f;    
    [SerializeField] public float PREBUNNY_DELAY = 0.5f;    
    [SerializeField] public float PREBOSS_DELAY = 0.5f;    
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
        QualitySettings.vSyncCount = 2;
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
    }
    public void ClearGame() {
        QualitySettings.vSyncCount = 2;
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
                if(state_first_time) {
                    if(onLCDForce != null) { onLCDForce(LCDForceKind.Off); }
                }
                //onLCDForce(LCDForceKind.Off);
                SwitchState(GameState.Boot);
                break;
            case GameState.Boot:
                if(state_first_time) {
                    // Turn everything on
                    if(onLCDForce != null) { onLCDForce(LCDForceKind.On); }
                    delay_timer = BOOT_DELAY;
                }
                if(delay_timer <= 0.0f) {
                    SwitchState(GameState.NewGame);
                }
                break;
            case GameState.NewGame:
                if(state_first_time) {
                    if(onLCDForce != null) { onLCDForce(LCDForceKind.Normal); }
                    Debug.Log("Going Normal");
                    delay_timer = GAME_START_DELAY;
                    ClearGame();
                    onScore(stage, score, highscore);
                }
                if(delay_timer <= 0.0f) {
                    SwitchState(GameState.BunnyStage);
                }
                break;
            case GameState.PreBunny:
                if(state_first_time) {
                    delay_timer = PREBUNNY_DELAY;
                    if(onBeginBunny != null) {
                        onBeginBunny();
                    }
                }
                if(delay_timer <= 0) {
                    SwitchState(GameState.BunnyStage);
                }
                break;
            case GameState.BunnyStage:
                if(state_first_time) {
                    if(onBeginBunny != null) {
                        onBeginBunny();
                    }

                    player_row = 1;
                    onPlayerMove(player_row);
                    BunnyTick = BunnyTickForStage(stage);
                    BunnyCount = BunniesForStage(stage);
                    BunnyChance = BunnySpawnChanceForStage(stage);
                    Debug.Log("Begin bunny stage: " + BunnyTick.ToString("0.000") + " tick " + BunnyCount.ToString() + " count " + BunnyChance.ToString() + " chance");
                }

                if(player_row > 0 && Input.GetKeyDown(KeyCode.UpArrow)) {
                    player_row -= 1;
                    onPlayerMove(player_row);
                } else if(player_row < 2 && Input.GetKeyDown(KeyCode.DownArrow)) {
                    player_row += 1;
                    onPlayerMove(player_row);
                }

                if(Input.GetKeyDown(KeyCode.Q)) {
                    if(onPlayerShoot != null) { onPlayerShoot(player_row, ShotKind.Magic); }
                } else if(Input.GetKeyDown(KeyCode.Q)) {
                    if(onPlayerShoot != null) { onPlayerShoot(player_row, ShotKind.Shield); }

                } else if(Input.GetKeyDown(KeyCode.Q)) {
                    if(onPlayerShoot != null) { onPlayerShoot(player_row, ShotKind.Sword); }
                }

                if(Input.GetKeyDown(KeyCode.P)) {
                    SwitchState(GameState.PreBoss);
                }
                //if bunny over, switch to boss
                break;
            case GameState.PreBoss:
                if(state_first_time) {
                    delay_timer = PREBOSS_DELAY;
                    if(onBeginBoss != null) {
                        onBeginBoss();
                    }
                }
                if(delay_timer <= 0) {
                    SwitchState(GameState.BossStage);
                }
                break;
            case GameState.BossStage:
                if(state_first_time) {
                    if(onSpawnBoss != null) {
                        int r = Random.Range(0,3);
                        if(r == 0) {
                            onSpawnBoss(r, ShotKind.Magic);
                        } else if(r == 1) {
                            onSpawnBoss(r, ShotKind.Shield);
                        } else if(r == 2) {
                            onSpawnBoss(r, ShotKind.Sword);
                        }
                    }
                }

                if(lives == 0) {
                    SwitchState(GameState.Ending);
                }

                if(Input.GetKeyDown(KeyCode.P)) {
                    SwitchState(GameState.PreBunny);
                }
                //if boss over, switch to bunny
                break;
            case GameState.Ending:
                // we get here if we lose
                // shows scores
                // press button to move on
                if(Input.GetKeyDown(KeyCode.P)) {
                    SwitchState(GameState.PreBunny);
                }

                break;
        }
        state_first_time = false;
    }

    private int BunniesForStage(int stage) {
        return (int)((Mathf.Sqrt((float)stage) + 1.5f) * 2f + 2f);
    }

    private float BunnyTickForStage(int stage) {
        return 0.25f;
        return 0.3f * Mathf.Sqrt((float)stage * 3.5f);
    }

    private float BunnySpawnChanceForStage(int stage)
    {
        return 0.25f;
        return 0.15f * Mathf.Sqrt((float)stage * 3.5f);
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
//        if(onPlayerLifeChange != null) { onPlayerLifeChange(lives); }

        bunny_timer -= Time.deltaTime;
        player_bullet_timer -= Time.deltaTime;
        boss_timer -= Time.deltaTime;
        boss_bullet_timer -= Time.deltaTime;
        if(bunny_timer <= 0) {
            bunny_timer = BunnyTick;
            if(onBunnyTick != null) {
                var rowdata = new List<RowInfo>();
                foreach (var r in rows)
                {
                    var newrow = new RowInfo();
                    var newbunnies = new List<BunnyState>();
                    foreach (var b in r.bunnies)
                    {
                        newbunnies.Add(b.GetComponent<BunnyController>().state);
                    }
                    newrow.bunnies = newbunnies;
                    rowdata.Add(newrow);
                }
                Debug.Log("Bunny tick");

                onBunnyTick(rowdata);
                if(Random.value < BunnyChance) {
                    var row = Random.Range(0, N_ROWS);
                    Debug.Log("New bunny on row " + row.ToString());
                    onNewBunny(row);
                }
            }
        }
        if(player_bullet_timer <= 0) {
            player_bullet_timer = PlayerBulletTick;
            if(onPlayerBulletTick != null) {
                var rowdata = new List<RowInfo>();
                onPlayerBulletTick(rowdata);
            }
        }
        if(boss_timer <= 0) {
            boss_timer = BossTick;
            if(onBossTick != null) {
                var rowdata = new List<RowInfo>();
                onBossTick(rowdata);
            }
        }
        if(boss_bullet_timer <= 0) {
            boss_bullet_timer = BossBulletTick;
            if(onBossBulletTick != null) {
                var rowdata = new List<RowInfo>();
                onBossBulletTick(rowdata);
            }
        }

        HandleState();
    }


    public void LifeDown() {
        lives -= 1;
        if(lives == 0) {
            SwitchState(GameState.Ending);
        }
    }

    public delegate void ScoreEvent(int stage, int score, int highscore);
    public static event ScoreEvent onScore;

    public delegate void PlayerBulletTickEvent(List<RowInfo> rows);
    public static event PlayerBulletTickEvent onPlayerBulletTick;

    
    public delegate void BunnyTickEvent(List<RowInfo> rows);
    public static event BunnyTickEvent onBunnyTick;

    public delegate void NewBunnyEvent(int row);
    public static event NewBunnyEvent onNewBunny;


    public delegate void BossTickEvent(List<RowInfo> rows);
    public static event BossTickEvent onBossTick;

    public delegate void BossBulletTickEvent(List<RowInfo> rows);
    public static event BossBulletTickEvent onBossBulletTick;

    public delegate void BeginBossEvent();
    public static event BeginBossEvent onBeginBoss;

    public delegate void BeginBunnyEvent();
    public static event BeginBunnyEvent onBeginBunny;

    public delegate void PlayerLifeChangeEvent(int lives);
    public static event PlayerLifeChangeEvent onPlayerLifeChange;

    public delegate void PlayerShootEvent(int row, ShotKind kind);
    public static event PlayerShootEvent onPlayerShoot;

    public delegate void PlayerMoveEvent(int row);
    public static event PlayerMoveEvent onPlayerMove;

    public delegate void BossShootEvent();
    public static event BossShootEvent onBossShoot;

    public delegate void LCDForceEvent(LCDForceKind kind);
    public static event LCDForceEvent onLCDForce;

    public delegate void SpawnBossEvent(int row, ShotKind kind);
    public static event SpawnBossEvent onSpawnBoss;
}

