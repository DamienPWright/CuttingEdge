using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCD_Gameobject : MonoBehaviour {

    protected float Step_Time = 0;
    protected float Step_Timer = 0;

	public virtual void Step()
    {
        Debug.Log("Step");
    }

    private void Update()
    {
        Step_Timer += Time.deltaTime;
        if (Step_Timer >= Step_Time)
        {
            Step_Timer = 0;
            Step();
        }
    }
}
