using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    bool _random;

    [SerializeField]
    int _x;

    [SerializeField]
    int _y;

    [SerializeField]
    int _z;

    Vector3 _pos;

    void Awake()
    {
        if (_random) _pos = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2)) * _moveSpeed;
        else _pos = new Vector3(_x, _y, _z) * _moveSpeed;    
    }

    private void OnEnable()
    {
        StartCoroutine(Rotate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(_pos);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
