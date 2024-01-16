using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    bool _isAcceleration;
    Rigidbody _rigidbody;
    Vector3 _movement;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _movement = transform.up * StageController.Instance.Speed;
        Move();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void FixedUpdate()
    {
        if (_isAcceleration) _rigidbody.AddForce(_movement * StageController.Instance.AccSpeed, ForceMode.Acceleration);
        CheckMaxSpeed();
    }

    public void Move()
    {
        _rigidbody.AddForce(_movement, ForceMode.VelocityChange);
    }

    public void SetAcceleration()
    {
         _isAcceleration = true;
    }

    public void UnsetAcceleration()
    {
        _rigidbody.velocity = Vector3.zero;
        _isAcceleration = false;
        Move();
    }

    void CheckMaxSpeed()
    {
        if(Mathf.Abs(_rigidbody.velocity.y) >= StageController.Instance.MaxSpeed)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Sign(_rigidbody.velocity.y) * StageController.Instance.MaxSpeed, _rigidbody.velocity.z);
        }
    }
}
