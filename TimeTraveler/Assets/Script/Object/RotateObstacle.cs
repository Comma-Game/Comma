using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    bool _random;

    Vector3 _pos;

    void Awake()
    {
        if (_random)
        {
            _pos = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
            _moveSpeed = Random.Range(0.01f, 0.05f);
        }
        else _pos = new Vector3(0, -3, 0);    
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
            yield return new WaitForSeconds(_moveSpeed);
        }
    }
}
