using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : LCD_Gameobject
{
    private Dictionary<string, SpriteRenderer> renderers;

    public ShotKind state = ShotKind.None;

    // Start is called before the first frame update
    void Start()
    {
        renderers = new Dictionary<string, SpriteRenderer>();
        SpriteRenderer[] the_renderers = this.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer r in the_renderers) {
            //Debug.Log(r.gameObject.name);
            SpriteRenderer rend = r.gameObject.GetComponent<SpriteRenderer>();
            //Debug.Log(rend);
            this.renderers.Add(r.gameObject.name, rend);
        }
        foreach(SpriteRenderer r in renderers.Values) {
            r.enabled = false;
        }
        GameManager.onBeginBoss += BeginBoss;
        GameManager.onBeginBunny += BeginBunny;
        GameManager.onSpawnBoss += SpawnBoss;
    }

    void BeginBunny()
    {
        foreach(SpriteRenderer r in renderers.Values) {
            r.enabled = false;
        }
    }

    void BeginBoss()
    {

        foreach(SpriteRenderer r in renderers.Values) {
            r.enabled = false;
        }
    }

    void SpawnBoss(int r, ShotKind kind)
    {
        if(row == r) {
            state = kind;
            foreach(SpriteRenderer rend in renderers.Values) {
                if(rend.gameObject.name == kind.ToString()) {
                    rend.enabled = true;
                }
            }
        }
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
}
