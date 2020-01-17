using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureSwitch : InteractableBlock {

    public UnityEvent OnActivation;

    public override void Activate() {
        OnActivation.Invoke();
    }
}
