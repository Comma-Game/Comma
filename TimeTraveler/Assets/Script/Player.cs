using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int _hp, _maxHp, _energy, _energyChargeSpeed;
    bool _isInvincible, _isPassPortal, _isCast;
    StageController _stageController;
    SaveLoadManager _saveLoadManager;
    GameData _gameData;
    Coroutine _coroutine;
    RaycastHit _hit;
    ColliderRange _colliderRange;

    string _obstacle;

    private void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);
        if (Physics.Raycast(transform.position, transform.forward * 5, out _hit))
        {
            //Debug.Log("Raycast : " + _hit.transform.gameObject.name);
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
            //Debug.Log("Collision : " + collision.gameObject.name);
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
            StopMyCoroutine();
            _stageController.SetAcceleration();
        }
    }

    void Init()
    {
        transform.gameObject.SetActive(true);

        _saveLoadManager = SaveLoadManager.Instance;
        _gameData = _saveLoadManager.GameData;
        _stageController = StageController.Instance;

        _maxHp = 100000 + 20 * _saveLoadManager.GetUpgradeHP();
        _hp = _maxHp;

        _energy = 0;
        _energyChargeSpeed = 2 + 2 * _saveLoadManager.GetUpgradeEnergy();
        _isCast = false;

        _isPassPortal = false;
        _isInvincible = false;

        _colliderRange = GameObject.Find("ColliderRange").GetComponent<ColliderRange>();
        _colliderRange.ReSetInvincible();

        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    void TriggerPortal()
    {
        Heal(30);
        StopMyCoroutine();
        _coroutine = StartCoroutine(PortalTime(0.5f));
    }

    void TriggerGround()
    {
        _stageController.DestroyStage();
        if(!_isPassPortal) Damage(80);
    }

    public void ChargeEnergy()
    {
        if(!_isCast) _energy = _energy + _energyChargeSpeed > 100 ? 100 : _energy + _energyChargeSpeed;
        Debug.Log("Energy : " + _energy);
    }

    public void Heal(int amount)
    {
        _hp = _hp + amount > _maxHp ? _maxHp : _hp + amount;
        Debug.Log("HP : " + _hp);
    }

    public void TimeDamage()
    {
        if (!_isPassPortal)
        {
            _hp--;
            Debug.Log("HP : " + _hp);

            if (_hp <= 0) EndGame();
        }
    }

    public void Damage(int damage)
    {
        if (!_isInvincible)
        {
            _hp -= damage;

            SetInvincible(1);

            Debug.Log("HP : " + _hp);

            if (_hp <= 0) EndGame();
        }
    }

    public void UseSkill()
    {
        if(_energy == 100)
        {
            StopMyCoroutine();
            _coroutine = StartCoroutine(CastTime(2));
            _energy = 0;
        }
    }

    public void SetInvincible(float time)
    {
        StopMyCoroutine();
        _coroutine = StartCoroutine(InvincibleTime(time));
    }

    void EndGame()
    {
        _stageController.EndGame();
        transform.gameObject.SetActive(false);
    }

    IEnumerator PortalTime(float t)
    {
        _isPassPortal = true;
        _colliderRange.SetInvincible();

        yield return new WaitForSeconds(t);

        _colliderRange.ReSetInvincible();
        _isPassPortal = false;
    }

    IEnumerator InvincibleTime(float t)
    {
        _isInvincible = true;
        _colliderRange.SetInvincible();

        yield return new WaitForSeconds(t);

        _colliderRange.ReSetInvincible();
        _isInvincible = false;
    }

    IEnumerator CastTime(float t)
    {
        _isCast = true;
        _isInvincible = true;
        _colliderRange.SetInvincible();

        yield return new WaitForSeconds(t);

        _colliderRange.ReSetInvincible();
        _isInvincible = false;
        _isCast = false;
    }

    void StopMyCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _colliderRange.ReSetInvincible();

            _isPassPortal = false;
            _isInvincible = false;
            _isCast = false;
        }
    }

    public int GetEnergy()
    {
        return _energy;
    }

    private void OnApplicationQuit()
    {
        //SaveLoadManager.Instance.SavePlayer(_hp);
    }
}
