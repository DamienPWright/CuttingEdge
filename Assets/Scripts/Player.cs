using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float[] YPositions;
    [SerializeField] sbyte Y_Pos_Index;

    public float Ypos
    {
        get
        {
            return Y_Pos_Index;
        }
    }

	// Use this for initialization
	void Start () {
        GameManager._instance.player = this;
	}
	
    public bool isOnLane(sbyte lane)
    {
        if(lane == Y_Pos_Index)
        {
            return true;
        }
        return false;
    }
	// Update is called once per frame
	void Update () {

        if(Input.GetButtonDown("Up")){
            Y_Pos_Index--;
            if(Y_Pos_Index <= 0)
            {
                Y_Pos_Index = 0;
            }
            Reposition();
        }else if (Input.GetButtonDown("Down"))
        {
            Y_Pos_Index++;
            if(Y_Pos_Index >= YPositions.Length)
            {
                Y_Pos_Index = (sbyte)(YPositions.Length - 1);
            }
            Reposition();
        }

        if (Input.GetButtonDown("Fire1")){
            BulletManager._instance.FireBullet(Y_Pos_Index, true, false, 0.2f);
        }
	}

    void Reposition()
    {
        transform.position = new Vector2(transform.position.x, YPositions[Y_Pos_Index]); 
    }
}
