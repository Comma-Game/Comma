using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int _hp;
    bool _isInvincible, _isPassPortal;
    StageController _stageController;
    Coroutine _coroutine;
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
        if (Physics.Raycast(transform.position, transform.forward * 5, out _hit))
        {
            Debug.Log("Raycast : " + _hit.transform.gameObject.name);
            if (_hit.transform.gameObject.CompareTag("Obstacle"))
            {
                _obstacle = _hit.transform.gameObject.name;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_obstacle == null) return;
        if(_obstacle.Equals(collision.gameObject.name))
        {
            Debug.Log("Collision : " + collision.gameObject.name);
            Damage(10);
            collision.gameObject.GetComponent<MeshExploder>().Explode();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal")) TriggerPortal();
        else if (other.gameObject.CompareTag("Ground")) TriggerGround();
        else if (other.gameObject.CompareTag("AccelerationZone"))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _isPassPortal = false;
                _isInvincible = false;
            }

            _stageController.SetAcceleration();
        }
    }

    void Init()
    {
        _hp = 10000000 + SaveLoadManager.Instance.GameData.upgrade_hp * 20;
        _isPassPortal = false;
        _isInvincible = false;
    }

    void TriggerPortal()
    {
        _coroutine = StartCoroutine(PortalTime(0.5f));
    }

    void TriggerGround()
    {
        _stageController.DestroyStage();
        if(!_isPassPortal) Damage(80);
    }

    public void Damage(int damage)
    {
        if (!_isInvincible)
        {
            _hp -= damage;
            _coroutine = StartCoroutine(InvincibleTime(2));

            Debug.Log("HP : " + _hp);

            if (_hp <= 0) EndGame();
        }
    }

    void EndGame()
    {
        transform.gameObject.SetActive(false);
        _stageController.EndGame();
    }

    IEnumerator PortalTime(float t)
    {
        _isPassPortal = true;
        yield return new WaitForSeconds(t);
        _isPassPortal = false;
    }

    IEnumerator InvincibleTime(float t)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(t);
        _isInvincible = false;
    }

    private void OnApplicationQuit()
    {
        //SaveLoadManager.Instance.SavePlayer(_hp);
    }
}
