using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int _hp;
    bool _isInvincible;
    StageController _stageController;
    Coroutine coroutine;
    RaycastHit _hit;
    string _obstacle;

    private void OnEnable()
    {
        transform.gameObject.SetActive(true);
        Init();
        _stageController = StageController.Instance;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out _hit))
        {
            if (_hit.transform.gameObject.CompareTag("Obstacle"))
            {
                _obstacle = _hit.transform.gameObject.name;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_obstacle.Equals(collision.gameObject.name))
        {
            Damage(10);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal")) TriggerPortal();
        else if (other.gameObject.CompareTag("Ground")) TriggerGround();
        else if (other.gameObject.CompareTag("AccelerationZone"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                _isInvincible = false;
            }

            _stageController.SetAcceleration();
        }
    }

    void Init()
    {
        _hp = 100 + SaveLoadManager.Instance.GameData.upgrade_hp * 20;
        _isInvincible = false;
    }

    void TriggerPortal()
    {
        _isInvincible = true;
        coroutine = StartCoroutine(InvincibleTime(0.5f));
    }

    void TriggerGround()
    {
        _stageController.DestroyStage();
        Damage(80);
    }

    public void Damage(int damage)
    {
        if (!_isInvincible)
        {
            _hp -= damage;
            Debug.Log("HP : " + _hp);

            if (_hp <= 0) EndGame();
        }
    }

    void EndGame()
    {
        transform.gameObject.SetActive(false);
        _stageController.EndGame();
    }

    IEnumerator InvincibleTime(float t)
    {
        yield return new WaitForSeconds(t);
        _isInvincible = false;
    }

    private void OnApplicationQuit()
    {
        //SaveLoadManager.Instance.SavePlayer(_hp);
    }
}
