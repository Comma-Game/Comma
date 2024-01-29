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
    GameObject _camera;
    Canvas _canvas;
    bool _useSkill;

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
        _camera = GameObject.Find("Main Camera");
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _swipeRange = 1.5f * _canvas.scaleFactor;
        _swipeSpeed = 10f;
        _maxSpeed = 20f;
        _useSkill = false;
    }

    void Update()
    {
        GetTouch();
        CheckSpeed();

        if(_useSkill && Input.touchCount == 0)
        {
            _useSkill = false;
            _sTouchPos = Vector3.zero;
            _eTouchPos = Vector3.zero;
            _force = Vector3.zero;
        }
    }

    void GetTouch()
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount == 2)
            {
                _useSkill = true;
                _player.UseSkill();
            }
            else if(!_useSkill && Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) _sTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f)) - _camera.transform.position;
                else if (touch.phase == TouchPhase.Ended)
                {
                    _eTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f)) - _camera.transform.position;

                    if (_coroutine != null)
                    {
                        _slowTime = 1f;
                        StopCoroutine(_coroutine);
                    }

                    _force = (_sTouchPos - _eTouchPos);

                    float dis = Vector3.Magnitude(_force);
                    if (dis < 0.1f) return;
                    Debug.Log("DIS : !!!!!!!!!!!!!!!!!!!!! ::::::::: " + dis);
                    _force =  dis >= _swipeRange ? _force.normalized * _swipeRange : _force;
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
            _slowTime -= 0.125f;
            _rigidbody.velocity = _v * _slowTime;
            yield return new WaitForSeconds(0.25f);
        }

        _slowTime = 1f;
    }

    IEnumerator MoveTime()
    {
        _rigidbody.AddForce(_force * _swipeSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(1f);
        _coroutine = StartCoroutine(GetSlow());
    }
}
