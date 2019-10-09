using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCD_Bullet : LCD_MovingObject
{
    public bool AttacksThePlayer = false;

    protected override void CheckCollisionAt(int Xpos, int Ypos)
    {
        if (AttacksThePlayer)
        {
            return;
        }
        //Debug.Log(Xpos + ", " + Ypos);
        LCD_Bunny bunny = GameManager._instance.checkBunnyAtPosition(Xpos, Ypos);
        if (bunny != null)
        {
            //Debug.Log("Found bunny!");
            bunny.OnBulletCollidedWithBunny(this);
            OnBulletCollidedWithBunny();
        }
    }

    

    protected override void InitlializeStepTime()
    {
        Step_Time = 0.5f;
    }

    public void Fire(int y_position, bool dir, bool targetsplayer, float steptime=0.5f)
    {
        x_pos_index = 0;
        y_pos_index = (sbyte)y_position;
        direction = dir;
        transform.position = new Vector2(x_positions[x_pos_index], y_positions[y_pos_index]);
        AttacksThePlayer = targetsplayer;
        Step_Time = steptime;
        gameObject.SetActive(true);
    }

    public void OnBulletCollidedWithBunny()
    {
        gameObject.SetActive(false);
    }

    protected override void OnEndOfLane()
    {
        if (!AttacksThePlayer){
            base.OnEndOfLane();
            return;
        }
        if (GameManager._instance.CheckForPlayer(y_pos_index))
        {
            Debug.Log("Hit the player!");
        }
        gameObject.SetActive(false);
    }
}
