using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCD_MovingObject : LCD_Gameobject {

    [SerializeField] protected float[] x_positions;
    [SerializeField] protected sbyte x_pos_index = 0;
    [SerializeField] protected float[] y_positions;
    [SerializeField] protected sbyte y_pos_index = 0;
    public AttackType attack_type = AttackType.sword;
    [SerializeField] public bool direction = false;

    public int GridXPos
    {
        get
        {
            return x_pos_index;
        }
    }

    public int GridYPos
    {
        get
        {
            return y_pos_index;
        }
    }

    private void Awake()
    {
        InitlializeStepTime();
    }

    private void OnEnable()
    {
        Debug.Log("Enabled");
        CheckCollisionAt(x_pos_index, y_pos_index);
    }

    public override void Step()
    {
        if (CheckCollision())
        {
            //Set self to inactive
        }

        if (direction)
        {
            x_pos_index++;
        }
        else
        {
            x_pos_index--;
        }

        if (CheckEndOfLane())
        {
            //Debug.Log("End of lane");
            //Check if there's a valid target at the end of the lane
            //  - Logic for what to do should be in that call
            //Set self to inactive
            OnEndOfLane();
        }
        else
        {
            Move();
        }
    }

    protected virtual bool CheckCollision()
    {
        //Check if a valid target is next to us
        //x_index + 1 or - 1 depending on direction, (byte)Lane
        //This shouldnt go out of bounds but might want a sanity check here anyway
        int x_check = 0;
        if (direction)
        {
            x_check = x_pos_index + 1;
        }
        else
        {
            x_check = x_pos_index - 1;
        }

        if((x_check >= x_positions.Length) || x_check < 0)
        {
            //Debug.Log("Attempted to check out of bounds");
            return false;
        }
        //Debug.Log("Checking collision at " + x_check + ", " + y_pos_index);
        CheckCollisionAt(x_check, y_pos_index);
        return false;
    }

    protected virtual void CheckCollisionAt(int Xpos, int Ypos)
    {
        //GameManager._instance.checkBunnyAtPosition(Xpos, Ypos);
    }

    protected virtual bool CheckEndOfLane()
    {
        if (direction)
        {
            if (x_pos_index >= (x_positions.Length))
            {
                x_pos_index = (sbyte)(x_positions.Length - 1);
                return true;
            }
        }
        else
        {
            if (x_pos_index < 0)
            {
                x_pos_index = 0;
                return true;
            }
        }
        return false;
    }



    protected virtual void Move()
    {
        
        transform.position = new Vector2(x_positions[x_pos_index], transform.position.y);
        //Debug.Log(x_pos_index);

    }

    protected virtual void InitlializeStepTime()
    {
       Step_Time = 1;
    }

    protected virtual void OnEndOfLane()
    {
        gameObject.SetActive(false);
    }
}
