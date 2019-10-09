using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] List<LCD_Gameobject> gameobjects;

    [SerializeField] List<LCD_Bunny> bunnies;
    [SerializeField] List<LCD_Bullet> bullets;

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
        //populate lists
        /*
        LCD_Gameobject[] gos = FindObjectsOfType<LCD_Bunny>();
        foreach (LCD_Bunny go in gos)
        {
            bunnies.Add(go);
        }
        LCD_Gameobject[] boolets = FindObjectsOfType<LCD_Bullet>();
        foreach (LCD_Bullet boolet in boolets)
        {
            bullets.Add(boolet);
        }
        */
    }

    public LCD_Bunny checkBunnyAtPosition(int x_pos, int y_pos)
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
}
