using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    [SerializeField]
    bool _isMemory;

    public bool CheckMemory() { return _isMemory; }
}
