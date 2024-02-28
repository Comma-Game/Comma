using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
    bool _isFall;
    Vector3 _pos;
    Quaternion _rot;
    float _randomRot;

    private void Awake()
    {
        _pos = transform.localPosition;
        _rot = transform.localRotation;
    }

    private void OnEnable()
    {
        _isFall = false;
        transform.localPosition = _pos;
        transform.localRotation = _rot;

        _randomRot = Random.Range(-3, 3);
    }

    void Update()
    {
        if (!_isFall)
        {
            if (transform.position.y >= 150) Fall();
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            transform.Rotate(new Vector3(_randomRot, 0, _randomRot));
        }
    }

    void Fall()
    {
        _isFall = true;
        transform.SetParent(transform.parent.GetComponent<FallObjectManager>().GetStageParent());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) gameObject.SetActive(false);
    }
}
