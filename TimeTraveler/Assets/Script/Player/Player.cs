using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Canvas _canvas;

    float _hp, _maxHp, _energy, _energyChargeSpeed, _tempSpeed, _sphereScale;
    bool _isInvincible, _isPassPortal, _isCast, _isHit;
    PlayGameManager _playGameManager;
    Coroutine _coroutine;
    ColliderRange _colliderRange;
    int _jellyScore;
    RaycastHit[] _hits;
    RaycastHit _hit;
    float _obstacleDamageBuff, _timeDamageBuff, _healBuff, _timeDamage, _iniTimeDamage;
    SaveLoadManager _saveLoadManager;
    GameObject _camera;
    Animator _animator;

    //TestControlButton ���ָ� ���� ���� ����
    bool _isMobile;
    private void Update()
    {
        _isMobile = transform.GetComponent<MovePlayer>().enabled;
    }


    private void Awake()
    {
        _colliderRange = GameObject.Find("ColliderRange").GetComponent<ColliderRange>();
        _camera = GameObject.Find("Main Camera");
        _animator = transform.GetChild(0).GetComponent<Animator>();

        _tempSpeed = 0;
        _iniTimeDamage = 0.9f;
        _timeDamage = _iniTimeDamage;

        _isCast = false;
        _isPassPortal = false;
        _isInvincible = false;

        Init_Buff();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        _saveLoadManager = SaveLoadManager.Instance;

        transform.gameObject.SetActive(true);
        _colliderRange.EnableRawImage();

        _maxHp = 100 + CalculateHP();
        _hp = _maxHp;
        CanvasController.Instance.InitSetting(_maxHp);
        Debug.Log("HP : " + _hp);

        _energy = 100;

        //�׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _energyChargeSpeed = 8 + CalculateChargeEnergy();
        Debug.Log("Energy : " + _energy);

        _jellyScore = 70 + CalculateJelly();
        Debug.Log("Jelly : " + _jellyScore);

        _sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    private void OnEnable()
    {
        _playGameManager = PlayGameManager.Instance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Hit�� �������� ray�� �׷��ش�.
        Gizmos.DrawRay(transform.position, transform.forward * _hit.distance);

        // Hit�� ������ �ڽ��� �׷��ش�.
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

                AudioManager.Instance.PlayDamgeSFX();

                if (collision.gameObject.GetComponent<MeshExploder>() != null)
                    StageController.Instance.MakeExploder(collision.transform.parent, collision.gameObject.GetComponent<MeshExploder>().Explode());

                StageController.Instance.AddDisabled(collision.gameObject);
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
            StageController.Instance.DisablePrevStage();
            _camera.GetComponent<ShowPlayer>().SetOpaque();

            _isPassPortal = false;
            CanvasController.Instance.OnSpeedPanel(true);
            StageController.Instance.SetAcceleration();
        }
        else if (other.gameObject.CompareTag("Jelly"))
        {
            if (other.gameObject.GetComponent<Jelly>().CheckMemory())
            {
                //���� �Ҹ�
                AudioManager.Instance.PlayGetPieceSFX();
                PlayGameManager.Instance.ScoreUp(_jellyScore * 2);

                int[] jelly_info = other.gameObject.GetComponent<Jelly>().GetInfo();
                if (!SaveLoadManager.Instance.GetUnlockedMemory()[jelly_info[0]][jelly_info[1]])
                {
                    CanvasController.Instance.OnMessagePanel(); //����� ���� �޼��� ���
                    SaveLoadManager.Instance.SetUnlockedMemory(jelly_info[0], jelly_info[1]); //�������� �� ���� ����� ���� ����
                }

                //�׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                Heal(5f * _healBuff);
            }
            else
            {
                //���� �Ҹ�
                AudioManager.Instance.PlayGetPieceSFX();
                PlayGameManager.Instance.ScoreUp(_jellyScore);
            }

            other.gameObject.GetComponent<Jelly>().GetParent().SetActive(false);
            
            //��ƼŬ Ȱ��ȭ
            PlayGameManager.Instance.EnableParticle(other.transform.position);

            //��Ȱ��ȭ�� ������Ʈ ����Ʈ�� �־���
            StageController.Instance.AddDisabled(other.gameObject.GetComponent<Jelly>().GetParent());
        }
    }

    //���� �ʱ�ȭ
    void Init_Buff()
    {
        _obstacleDamageBuff = 1;
        _timeDamageBuff = 1;
        _healBuff = 1;
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

        //��Ż �Ҹ�
        AudioManager.Instance.PlayPortalSFX();

        //�ٶ� UI
        CanvasController.Instance.OnSpeedPanel(false);
        CanvasController.Instance.ChangeSpeedColor(0);

        //�������� �غ�
        StageController.Instance.PrepareToStage();
    }

    public void ChargeEnergy()
    {
        if (!_isCast)
        {
            if (_energy + _energyChargeSpeed > 100)
            {
                CanvasController.Instance.PlayerUpEnergy(100 - _energy);
                _energy = 100;
                _colliderRange.PrepareToSkill();
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
            CanvasController.Instance.PlayerRestoreHP(_maxHp - _hp);
            _hp = _maxHp;
        }
        else
        {
            _hp += amount;
            CanvasController.Instance.PlayerRestoreHP(amount);
        }

        //Debug.Log("HP : " + _hp);
        if (_hp > 50) CanvasController.Instance.OpenDamgePanel(false); //������ UI ��Ȱ��ȭ
    }

    public void GetDamage(float damage)
    {
        _hp -= damage;
        CanvasController.Instance.PlayerGetDamgeHP(damage);

        if(_hp <= 50) CanvasController.Instance.OpenDamgePanel(true); //������ UI Ȱ��ȭ
        Debug.Log("HP : " + _hp);
    }

    //ƽ �׹��� ��
    public void TimeDamageUp(int mul) 
    {
        if (_timeDamage == _iniTimeDamage)
        {
            _timeDamage *= mul;
            _colliderRange.SetPoison();
        }
    }

    //ƽ ������ ����
    public void ResetTimeDamage()
    {
        if(_timeDamage != _iniTimeDamage)
        {
            _timeDamage = _iniTimeDamage;
            _colliderRange.ReSetColor();
        }
    }
    
    //ƽ ������
    public void TimeDamage()
    {
        if (!_isPassPortal && !_isInvincible)
        {
            GetDamage(_timeDamage * _timeDamageBuff);

            if (_hp <= 0) EndGame();
        }
    }

    //��ֹ� ������
    void HitDamage(float damage)
    {
        if (!_isHit && !_isInvincible & !_isCast)
        {
            GetDamage(damage);
            Handheld.Vibrate(); //�޴��� ����
            _camera.GetComponent<StressReceiver>().InduceStress(0.2f); //ī�޶� ����

            if (_isMobile) transform.GetComponent<MovePlayer>().HitObstacle();
            else transform.GetComponent<TestMovePlayer>().HitObstacle();

            HitObstacle(1);

            if (_hp <= 0) EndGame();
        }
    }

    //�տ� ������
    void GroundDamage(float damage)
    {
        if (!_isPassPortal)
        {
            GetDamage(damage);
            Handheld.Vibrate(); //�޴��� ����
            _camera.GetComponent<StressReceiver>().InduceStress(0.2f); //ī�޶� ����
            StageController.Instance.MinusPassThroughCount(); //���ǵ� �� ���� �Ѵܰ� �϶�

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
        StageController.Instance.MinusPassThroughCount(); //���ǵ� �� ���� �Ѵܰ� �϶�

        StopMyCoroutine();
        _coroutine = StartCoroutine(HitObstacleTime(time));
    }

    void SetInvincible(float time)
    {
        StopMyCoroutine();
        _coroutine = StartCoroutine(InvincibleTime(time));
    }

    void EndGame()
    {
        CanvasController.Instance.OnSpeedPanel(false);

        _colliderRange.DisableRawImage();
        PlayGameManager.Instance.EndGame();

        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    public void DestroyPlayer() { transform.gameObject.SetActive(false); }

    void ResetColor()
    {
        if(_energy < 100) _colliderRange.ReSetColor();
        else _colliderRange.PrepareToSkill();
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

        ResetColor();

        _isInvincible = false;
    }

    IEnumerator CastTime(float t)
    {
        _isCast = true;
        _isInvincible = true;

        //�ִϸ��̼� ����
        _animator.speed = 0;

        //��ų ��ƼŬ Ȱ��ȭ
        _colliderRange.GetComponent<ColliderRange>().EnableSkillEffect();

        //��ų ���� UI �� �Ҹ� Ȱ��ȭ
        CanvasController.Instance.OnSpeedPanel(true);
        CanvasController.Instance.ChangeSpeedColor(1);
        AudioManager.Instance.PlaySkillSFX();

        //��ų ������ ����
        _colliderRange.SetSkill();

        //�ִ� �ӵ��� ����
        StageController.Instance.SetVelocity(StageController.Instance.MaxSpeed);

        yield return new WaitForSeconds(t);

        //�⺻���� �ٽ� �ʱ�ȭ
        _animator.speed = 1;

        _colliderRange.GetComponent<ColliderRange>().DisableSkillEffect();

        CanvasController.Instance.ChangeSpeedColor(0);
        StageController.Instance.ResetVelocity();

        _colliderRange.ReSetColor();

        _isInvincible = false;
        _isCast = false;
    }

    IEnumerator HitObstacleTime(float t)
    {
        _isHit = true;
        _tempSpeed = StageController.Instance.GetStageVelocity() / 2;

        StageController.Instance.SetVelocity(StageController.Instance.BasicSpeed);
        _colliderRange.SetInvincible();

        yield return new WaitForSeconds(t);

        ResetColor();

        _isHit = false;
    }

    //���� �� ��ų �ߴ�
    void StopMyCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);

            ResetColor();

            if (_isCast)
            {
                _animator.speed = 1;
                _colliderRange.GetComponent<ColliderRange>().DisableSkillEffect();

                CanvasController.Instance.ChangeSpeedColor(0);
                StageController.Instance.ResetVelocity();
            }

            _isCast = false;
            _isHit = false;
            _isPassPortal = false;
            _isInvincible = false;
        }
    }

    float CalculateHP()
    {
        int step = SaveLoadManager.Instance.GetUpgradeHP(), ret = 0, cnt = 0;

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
        int step = SaveLoadManager.Instance.GetUpgradeEnergy();
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
        int step = SaveLoadManager.Instance.GetUpgradeJelly(), ret = 0;

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

    public void SetHealBuff(float amount) { _timeDamageBuff += amount; }

    public void SetEnergyDeBuff()
    {
        _energy *= 0.8f;
        CanvasController.Instance.PlayerUpEnergy(-_energy * 0.2f);
    }

    public float GetHp() { return _hp; }
}