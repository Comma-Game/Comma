using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    [SerializeField]
    GameObject _memory;

    Rigidbody _rigidbody;
    bool _isMove, _isBonusTime;

    private void Awake()
    {
        MakeRigidbody();
    }

    private void OnEnable()
    {
        _isMove = false;
    }

    private void OnDisable()
    {
        if (_memory.activeSelf) _memory.SetActive(false);
    }

    private void Update()
    {
        if (_isMove) CheckMaxSpeed();
    }

    private void FixedUpdate()
    {
        if (_isMove && !_isBonusTime) _rigidbody.AddForce(transform.up * StageController.Instance.AccSpeed, ForceMode.Acceleration);
    }

    void MakeRigidbody()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StopMove()
    {
        _rigidbody.velocity = Vector3.zero;
        _isMove = false;
    }

    public void SetVelocity(float speed)
    {
        _isMove = true;
        _isBonusTime = false;
        _rigidbody.velocity = new Vector3(0, speed, 0);
    }

    public void SetBonusVelocity(float speed)
    {
        _isMove = true;
        _isBonusTime = true;
        _rigidbody.velocity = new Vector3(0, speed, 0);
    }

    public float GetVelocity()
    {
        return _rigidbody.velocity.y;
    }

    void CheckMaxSpeed()
    {

        if (Mathf.Abs(_rigidbody.velocity.y) >= StageController.Instance.GetMaxSpeed())
        {
            //Mathf.Sign(_rigidbody.velocity.y) : y >= 0 -> 1, y < 0 -> -1 
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Sign(_rigidbody.velocity.y) * StageController.Instance.GetMaxSpeed(), _rigidbody.velocity.z);
        }
    }

    public void EnableMemory()
    {
        _memory.SetActive(true);
    }
}
