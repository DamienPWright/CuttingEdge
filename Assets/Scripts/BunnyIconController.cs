using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyIconController : MonoBehaviour
{
    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = transform.GetComponent<SpriteRenderer>();
        GameManager.onBeginBunny += TurnOn;
        GameManager.onBeginBoss += TurnOff;
        GameManager.onLCDForce += DoForce;
    }

    void TurnOn() {
        renderer.enabled = true;
    }

    void TurnOff() {
        renderer.enabled = false;
    }

    void DoForce(LCDForceKind kind) {
        switch(kind) {
            case LCDForceKind.Normal:
                renderer.enabled = false;
                break;
            case LCDForceKind.On:
                renderer.enabled = true;
                break;
            case LCDForceKind.Off:
                renderer.enabled = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
