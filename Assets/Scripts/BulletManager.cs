using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    public List<LCD_Bullet> bullets;
    public LCD_Bullet bullet;
    public int maxBullets = 24;

    public static BulletManager _instance;

	// Use this for initialization
	void Awake () {
		if(_instance != null)
        {
            Debug.LogWarning("There can only be one BulletManager");
            Destroy(gameObject);
        }
        _instance = this;
        bullets = new List<LCD_Bullet>();
        for(int i = 0; i < maxBullets; i++)
        {
            LCD_Bullet newbullet = Instantiate<LCD_Bullet>(bullet);
            newbullet.gameObject.SetActive(false);
            bullets.Add(newbullet);
            GameManager._instance.AddBullet(newbullet);
        }
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        { 
            FireBullet(0, true, true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            FireBullet(1, true, true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            FireBullet(2, true, true);
        }
        */
    }

    public void FireBullet(int lane, bool direction, bool targetsplayer, float steptime)
    {
        LCD_Bullet firedbullet = GetFirstInactiveBullet();
        firedbullet.Fire(lane, direction, targetsplayer, steptime);
        //Debug.Log(firedbullet);
    }

    LCD_Bullet GetFirstInactiveBullet() {
        foreach(LCD_Bullet blt in bullets)
        {
            if (!blt.gameObject.activeSelf)
            {
                return blt;
            }
        }
        return null;
    }
}
