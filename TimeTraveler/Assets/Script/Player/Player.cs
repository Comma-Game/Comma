using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    float _hp, _maxHp, _energy, _energyChargeSpeed, _tempSpeed;
    bool _isInvincible, _isPassPortal, _isCast, _isHit;
    StageController _stageController;
    SaveLoadManager _saveLoadManager;
    Coroutine _coroutine;
    RaycastHit _hit;
    ColliderRange _colliderRange;
    string _obstacle;
    int _jelly;

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
            else if (_hit.transform.gameObject.CompareTag("Jelly"))
            {
                if (_hit.transform.GetComponent<Jelly>().CheckMemory())
                {
                    StageController.Instance.ScoreUp(_jelly * 2);
                    Heal(10);
                }
                else
                {
                    StageController.Instance.ScoreUp(_jelly);
                }

                StageController.Instance.AddDisabled(transform.gameObject);
                _hit.transform.gameObject.SetActive(false);
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
            collision.gameObject.GetComponent<MeshExploder>().Explode();

            _stageController.AddDisabled(collision.gameObject);
            collision.gameObject.SetActive(false);
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
        _stageController = StageController.Instance;

        _maxHp = 100 + CalculateHP();
        _hp = _maxHp;
        Debug.Log("HP : " + _hp);

        _energy = 100;
        _energyChargeSpeed = 2 + CalculateEnergy();
        Debug.Log("Energy : " + _energy);

        _jelly = 70 + CalculateJelly();
        Debug.Log("Jelly : " + _jelly);

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
        GroundDamage(20);
        _stageController.DisableStage();
    }

    public void ChargeEnergy()
    {
        if (!_isCast)
        {
            if (_energy + _energyChargeSpeed > 100)
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
            _hp -= 0.9f;
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
        if (_tempSpeed > 0f) _stageController.Speed += _tempSpeed; ;
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

    public float GetEnergy()
    {
        return _energy;
    }

    float CalculateHP()
    {
        float step = _saveLoadManager.GetUpgradeHP(), ret = 0, cnt = 0;

        while (step > 0)
        {
            float mul = step > 10 ? 10 : step;
            ret += (15 - cnt) * mul;
            step -= 10;
            cnt += 5;
        }

        return ret;
    }

    float CalculateEnergy()
    {
        float step = _saveLoadManager.GetUpgradeEnergy(), ret = 0, cnt = 0;

        while (step > 0)
        {
            float mul = step > 10 ? 10 : step;
            ret += (0.06f - cnt) * mul;
            step -= 10;
            cnt += 0.02f;
        }

        return ret;
    }

    int CalculateJelly()
    {
        int step = _saveLoadManager.GetUpgradeJelly(), ret = 0;

        if (step <= 10) ret = 10 * step;
        else if (step <= 20) {
            step -= 10;
            ret = 50 * step + 100;
        }
        else if (step <= 30)
        {
            step -= 20;
            ret = 100 * step + 600;
        }

        return ret;
    }
}
