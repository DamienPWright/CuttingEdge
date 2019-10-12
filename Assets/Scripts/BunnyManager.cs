using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyManager : MonoBehaviour
{

    public List<LCD_Bunny> bunnies;
    public LCD_Bunny bunny;
    public int maxBunnies = 24;

    public static BunnyManager _instance;

    // Use this for initialization
    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("There can only be one Bunnymanager");
            Destroy(gameObject);
        }
        _instance = this;
        bunnies = new List<LCD_Bunny>();
        /*for (int i = 0; i < maxBunnies; i++)
        {
            LCD_Bunny newbunny = Instantiate<LCD_Bunny>(bunny);
            newbunny.gameObject.SetActive(false);
            bunnies.Add(newbunny);
            GameManager._instance.AddBunny(newbunny);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnBunny(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnBunny(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SpawnBunny(2);
        }
    }

    public void SpawnBunny(int lane)
    {
        LCD_Bunny newbunny = GetFirstInactiveBunny();
        newbunny.Spawn(lane);
        //Debug.Log(newbunny);
    }

    LCD_Bunny GetFirstInactiveBunny()
    {
        foreach (LCD_Bunny bny in bunnies)
        {
            if (!bny.gameObject.activeSelf)
            {
                return bny;
            }
        }
        return null;
    }
}