using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    float _moveSpeed;

    Vector3 _pos;

    private void Awake()
    {
        _moveSpeed = Random.Range(0.5f, 1.5f);
        _pos = new Vector3(0, transform.localPosition.y, 0);    
    }

    private void Start()
    {
        StartCoroutine(RotateAround());
    }

    IEnumerator RotateAround()
    {
        while (true)
        {
            transform.RotateAround(_pos, Vector3.up, _moveSpeed);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
