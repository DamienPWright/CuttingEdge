using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BunnyState {
    Off,
    Shield,
    Sword,
    Magic,
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
        GameManager.onBeginBunny += BeginBunny;
        GameManager.onBeginBoss += BeginBunny;
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

    void BeginBunny()
    {
        foreach(SpriteRenderer r in renderers.Values) {
            r.enabled = false;
        }
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
        //Debug.Log("Update Bunnies");
        foreach(SpriteRenderer r in renderers.Values) {
            if(r.name == state.ToString()) {
                r.enabled = true;
            } else {
                r.enabled = false;
            }
            
        }
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
        if(col == 0 && state != BunnyState.Off) {
            //GameManager._instance.LifeDown();
        }
        if(col < 3) {
            Debug.Log("Moving bunnies, " + state.ToString() + " -> " + rows[row].bunnies[col + 1].ToString());
            state = rows[row].bunnies[col + 1];
        }
    }

    void DoNewBunny(int r)
    {
        if(col + 1 < 4) { return; }
        if(r == row) {
            var n = Random.Range(1, 3);
            if(n == 1) {
                state = BunnyState.Magic;
            } else if(n == 2) {
                state = BunnyState.Shield;

            } else {
                state = BunnyState.Sword;

            }
            UpdateBunnies();
        }
    }
}
