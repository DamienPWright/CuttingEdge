using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameState {
    Start,
    Boot,
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


    float Step_Timer = 0;
    float Step_Time = 1;
    public Player player;

    public static GameManager _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Debug.LogWarning("Only one instance of GameManager is allowed");
            Destroy(gameObject);
            return;
        }
        _instance = this;
        CreateRows(3, 4, 4, 5, -6, 5);
    }


    private void CreateRows(int n, int bunnies, float top, float bot, float left, float right)
    {
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
