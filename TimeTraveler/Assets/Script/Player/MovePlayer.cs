using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    float _swipeRange;

    [SerializeField]
    float _swipeSpeed;

    [SerializeField]
    float _maxSpeed;

    private Vector3 _sTouchPos, _eTouchPos, _force, _v;
    Rigidbody _rigidbody;
    Coroutine _coroutine;
    Player _player;
    float _slowTime;
    Canvas _canvas;

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
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _swipeRange = 200f;
        _swipeSpeed = 0.08f;
        _maxSpeed = 20f;
    }

    void Update()
    {
        GetTouch();
        CheckSpeed();
    }

    void GetTouch()
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount >= 2)
            {
                _sTouchPos = Vector3.zero;
                _eTouchPos = Vector3.zero;
                _force = Vector3.zero;

                _player.UseSkill();
            }
            else
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) _sTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) * _canvas.scaleFactor;
                if (touch.phase == TouchPhase.Ended)
                {
                    _eTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) * _canvas.scaleFactor;

                    if (_coroutine != null)
                    {
                        _slowTime = 1f;
                        StopCoroutine(_coroutine);
                    }

                    _force = _sTouchPos - _eTouchPos;

                    _force = Vector3.Magnitude(_force) >= _swipeRange ? _force.normalized * _swipeRange : _force;
                    _force = new Vector3(_force.x, 0, _force.y);
                    Debug.Log("Force : " + _force); Debug.Log("Force.Magnitude : " + Vector3.Magnitude(_force));
                    _coroutine = StartCoroutine(MoveTime());
                }
            }
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
