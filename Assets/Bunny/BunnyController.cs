using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BunnyState {
    Off,
    Bunny,
}

public class BunnyController : LCD_Gameobject
{
    public BunnyState state = BunnyState.Off;
    public int col = -1;
    public bool forced = false;
    private Dictionary<string, SpriteRenderer> renderers;

    // Start is called before the first frame update
    void Start()
    {
        renderers = new Dictionary<string, SpriteRenderer>();
        SpriteRenderer[] the_renderers = this.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer r in the_renderers) {
        //    Debug.Log(r.gameObject.name);
            SpriteRenderer rend = r.gameObject.GetComponent<SpriteRenderer>();
        //    Debug.Log(rend);
            this.renderers.Add(r.gameObject.name, rend);
        }
        foreach(SpriteRenderer r in renderers.Values) {
            r.enabled = false;
        }
        GameManager.onBunnyTick += DoTick;
        GameManager.onLCDForce += DoForce;
        GameManager.onNewBunny += DoNewBunny;
    }   

    // Update is called once per frame
    void Update()
    {
        /*if(Random.value < 0.2) {
            foreach(SpriteRenderer r in renderers.Values) {
                if(Random.value < 0.5) {
                    r.enabled = false;
                } else {
                    r.enabled = true;
                }
            }
        }*/

    }

    void DoForce(LCDForceKind kind) {
        foreach(SpriteRenderer r in renderers.Values) {
            switch(kind) {
                case LCDForceKind.On:
                    r.color = GameManager.NORMAL_COLOR;
                    r.enabled = true;
                    break;
                case LCDForceKind.Off:
                    r.color = GameManager.OFF_COLOR;
                    forced = true;
                    break;
                default:
                    r.color = GameManager.NORMAL_COLOR;
                    r.enabled = false;
                    forced = false;
                    UpdateBunnies();
                    break;
            }
            
        }
    }
    
    void UpdateBunnies()
    {
        Debug.Log("Update Bunnies");
    }
/*  void OnEnable()
    {
        GameManager.onBunnyTick += DoTick;
    }
    void OnDisable()
    {
        GameManager.onBunnyTick -= DoTick;
    }*/

    void DoTick(List<RowInfo> rows)
    {
        if(col == 0 && state == BunnyState.Bunny) {
            //GameManager._instance.LifeDown();
        }
        if(col + 1 < GameManager.N_BUNNIES) {
            state = rows[row].bunnies[col + 1];
        }
    }

    void DoNewBunny(int r)
    {
        if(r == row) {
            state = BunnyState.Bunny;
        }
    }
}
