using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject _wind;

    [SerializeField]
    Canvas _canvas;

    float _hp, _maxHp, _bloodHp, _energy, _maxEnergy, _energyChargeSpeed, _sphereScale;
    bool _isInvincible, _isPassPortal, _isCast, _isPoison, _isBonusTime;
    Coroutine _portalCoroutine, _invincibleCoroutine, _skillCoroutine;
    ColliderRange _colliderRange;
    int _jellyScore;
    RaycastHit[] _hits;
    RaycastHit _hit;
    float _obstacleDamageBuff, _timeDamageBuff, _healBuff, _timeDamage;
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

        _timeDamage = 2f;

        _isCast = false;
        _isPassPortal = false;
        _isInvincible = false;

        Init_Buff();
        Init_PlayerSetting();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        StopAllCoroutines();

        transform.gameObject.SetActive(true);
        _colliderRange.ResetColor();

        _maxHp += CalculateHP();
        _hp = _maxHp;
        _bloodHp = _maxHp * 0.15f;

        //ü�� UI �ִ뷮 ����
        CanvasController.Instance.InitSettingHealth(_maxHp);
        //������ UI �ִ뷮 ����
        CanvasController.Instance.InitSettingEnergy(_maxEnergy);

        _energyChargeSpeed += CalculateChargeEnergy();

        _jellyScore += CalculateJelly();

        _sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        
        //�ٶ� UI Enable
        CanvasController.Instance.OnSpeedPanel(true);
        CanvasController.Instance.ChangeSpeedColor(0);
    }

    //���� �ʱ�ȭ
    void Init_Buff()
    {
        _obstacleDamageBuff = 1;
        _timeDamageBuff = 1;
        _healBuff = 1;
    }

    void Init_PlayerSetting()
    {
        _maxHp = 100;
        _maxEnergy = 100;
        _energyChargeSpeed = 5f;
        _jellyScore = 70;
        _energy = 0;
    }

    private void OnEnable()
    {
        PlayGameManager.Init_Instance();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Hit�� �������� ray�� �׷��ش�.
        Gizmos.DrawRay(transform.position, Vector3.down * _hit.distance);

        // Hit�� ������ �ڽ��� �׷��ش�.
        Gizmos.DrawWireSphere(transform.position + Vector3.down * _hit.distance, _sphereScale);
    }
    
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);
        _hits = Physics.SphereCastAll(transform.position, _sphereScale / 2, Vector3.down, 1f, 1 << LayerMask.NameToLayer("Object"));
    }

    private void OnCollisionEnter(Collision collision)
    {
        for(int i = 0; i < _hits.Length; i++)
        {
            if (!_hits[i].transform.gameObject.name.Equals(collision.gameObject.name)) continue;
            if(collision.transform.CompareTag("Obstacle"))
            {
                AudioManager.Instance.PlayDamgeSFX();

                HitDamage(10 * _obstacleDamageBuff);

                if (collision.gameObject.GetComponent<MeshExploder>() != null)
                    StageController.Instance.MakeExploder(collision.transform.parent, collision.gameObject.GetComponent<MeshExploder>().Explode());
                
                collision.gameObject.SetActive(false);

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
            StageController.Instance.SetAcceleration();
        }
        else if (other.gameObject.CompareTag("Jelly"))
        {
            if (other.gameObject.GetComponent<Jelly>().CheckMemory())
            {
                int[] jelly_info = other.gameObject.GetComponent<Jelly>().GetInfo();
                if (!SaveLoadManager.Instance.GetUnlockedMemory()[jelly_info[0]].CheckStory(jelly_info[1])) //���丮�� �رݵ��� �ʾҴٸ�
                {
                    CanvasController.Instance.OnMessagePanel(); //����� ���� �޼��� ���
                    SaveLoadManager.Instance.SetUnlockedMemory(jelly_info[0], jelly_info[1]); //�������� �� ���� ����� ���� ����
                    SaveLoadManager.Instance.SetUnOpenedStory(jelly_info[0], jelly_info[1]); //���丮 ���� Ȯ�� DB ����
                }

                Heal(10f * _healBuff);
            }
            else if (other.gameObject.GetComponent<Jelly>().CheckBigJelly())
            {
                PlayGameManager.Instance.ScoreUp(_jellyScore * 2);
                if(!_isBonusTime) ChargeEnergy(2);
            }
            else
            {
                PlayGameManager.Instance.ScoreUp(_jellyScore);
                if (!_isBonusTime) ChargeEnergy(1);
            }
            //���� �Ҹ�
            AudioManager.Instance.PlayGetPieceSFX();

            //��Ȱ��ȭ�� ������Ʈ ����Ʈ�� �־���
            StageController.Instance.AddDisabled(other.gameObject.transform.parent.gameObject);

            other.gameObject.transform.parent.gameObject.SetActive(false);

            //��ƼŬ Ȱ��ȭ
            PlayGameManager.Instance.EnableParticle(other.transform.position);
        }
        else if (other.gameObject.CompareTag("Bonus"))
        {
            PlayGameManager.Instance.ScoreUp(_jellyScore / 2);

            //���� �Ҹ�
            AudioManager.Instance.PlayGetPieceSFX();

            other.gameObject.transform.parent.gameObject.SetActive(false);

            //��ƼŬ Ȱ��ȭ
            PlayGameManager.Instance.EnableParticle(other.transform.position);

            //���ʽ� ���� ȹ��
            StageController.Instance.TriggerBonusJelly();
        }
    }

    void TriggerPortal()
    {
        _portalCoroutine = StartCoroutine(PortalTime(0.2f));
    }

    void TriggerGround()
    {
        //����ȭ �� ������Ʈ �����·� ����
        _camera.GetComponent<ShowPlayer>().SetOpaque();

        //��Ż �Ҹ�
        AudioManager.Instance.PlayPortalSFX();

        //�������� �غ�
        StageController.Instance.PrepareToStage();

        if (_isBonusTime)
        {
            //���ʽ� �������� ����
            ExitBonusStage();

            //ƽ �������� ������ ��¡�� ���� ����
            PlayGameManager.Instance.ResetBonusTime();
        }

        GroundDamage(30);
    }

    public void ChargeEnergy()
    {
        ChargeEnergy(_energyChargeSpeed);
    }

    public void ChargeEnergy(float e)
    {
        if (!_isCast)
        {
            _energy += e;
            CanvasController.Instance.PlayerUpEnergy(e);

            if (_energy >= _maxEnergy)
            {
                if (!_isInvincible && !_isPoison) _colliderRange.PrepareToSkill();
                if (!_isBonusTime) UseSkill();
            }
        }
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
            CanvasController.Instance.PlayerRestoreHP(amount);
            _hp += amount;
        }

        if (_hp > _bloodHp) CanvasController.Instance.OpenDamgePanel(false); //������ UI ��Ȱ��ȭ
    }

    public void GetDamage(float damage)
    {
        _hp -= damage;
        CanvasController.Instance.PlayerGetDamgeHP(damage);

        if(_hp <= _bloodHp) CanvasController.Instance.OpenDamgePanel(true); //������ UI Ȱ��ȭ
        if (_hp <= 0) EndGame();

        Debug.Log("HP : " + _hp);
    }

    //ƽ �׹��� ��
    public void TimeDamageUp()
    {
        _isPoison = true;
        if (!_isInvincible) _colliderRange.SetPoison();
    }

    //ƽ ������ ����
    public void ResetTimeDamage() 
    {
        _isPoison = false;
        if (!_isInvincible) ResetColor(); 
    }
    
    //ƽ ������
    public void TimeDamage()
    {
        if (!_isPassPortal && !_isInvincible) GetDamage(_timeDamage * _timeDamageBuff);
    }

    //��ֹ� ������
    void HitDamage(float damage)
    {
        if (!_isInvincible)
        {
            if (SaveLoadManager.Instance.GetHaptic()) Handheld.Vibrate(); //�޴��� ����
            _camera.GetComponent<StressReceiver>().InduceStress(0.2f); //ī�޶� ����

            if (_isMobile) transform.GetComponent<MovePlayer>().HitObstacle();
            else transform.GetComponent<TestMovePlayer>().HitObstacle();

            HitObstacle(1);

            GetDamage(damage);
        }
    }

    //�տ� ������
    void GroundDamage(float damage)
    {
        if (!_isInvincible && !_isPassPortal)
        {
            if(SaveLoadManager.Instance.GetHaptic()) Handheld.Vibrate(); //�޴��� ����
            _camera.GetComponent<StressReceiver>().InduceStress(0.2f); //ī�޶� ����
            StageController.Instance.MinusPassThroughCount(); //���ǵ� �� ���� �Ѵܰ� �϶�

            HitObstacle(1);

            GetDamage(damage);
        }
    }

    public void UseSkill()
    {
        _isCast = true;

        if (_skillCoroutine != null) StopCoroutine(_skillCoroutine);

        _skillCoroutine = StartCoroutine(CastTime(2));
    }

    void HitObstacle(float time)
    {
        if (_invincibleCoroutine != null) StopCoroutine(_invincibleCoroutine);

        StageController.Instance.MinusPassThroughCount(); //���ǵ� �� ���� �Ѵܰ� �϶�

        _invincibleCoroutine = StartCoroutine(InvincibleTime(time));
    }

    void EndGame()
    {
        //�ٶ� UI Disable
        CanvasController.Instance.OnSpeedPanel(false);

        _colliderRange.DisableRawImage();
        PlayGameManager.Instance.EndGame();

        StopAllCoroutines();
    }

    public void DestroyPlayer() { transform.gameObject.SetActive(false); }

    void ResetColor()
    {
        if (_isPoison) _colliderRange.SetPoison();
        else if (_energy < _maxEnergy) _colliderRange.ResetColor();
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
        
        //�ٶ� UI Disable
        CanvasController.Instance.OnSpeedPanel(false);

        yield return new WaitForSeconds(t);

        //�ٶ� UI Enable
        CanvasController.Instance.OnSpeedPanel(true);
        
        _isInvincible = false;
        ResetColor();
    }

    IEnumerator CastTime(float t)
    {
        yield return new WaitForSeconds(1);

        if (_invincibleCoroutine != null) StopCoroutine(_invincibleCoroutine);
        _isInvincible = true;

        //��ų ���� UI �� �Ҹ� Ȱ��ȭ
        CanvasController.Instance.OnSpeedPanel(true);
        CanvasController.Instance.ChangeSpeedColor(1);
        AudioManager.Instance.PlaySkillSFX();

        //��ų ������ ����
        _colliderRange.PrepareToSkill();
        _colliderRange.SetSkill();

        //��ų ����Ʈ Ȱ��ȭ
        MyPostProcess.Instance.EnableSkillEffect();

        //��ų ��� ���� ���ǵ�
        float prevSpeed = StageController.Instance.GetStageVelocity();

        //�ִ� �ӵ��� ����
        StageController.Instance.SetSkill();

        //��ų �ִϸ��̼� ����
        _animator.SetBool("UseSkill", true);

        //������ 0���� ����
        CanvasController.Instance.PlayerDownEnergy(_energy);
        _energy = 0;

        yield return new WaitForSeconds(t);

        //���� �ִϸ��̼� ����
        _animator.SetBool("UseSkill", false);

        MyPostProcess.Instance.DisableSkillEffect();

        CanvasController.Instance.ChangeSpeedColor(0);

        _colliderRange.ResetSkill();

        StageController.Instance.ResetSkill(prevSpeed);

        _isCast = false;

        if (_invincibleCoroutine != null) StopCoroutine(_invincibleCoroutine);
        _invincibleCoroutine = StartCoroutine(InvincibleTime(2));
    }

    //���� �� ��ų �ߴ�
    public void StopMyCoroutine()
    {
        ResetColor();

        if (_portalCoroutine != null) StopCoroutine(_portalCoroutine);

        if (_invincibleCoroutine != null)
        {
            StopCoroutine(_invincibleCoroutine);
            CanvasController.Instance.OnSpeedPanel(true);
        }

        if (_skillCoroutine != null)
        {
            _animator.speed = 1;
            MyPostProcess.Instance.DisableSkillEffect();

            _animator.SetBool("UseSkill", false);
            CanvasController.Instance.ChangeSpeedColor(0);

            StopCoroutine(_skillCoroutine);
        }

        _isPassPortal = false;
        _isInvincible = false;
        _isCast = false;
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
        return step * 0.1f;
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

    public void SetObstacleDamageBuff() { _obstacleDamageBuff -= _obstacleDamageBuff * 0.5f; }

    public void SetTimeDamageBuff() { _timeDamageBuff -= _timeDamageBuff * 0.15f; }

    public void PlusMaxHp(float amount) 
    { 
        _maxHp += amount;
        _hp = _maxHp;
        _bloodHp = _maxHp * 0.15f;
        CanvasController.Instance.InitSettingHealth(_maxHp);
    }

    public void SetHealBuff(float amount) { _healBuff += _healBuff * amount; }

    public void SetEnergyBuff()
    {
        _maxEnergy *= 0.8f;

        //������ UI �ִ뷮 ����
        CanvasController.Instance.InitSettingEnergy(_maxEnergy);
    }

    public void EnterBonusStage() { _isBonusTime = true; }
    public void ExitBonusStage() { _isBonusTime = false; }

    public float GetHp() { return _hp; }

    //���ʽ� �������� ���� �� �÷��̾� �̵� ���� ���
    public void EnablePlayerMove()
    {
        transform.GetComponent<MovePlayer>().enabled = true;
    }
    public void DisablePlayerMove()
    {
        transform.GetComponent<MovePlayer>().enabled = false;
    }
}