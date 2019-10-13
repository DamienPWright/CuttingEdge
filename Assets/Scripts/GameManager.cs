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
    [SerializeField] float padding = 1.0f;

    public const int N_ROWS = 3;
    public const int N_BUNNIES = 4;
    public const float BOOT_DELAY = 1.0f;
    public const float GAME_START_DELAY = 1.0f;    

    float Step_Timer = 0.0f;
    float Step_Time = 1.0f;
    public Player player;

    public static GameManager _instance;

    private GameState state = GameState.Start;
    public bool state_first_time = true;
    public float delay_timer = 0.0f;
    private void Awake()
    {
        if(_instance != null)
        {
            Debug.LogWarning("Only one instance of GameManager is allowed");
            Destroy(gameObject);
            return;
        }
        _instance = this;
<<<<<<< HEAD
        CreateRows(3, 4, 4, 5, -6, 5);
=======
        SwitchState(GameState.Start);
        ResetGame();
>>>>>>> 5babf196982e31b23f2dc1a0ba63cb3bdc9506a3
    }

    public void ResetGame() {
        CreateRows(3, 5, -5, 5);
        
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

    private void CreateRows(float top, float bot, float left, float right)
    {
        int n = N_ROWS;
        int bunnies = N_BUNNIES;
        float height = top - bot;
        float width = right - left;
        float x = left;
        float y = top;
        float z = 1.0f;

        List<Row> l = new List<Row>();
        for (int i = 0; i < n; i++)
        {
            Row r = new Row();
            x = left;
            var b = Instantiate(player_prefab, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
            r.player = b;
            x += player_spacing;
            x += bunny_spacing;
            r.bunnies = new List<GameObject>();
            for (int j = 0; j < bunnies; j++)
            {
                b = Instantiate(bunny_prefab, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                r.bunnies.Add(b);
                x += bunny_spacing;
            }
            x += player_spacing;
            b = Instantiate(boss_prefab, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
            r.boss = b;
            y -= row_spacing;
            l.Add(r);
        }
        rows = l;


        //Update is called every frame.
        void Update()
        {
            
        }
    }
}

    /*public LCD_Bunny checkBunnyAtPosition(int x_pos, int y_pos)
    {
        return checkObjectAtPosition(x_pos, y_pos, bunnies);
    }

    public LCD_Bullet checkBulletAtPosition(int x_pos, int y_pos)
    {
        return checkObjectAtPosition(x_pos, y_pos, bullets);
    }

    public T checkObjectAtPosition<T>(int x_pos, int y_pos, List<T> list) where T : LCD_MovingObject
    {
        T objectCollidedWith = null;
        foreach (T thing in list)
        {
            if (!thing.gameObject.activeSelf)
            {
                continue;
            }
            if ((x_pos == thing.GridXPos) && (y_pos == thing.GridYPos))
            {
                objectCollidedWith = thing;
            }
        }
        return objectCollidedWith;
    }

    public void AddBullet(LCD_Bullet bullet)
    {
        bullets.Add(bullet);
    }

    public void AddBunny(LCD_Bunny bunny)
    {
        bunnies.Add(bunny); 
    }

    public bool CheckForPlayer(float y_pos)
    {
        if(player.Ypos == y_pos)
        {
            return true;
        }
        return false;
    }
}*/
