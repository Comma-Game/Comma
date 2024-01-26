using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    Rigidbody _rigidbody;
    Vector3 _movement;

    private void Update()
    {
        _movement = transform.up * StageController.Instance.Speed;
        CheckMaxSpeed();
    }

    public void Move()
    {
        if(!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        if (_movement.y == 0) _movement = transform.up * StageController.Instance.Speed;

        _rigidbody.velocity = new Vector3(0, StageController.Instance.Speed, 0);
        //Debug.Log("Movement : " + _movement);
        //Debug.Log("Speed : " + StageController.Instance.Speed);
    }

    public float GetVelocity()
    {
        return _rigidbody.velocity.y;
    }

    public void SetAcceleration()
    {
        _rigidbody.AddForce(_movement * StageController.Instance.AccSpeed, ForceMode.Acceleration);
    }

    void CheckMaxSpeed()
    {
        if(Mathf.Abs(_rigidbody.velocity.y) >= StageController.Instance.MaxSpeed)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Sign(_rigidbody.velocity.y) * StageController.Instance.MaxSpeed, _rigidbody.velocity.z);
        }
    }
}
