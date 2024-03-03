using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectAndCheckAngle : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    int _x;

    [SerializeField]
    int _y;

    [SerializeField]
    int _z;

    [SerializeField]
    bool _compareX;

    [SerializeField]
    bool _compareY;

    [SerializeField]
    bool _compareZ;

    [SerializeField]
    float _maxAngle;

    [SerializeField]
    float _minAngle;

    Vector3 _pos;

    void Awake()
    {
        _pos = new Vector3(_x, _y, _z) * _moveSpeed;    
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
            Debug.Log(transform.name + " " + transform.localRotation.eulerAngles);
            if(_compareX)
            {
                if (transform.localRotation.eulerAngles.x >= _maxAngle || transform.localRotation.eulerAngles.x <= _minAngle) _pos = -_pos;
            }
            else if (_compareY)
            {
                if (transform.localRotation.eulerAngles.y >= _maxAngle || transform.localRotation.eulerAngles.y <= _minAngle) _pos = -_pos;
            }
            else if (_compareZ)
            {
                if (transform.localRotation.eulerAngles.z >= _maxAngle || transform.localRotation.eulerAngles.z <= _minAngle) _pos = -_pos;
            }
            transform.Rotate(_pos);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
