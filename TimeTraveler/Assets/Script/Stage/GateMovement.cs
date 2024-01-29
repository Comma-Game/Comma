using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    Rigidbody _rigidbody;
    bool _isAcc, _isMove;

    private void Awake()
    {
        MakeRigidbody();
    }

    private void OnEnable()
    {
        _isMove = false;
        _isAcc = false;
    }

    private void Update()
    {
        if(_isMove) CheckMaxSpeed();
    }

    private void FixedUpdate()
    {
        if (_isAcc)
            _rigidbody.AddForce(transform.up * StageController.Instance.Speed * StageController.Instance.AccSpeed, ForceMode.Acceleration);
    }

    public void Move()
    {
        _isMove = true;
        _rigidbody.velocity = new Vector3(0, StageController.Instance.Speed, 0);
    }

    void MakeRigidbody()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StopMove()
    {
        _isMove = false;
        _isAcc = false;
        _rigidbody.velocity = Vector3.zero;
    }

    public void SetVelocity(float speed)
    {
        _rigidbody.velocity = new Vector3(0, speed, 0);
    }

    public float GetVelocity()
    {
        return _rigidbody.velocity.y;
    }

    public void SetAcceleration()
    {
        _isAcc = true;
    }

    void CheckMaxSpeed()
    {
        if(Mathf.Abs(_rigidbody.velocity.y) >= StageController.Instance.MaxSpeed)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Sign(_rigidbody.velocity.y) * StageController.Instance.MaxSpeed, _rigidbody.velocity.z);
        }
    }
}
