using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    float _swipeSpeed;

    private Vector3 _sTouchPos, _eTouchPos, _force;
    Rigidbody _rigidbody;
    Coroutine _coroutine;
    Player _player;

    void Start()
    {
        _swipeSpeed = 200f;
        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();

        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    void Update()
    {
        GetTouch();
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

                if(touch.phase == TouchPhase.Began) _sTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                if(touch.phase == TouchPhase.Ended)
                {
                    _eTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                    _force = _sTouchPos - _eTouchPos;
                    _force = Vector3.Magnitude(_force) >= 5f ? _force.normalized * 5f : _force;
                    
                    if (_coroutine != null) StopCoroutine(_coroutine);
                    _coroutine = StartCoroutine(Move());
                }
            }
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
