using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public GridScript grid;
    Renderer rend;
    LayerMask originalMask;

    public virtual void Start() {
        if (!grid) {
            if (GameObject.Find("AStar")) {
                grid = GameObject.Find("AStar").GetComponent<GridScript>();
            }
        }

        rend = GetComponent<Renderer>();
        originalMask = gameObject.layer;

    }

    public void BecomeTransparent() {
        CancelInvoke("BecomeOpaque");
        Invoke("BecomeOpaque", .1f);
        gameObject.layer = 0;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    public void BecomeOpaque() {
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        gameObject.layer = originalMask;
    }
}
