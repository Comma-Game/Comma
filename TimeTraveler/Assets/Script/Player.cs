using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    int _hp, _maxHp, _energy, _energyChargeSpeed;
    bool _isInvincible, _isPassPortal, _isCast, _isHit;
    StageController _stageController;
    SaveLoadManager _saveLoadManager;
    GameData _gameData;
    Coroutine _coroutine;
    RaycastHit _hit;
    ColliderRange _colliderRange;
    float _tempSpeed;
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
        if (_obstacle.Equals(collision.gameObject.name))
        {
            //Debug.Log("Collision : " + collision.gameObject.name);
            HitDamage(10);
            //collision.gameObject.GetComponent<MeshExploder>().Explode();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal")) TriggerPortal();
        else if (other.gameObject.CompareTag("Ground")) TriggerGround();
        else if (other.gameObject.CompareTag("AccelerationZone"))
        {
            //StopMyCoroutine();
            if (_isPassPortal) _isPassPortal = false;
            _stageController.SetAcceleration();
        }
    }

    void Init()
    {
        transform.gameObject.SetActive(true);

        _saveLoadManager = SaveLoadManager.Instance;
        _gameData = _saveLoadManager.GameData;
        _stageController = StageController.Instance;

        _maxHp = 100 + 20 * _saveLoadManager.GetUpgradeHP();
        _hp = _maxHp;
        Debug.Log("HP : " + _hp);

        _energy = 100;
        _energyChargeSpeed = 2 + 2 * _saveLoadManager.GetUpgradeEnergy();

        _tempSpeed = 0;

        _isCast = false;
        _isPassPortal = false;
        _isInvincible = false;

        _colliderRange = GameObject.Find("ColliderRange").GetComponent<ColliderRange>();
        _colliderRange.ReSetColor();

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
        GroundDamage(80);
    }

    public void ChargeEnergy()
    {
        if (!_isCast)
        {
            if(_energy + _energyChargeSpeed > 100)
            {
                _energy = 100;
                CanvasController.Instance.PlayerUpEnergy(100 - _energyChargeSpeed);
            }
            else
            {
                _energy += _energyChargeSpeed;
                CanvasController.Instance.PlayerUpEnergy(_energyChargeSpeed);
            }
        }
        
        //Debug.Log("Energy : " + _energy);
    }

    public void Heal(int amount)
    {
        if (_hp + amount > _maxHp)
        {
            _hp = _maxHp;
            CanvasController.Instance.PlayerRestoreHP(_maxHp - amount);
        } 
        else
        {
            _hp += amount;
            CanvasController.Instance.PlayerRestoreHP(amount);
        }

        //Debug.Log("HP : " + _hp);
    }

    public void TimeDamage()
    {
        if (!_isPassPortal && !_isInvincible)
        {
            _hp--;
            CanvasController.Instance.PlayerGetDamgeHP(1);

            //Debug.Log("HP : " + _hp);

            if (_hp <= 0) EndGame();
        }
    }

    void HitDamage(int damage)
    {
        if (!_isHit && !_isInvincible & !_isCast)
        {
            _hp -= damage;
            CanvasController.Instance.PlayerGetDamgeHP(damage);

            HitObstacle(1);

            //Debug.Log("Remain HP : " + _hp);

            if (_hp <= 0) EndGame();
        }
    } 

    void GroundDamage(int damage)
    {
        if (!_isPassPortal)
        {
            _hp -= damage;
            CanvasController.Instance.PlayerGetDamgeHP(damage);

            SetInvincible(1);

            //Debug.Log("HP : " + _hp);

            if (_hp <= 0) EndGame();
        }
    }

    public void UseSkill()
    {
        if (_energy == 100)
        {
            StopMyCoroutine();
            _coroutine = StartCoroutine(CastTime(2));
            _energy = 0;
            CanvasController.Instance.PlayerDownEnergy(100);
        }
    }

    void HitObstacle(float time)
    {
        StopMyCoroutine();
        _coroutine = StartCoroutine(HitObstacleTime(time));
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

        yield return new WaitForSeconds(t);

        _isPassPortal = false;
    }

    IEnumerator InvincibleTime(float t)
    {
        _isInvincible = true;
        _colliderRange.SetInvincible();

        yield return new WaitForSeconds(t);

        _colliderRange.ReSetColor();
        _isInvincible = false;
    }

    IEnumerator CastTime(float t)
    {
        _isCast = true;
        _isInvincible = true;
        _colliderRange.SetSkill();

        yield return new WaitForSeconds(t);

        _colliderRange.ReSetColor();
        _isInvincible = false;
        _isCast = false;
    }

    IEnumerator HitObstacleTime(float t)
    {
        _isHit = true;
        if(_tempSpeed > 0f) _stageController.Speed += _tempSpeed; ;
        _tempSpeed = _stageController.GetStageVelocity() / 2;
        
        _stageController.Speed -= _tempSpeed;
        _stageController.SetStagesVelocity();
        _colliderRange.SetInvincible();

        yield return new WaitForSeconds(t);

        _stageController.Speed += _tempSpeed;
        _stageController.SetStagesVelocity();
        _colliderRange.ReSetColor();

        _tempSpeed = 0f;
        _isHit = false;
    }

    void StopMyCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            if (_isHit)
            {
                _stageController.Speed += _tempSpeed;
                
                _tempSpeed = 0f;
                _isHit = false;
            }

            _colliderRange.ReSetColor();

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
