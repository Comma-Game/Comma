using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    [SerializeField]
    float _swipeSpeed;

    private Vector3 _sTouchPos, _eTouchPos, _force;
    Rigidbody _rigidbody;

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnEnable()
    {

    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _swipeSpeed = 10f;
    }

    void Update()
    {
        SetForce();
        //Debug.Log("Velocity : " + _rigidbody.velocity);
    }

    void SetForce()
    {
        if (Input.GetMouseButtonDown(0)) _sTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        if (Input.GetMouseButtonUp(0))
        {
            _eTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            _force = _sTouchPos - _eTouchPos;
            //Debug.Log("Force.Magnitude : " + Vector3.Magnitude(_force));
            
            _force = Vector3.Magnitude(_force) >= 1.5f ? _force.normalized * 1.5f : _force;

            _rigidbody.AddForce(_force * _swipeSpeed, ForceMode.VelocityChange);
        }
    }

    public void HitObstacle()
    {
        _rigidbody.velocity /= 2;
    }
}
