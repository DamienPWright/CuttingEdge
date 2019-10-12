using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCD_Bunny : LCD_MovingObject {

    protected override void CheckCollisionAt(int Xpos, int Ypos)
    {
        /*LCD_Bullet bullet = GameManager._instance.checkBulletAtPosition(Xpos, Ypos);
        if (bullet != null)
        {
            //Trigger the collided function on the bullet,
            if (bullet.AttacksThePlayer)
            {
                return;
            }
            bullet.OnBulletCollidedWithBunny();
            OnBulletCollidedWithBunny(bullet);
            //Trigger the collided function on this
            Debug.Log("Found Bullet!");
        }*/
    }

    public void OnBulletCollidedWithBunny(LCD_Bullet bullet)
    {
        if(attack_type == AttackType.sword && bullet.attack_type == AttackType.shield)
        {

        }
        if (attack_type == AttackType.magic && bullet.attack_type == AttackType.sword)
        {

        }
        if (attack_type == AttackType.shield && bullet.attack_type == AttackType.magic)
        {

        }
        gameObject.SetActive(false);
    }

    protected override void InitlializeStepTime()
    {
        Step_Time = 1f;
    }

    public void Spawn(int y_position)
    {
        x_pos_index = 3;
        y_pos_index = (sbyte)y_position;
        transform.position = new Vector2(x_positions[x_pos_index], y_positions[y_pos_index]);
        gameObject.SetActive(true);
    }

    protected override void OnEndOfLane()
    {
        /*if (GameManager._instance.CheckForPlayer(y_pos_index))
        {
            Debug.Log("Hit the player!");
        }*/
        gameObject.SetActive(false);
    }
}
