using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RowArray
{
    [SerializeField]
    GameObject[] row;

    public GameObject[] GetRow() { return row; }
}

public class GetStage : MonoBehaviour
{
    [SerializeField]
    RowArray[] _stagePrefab;

    public GameObject[] GetStagePrefab(int index) { return _stagePrefab[index].GetRow(); }

    public int ConceptCount() { return _stagePrefab.Length; }
}
