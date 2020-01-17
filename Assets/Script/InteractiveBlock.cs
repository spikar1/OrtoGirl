using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : Block {
    public override void Start() {

        base.Start();
        if (grid.NodeFromWorldPoint(transform.position).currentObject == null)
            grid.NodeFromWorldPoint(transform.position).currentObject = this.gameObject;
    }
    public virtual void Update() {

    }

    public virtual void Activate() {

    }
}
