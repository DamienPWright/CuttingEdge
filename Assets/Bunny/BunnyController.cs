using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BunnyState {
    Off,
    Bunny,
}

public class BunnyController : LCD_Gameobject
{
    public int col = -1;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.value < 0.2) {
            foreach(SpriteRenderer r in renderers.Values) {
                if(Random.value < 0.5) {
                    r.enabled = false;
                } else {
                    r.enabled = true;
                }
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

    void DoTick(List<Row> rows)
    {
    }
}
