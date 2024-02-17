using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    [SerializeField]
    GameObject _memory;

    Rigidbody _rigidbody;
    bool _isAcc, _isMove, _isBonusTime, _isSkill;

    private void Awake()
    {
        MakeRigidbody();
    }

    private void OnEnable()
    {
        _isMove = false;
        _isAcc = false;
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
        //if (_isAcc) _rigidbody.AddForce(transform.up * StageController.Instance.Speed * StageController.Instance.AccSpeed, ForceMode.Acceleration);
        if (_isSkill) _rigidbody.AddForce(transform.up * GetVelocity() * 0.5f, ForceMode.Acceleration);
        else if (_isMove && !_isBonusTime) _rigidbody.AddForce(transform.up * StageController.Instance.AccSpeed, ForceMode.Acceleration);
    }

    void MakeRigidbody()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StopMove()
    {
        _rigidbody.velocity = Vector3.zero;
        _isMove = false;
        _isAcc = false;
        _isSkill = false;
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

    public void SetAcceleration()
    {
        _isAcc = true;
    }

    void CheckMaxSpeed()
    {

        if (_isSkill)
        {
            if (Mathf.Abs(_rigidbody.velocity.y) >= StageController.Instance.GetSkillSpeed())
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Sign(_rigidbody.velocity.y) * StageController.Instance.GetSkillSpeed(), _rigidbody.velocity.z);
        }
        else if (Mathf.Abs(_rigidbody.velocity.y) >= StageController.Instance.GetMaxSpeed())
        {
            //Mathf.Sign(_rigidbody.velocity.y) : y >= 0 -> 1, y < 0 -> -1 
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Sign(_rigidbody.velocity.y) * StageController.Instance.GetMaxSpeed(), _rigidbody.velocity.z);
        }
    }

    public void EnableMemory()
    {
        _memory.SetActive(true);
    }

    public void SetSkill()
    {
        _isSkill = true;
    }

    public void ResetSkill()
    {
        _isSkill = false;
    }
}
