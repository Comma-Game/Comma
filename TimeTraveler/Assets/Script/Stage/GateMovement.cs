using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    Rigidbody _rigidbody;
    bool _isAcc;

    private void Awake()
    {
        MakeRigidbody();
    }

    private void Start()
    {
        _isAcc = false;
    }

    private void Update()
    {
        Debug.Log("Speed : " + _rigidbody.velocity.y);
        CheckMaxSpeed();
    }

    private void FixedUpdate()
    {
        if (_isAcc)
            _rigidbody.AddForce(transform.up * StageController.Instance.Speed * StageController.Instance.AccSpeed, ForceMode.Acceleration);
    }

    public void Move()
    {
        //if (_rigidbody == null) MakeRigidbody();
        _isAcc = false;
        _rigidbody.velocity = new Vector3(0, StageController.Instance.Speed, 0);
        //Debug.Log("Movement : " + _movement);
        //Debug.Log("Speed : " + StageController.Instance.Speed);
    }

    void MakeRigidbody()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StopMove()
    {
        //if (_rigidbody == null) MakeRigidbody();
        _rigidbody.velocity = new Vector3(0, 0, 0);
    }

    public void AddVelocity(float speed)
    {
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y + speed, 0);
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
