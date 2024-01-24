using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    [SerializeField]
    float _swipeSpeed;
    private Vector3 _sTouchPos, _eTouchPos, _force;
    Rigidbody _rigidbody;
    Coroutine _coroutine;

    void Start()
    {
        _swipeSpeed = 150f;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        SetForce();
    }

    void SetForce()
    {
        if (Input.GetMouseButtonDown(0)) _sTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        if (Input.GetMouseButtonUp(0))
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _eTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            _force = _sTouchPos - _eTouchPos;
            _force = Vector3.Magnitude(_force) >= 5f ? _force.normalized * 5f : _force;
            _coroutine = StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        _rigidbody.AddForce(_force * _swipeSpeed, ForceMode.Acceleration);
        yield return new WaitForSeconds(1f);

        Vector3 v = _rigidbody.velocity;
        float d = 0f;
        while (d < 1f)
        {
            d += 0.05f;
            _rigidbody.velocity = v * (1 - d);
            yield return new WaitForSeconds(0.05f);
        }

        _rigidbody.velocity = Vector3.zero;
    }
}
