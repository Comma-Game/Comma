using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    float _swipeRange;

    [SerializeField]
    float _swipeSpeed;

    [SerializeField]
    float _maxSpeed;

    [SerializeField]
    RectTransform _arrow;

    [SerializeField]
    RectTransform _colliderRange;

    private Vector3 _sTouchPos, _eTouchPos, _force;
    Rigidbody _rigidbody;
    GameObject _camera;
    Canvas _canvas;
    bool _isPause;
    float _radius;
    Vector2 _arrowDir;

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = GameObject.Find("Main Camera");
        _canvas = GameObject.Find("PlayerCanvas").GetComponent<Canvas>();

        _swipeRange = 1.5f * _canvas.scaleFactor;
        _swipeSpeed = 2.5f;
        _maxSpeed = 5f;

        _radius = (_colliderRange.sizeDelta / 2).magnitude;
    }

    void Update()
    {
        if (!_isPause)
        {
            GetTouch();
            CheckSpeed();

            ArrowRotate();
            ArrowPosition();
        }

        if (Input.touchCount == 0)
        {
            _arrow.GetComponent<RawImage>().enabled = false;
            _sTouchPos = Vector3.zero;
            _eTouchPos = Vector3.zero;
            _force = Vector3.zero;
        }
    }

    void GetTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) _sTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f)) - _camera.transform.position;
            else if (touch.phase == TouchPhase.Ended)
            {
                _eTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f)) - _camera.transform.position;

                _force = (_sTouchPos - _eTouchPos) * _canvas.scaleFactor;

                float dis = Vector3.Magnitude(_force);
                if (dis < 0.1f) return;

                _force = dis >= _swipeRange ? _force.normalized * _swipeRange : _force;
                _rigidbody.AddForce(_force * _swipeSpeed, ForceMode.VelocityChange);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                _arrow.transform.GetComponent<RawImage>().enabled = true;
                _eTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f)) - _camera.transform.position;

                Vector3 tempDir = (_sTouchPos - _eTouchPos) * _canvas.scaleFactor;
                _arrowDir = new Vector2(tempDir.x, tempDir.z).normalized;
            }
        }
    }

    //화살표 위치
    void ArrowPosition()
    {
        _arrow.anchoredPosition = new Vector2(_arrowDir.x, _arrowDir.y) * _radius;
    }

    //화살표 방향
    void ArrowRotate()
    {
        _arrow.transform.rotation = Quaternion.Euler(0, 0, Quaternion.FromToRotation(Vector2.up, _arrowDir).eulerAngles.z);
    }

    void CheckSpeed()
    {
        _rigidbody.velocity = Vector3.Magnitude(_rigidbody.velocity) >= _maxSpeed ? _rigidbody.velocity.normalized * _maxSpeed : _rigidbody.velocity;
    }

    public void HitObstacle()
    {
        _rigidbody.velocity /= 2;
    }

    public void SetPause() { _isPause = true; }

    public void ResetPause() { _isPause = false; }
}
