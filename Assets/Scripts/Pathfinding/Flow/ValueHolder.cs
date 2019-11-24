using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueHolder : MonoBehaviour
{
    [SerializeField]
    private int val = 0;
    public int Value {get {return val; }}
}
