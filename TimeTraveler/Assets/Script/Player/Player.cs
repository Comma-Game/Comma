using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject _camera;

    float _hp, _maxHp, _energy, _energyChargeSpeed, _tempSpeed, _sphereScale;
    bool _isInvincible, _isPassPortal, _isCast, _isHit;
    StageController _stageController;
    SaveLoadManager _saveLoadManager;
    Coroutine _coroutine;
    ColliderRange _colliderRange;
    int _jellyScore;
    RaycastHit[] _hits;
    RaycastHit _hit;
    float _obstacleDamageBuff, _timeDamageBuff, _healBuff;

    private void Awake()
    {
        _colliderRange = GameObject.Find("ColliderRange").GetComponent<ColliderRange>();

        _tempSpeed = 0;

        _isCast = false;
        _isPassPortal = false;
        _isInvincible = false;

        Init_Buff();
    }

    private void Start()
    {
        Init();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Hit된 지점까지 ray를 그려준다.
        Gizmos.DrawRay(transform.position, transform.forward * _hit.distance);

        // Hit된 지점에 박스를 그려준다.
        Gizmos.DrawWireSphere(transform.position + transform.forward * _hit.distance, _sphereScale);
    }
    
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
        _hits = Physics.SphereCastAll(transform.position, _sphereScale / 2, transform.forward, 1f, 1 << LayerMask.NameToLayer("Object"));
    }

    private void OnCollisionEnter(Collision collision)
    {
        for(int i = 0; i < _hits.Length; i++)
        {
            if (!_hits[i].transform.gameObject.name.Equals(collision.gameObject.name)) continue;
            if(collision.transform.CompareTag("Obstacle"))
            {
                collision.gameObject.SetActive(false);
                HitDamage(10 * _obstacleDamageBuff);

                if(collision.gameObject.GetComponent<MeshExploder>() != null)
                    _stageController.MakeExploder(collision.transform.parent, collision.gameObject.GetComponent<MeshExploder>().Explode());
                
                
                _stageController.AddDisabled(collision.gameObject);
            }
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
            _camera.GetComponent<ShowPlayer>().SetOpaque();
        }
        else if (other.gameObject.CompareTag("Jelly"))
        {
            if (other.gameObject.GetComponent<Jelly>().CheckMemory())
            {
                PlayGameManager.Instance.ScoreUp(_jellyScore * 2);
                Heal(1.8f * _healBuff);
                //CanvasController.Instance.OpenMessagePanel();
            }
            else
            {
                PlayGameManager.Instance.ScoreUp(_jellyScore);
            }

            other.gameObject.SetActive(false);
            StageController.Instance.AddDisabled(other.gameObject);
        }
    }

    void Init_Buff()
    {
        _obstacleDamageBuff = 1;
        _timeDamageBuff = 1;
        _healBuff = 1;
    }

    void Init()
    {
        transform.gameObject.SetActive(true);
        _colliderRange.EnableRawImage();

        _saveLoadManager = SaveLoadManager.Instance;
        _stageController = StageController.Instance;

        _maxHp = 100 + CalculateHP();
        _hp = _maxHp;
        CanvasController.Instance.InitSetting(_maxHp);
        Debug.Log("HP : " + _hp);

        _energy = 100;
        _energyChargeSpeed = 4 + CalculateChargeEnergy();
        Debug.Log("Energy : " + _energy);

        _jellyScore = 70 + CalculateJelly();
        Debug.Log("Jelly : " + _jellyScore);

        _sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    void TriggerPortal()
    {
        Heal(1.8f);
        StopMyCoroutine();
        _coroutine = StartCoroutine(PortalTime(0.5f));
    }

    void TriggerGround()
    {
        GroundDamage(20);
        _stageController.DisableStage();
        StopMyCoroutine();
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

    public void Heal(float amount)
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
        if (_hp > 50) CanvasController.Instance.OpenDamgePanel(false);
    }

    public void GetDamage(float damage)
    {
        _hp -= damage;
        CanvasController.Instance.PlayerGetDamgeHP(damage);

        if(_hp <= 50) CanvasController.Instance.OpenDamgePanel(true);
        Debug.Log("HP : " + _hp);
    }

    public void TimeDamage()
    {
        if (!_isPassPortal && !_isInvincible)
        {
            GetDamage(0.9f * _timeDamageBuff);

            if (_hp <= 0) EndGame();
        }
    }

    void HitDamage(float damage)
    {
        if (!_isHit && !_isInvincible & !_isCast)
        {
            GetDamage(damage);

            transform.GetComponent<TestMovePlayer>().HitObstacle();
            //transform.GetComponent<MovePlayer>().HitObstacle();

            HitObstacle(1);

            if (_hp <= 0) EndGame();
        }
    }

    void GroundDamage(float damage)
    {
        if (!_isPassPortal)
        {
            GetDamage(damage);

            SetInvincible(1);

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
        _colliderRange.DisableRawImage();
        _stageController.EndGame();
        PlayGameManager.Instance.EndGame();
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
        _stageController.SetVelocity(_stageController.MaxSpeed);

        yield return new WaitForSeconds(t);

        _stageController.ResetVelocity();
        _colliderRange.ReSetColor();

        _isInvincible = false;
        _isCast = false;
    }

    IEnumerator HitObstacleTime(float t)
    {
        _isHit = true;
        _tempSpeed = _stageController.GetStageVelocity() / 2;

        _stageController.SetVelocity(_tempSpeed);
        _colliderRange.SetInvincible();

        yield return new WaitForSeconds(t);

        _stageController.ResetVelocity();
        _colliderRange.ReSetColor();

        _isHit = false;
    }

    void StopMyCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            if (_isHit)
            {
                _isHit = false;

                _stageController.ResetVelocity();
            }

            _colliderRange.ReSetColor();

            _isPassPortal = false;
            _isInvincible = false;
            _isCast = false;
        }
    }

    float CalculateHP()
    {
        int step = _saveLoadManager.GetUpgradeHP(), ret = 0, cnt = 0;

        while (step > 0)
        {
            int mul = step > 10 ? 10 : step;
            ret += (15 - cnt) * mul;
            step -= 10;
            cnt += 5;
        }

        return ret;
    }

    float CalculateChargeEnergy()
    {
        int step = _saveLoadManager.GetUpgradeEnergy();
        float ret = 0, cnt = 0;

        while (step > 0)
        {
            int mul = step > 10 ? 10 : step;
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

    public void SetObstacleDamageBuff() { _obstacleDamageBuff -= 0.5f; }

    public void SetTimeDamageBuff() { _timeDamageBuff -= 0.15f; }

    public void SetHealBuff() { _timeDamageBuff += 0.2f; }

    public void SetEnergyDeBuff()
    {
        _energy *= 0.8f;
        CanvasController.Instance.PlayerUpEnergy(-_energy * 0.2f);
    }

    public float GetHp() { return _hp; }
}
