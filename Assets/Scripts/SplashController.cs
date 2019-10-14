using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.onLCDForce += DoLCDForce;
    }

    void DoLCDForce(LCDForceKind kind) {
        GameManager.onLCDForce -= DoLCDForce;
        transform.position += Vector3.forward * 100f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
