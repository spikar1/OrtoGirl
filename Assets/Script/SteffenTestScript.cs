using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteffenTestScript : MonoBehaviour
{

    float myFloat = 3.945f;

    private void Start() {
        int myConvertedFloat = (int)myFloat;
        int myOtherInt = Mathf.RoundToInt(myFloat);
        string timer = myFloat.ToString("0.00");
    }
}
