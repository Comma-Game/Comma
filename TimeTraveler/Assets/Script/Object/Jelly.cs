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
    public int[] GetInfo() { return _info; } //�رݵ� ���丮���� Ȯ���� �� �ִ� ����
    public GameObject GetParent()
    {
        GameObject obj = transform.parent.gameObject;
        return obj;
    }
}
