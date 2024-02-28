using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowGrass : MonoBehaviour
{
    [SerializeField]
    float _growSpeed;

    [SerializeField]
    float _maxScale;

    Vector3 _scale;

    private void Awake()
    {
        _scale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = _scale;
    }

    void Update()
    {
        if (transform.position.y >= 150 && transform.localScale.y <= _maxScale) transform.localScale = new Vector3(transform.localScale.x + _growSpeed, transform.localScale.y + _growSpeed, transform.localScale.z + _growSpeed);
    }
}
