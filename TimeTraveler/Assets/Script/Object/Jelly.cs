using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    [SerializeField]
    bool _isMemory;

    [SerializeField]
    int[] _info;

    public bool CheckMemory() { return _isMemory; }
    public int[] GetInfo() { return _info; } //해금된 스토리인지 확인할 수 있는 변수
    public GameObject GetParent()
    {
        GameObject obj = transform.parent.gameObject;
        return obj;
    }
}
