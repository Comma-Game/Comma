using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    [SerializeField]
    float _swipeSpeed;

    [SerializeField]
    float _maxSpeed;

    private Vector3 _sTouchPos, _eTouchPos, _force, _v;
    Rigidbody _rigidbody;
    Coroutine _coroutine;
    float _slowTime;

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _slowTime = 1f;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _swipeSpeed = 10f;
        _maxSpeed = 20f;
    }

    void Update()
    {
        SetForce();
        Debug.Log("Velocity.Magnitude : " + Vector3.Magnitude(_rigidbody.velocity));
    }

    void SetForce()
    {
        if (Input.GetMouseButtonDown(0)) _sTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        if (Input.GetMouseButtonUp(0))
        {
            _eTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            if (_coroutine != null)
            {
                _slowTime = 1f;
                StopCoroutine(_coroutine);
            }

            _force = _sTouchPos - _eTouchPos;
            Debug.Log("Force.Magnitude : " + Vector3.Magnitude(_force));
            
            _force = Vector3.Magnitude(_force) >= 1.5f ? _force.normalized * 1.5f : _force;

            _coroutine = StartCoroutine(MoveTime());
            CheckSpeed();
        }
    }

    void CheckSpeed()
    {
        _rigidbody.velocity = Vector3.Magnitude(_rigidbody.velocity) >= _maxSpeed ? _rigidbody.velocity.normalized * _maxSpeed : _rigidbody.velocity;
    }

    public void HitObstacle()
    {
        _rigidbody.velocity /= 2;
        _v = _rigidbody.velocity;
    }

    IEnumerator GetSlow()
    {
        _v = _rigidbody.velocity;

        while (_slowTime > 0)
        {
            _slowTime -= 0.25f;
            _rigidbody.velocity = _v * _slowTime;
            yield return new WaitForSeconds(0.25f);
        }

        _slowTime = 1f;
    }

    IEnumerator MoveTime()
    {
        _rigidbody.AddForce(_force * _swipeSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(1.5f);
        _coroutine = StartCoroutine(GetSlow());
    }
}
