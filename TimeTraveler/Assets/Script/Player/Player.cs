using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject _wind;

    [SerializeField]
    Canvas _canvas;

    float _hp, _maxHp, _bloodHp, _energy, _energyChargeSpeed, _sphereScale;
    bool _isInvincible, _isPassPortal, _isCast, _isPoison;
    PlayGameManager _playGameManager;
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
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        StopAllCoroutines();

        transform.gameObject.SetActive(true);
        _colliderRange.EnableRawImage();

        _maxHp += 100 + CalculateHP();
        _hp = _maxHp;
        _bloodHp = _maxHp * 0.15f;
        CanvasController.Instance.InitSetting(_maxHp);

        _energy = 100;

        //�׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _energyChargeSpeed = 8 + CalculateChargeEnergy();

        _jellyScore = 70 + CalculateJelly();

        _sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        
        //�ٶ� UI Enable
        CanvasController.Instance.OnSpeedPanel(true);
    }

    private void OnEnable()
    {
        _playGameManager = PlayGameManager.Instance;
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
            //StopMyCoroutine();
            StageController.Instance.SetAcceleration();
        }
        else if (other.gameObject.CompareTag("Jelly"))
        {
            if (other.gameObject.GetComponent<Jelly>().CheckMemory())
            {
                //PlayGameManager.Instance.ScoreUp(_jellyScore * 2);

                int[] jelly_info = other.gameObject.GetComponent<Jelly>().GetInfo();
                if (!SaveLoadManager.Instance.GetUnlockedMemory()[jelly_info[0]].CheckStory(jelly_info[1]))
                {
                    CanvasController.Instance.OnMessagePanel(); //����� ���� �޼��� ���
                    SaveLoadManager.Instance.SetUnlockedMemory(jelly_info[0], jelly_info[1]); //�������� �� ���� ����� ���� ����
                }

                Heal(15f * _healBuff);
            }
            else if(other.gameObject.GetComponent<Jelly>().CheckBigJelly()) PlayGameManager.Instance.ScoreUp(_jellyScore * 2);
            else PlayGameManager.Instance.ScoreUp(_jellyScore);

            //���� �Ҹ�
            AudioManager.Instance.PlayGetPieceSFX();

            other.gameObject.GetComponent<Jelly>().GetParent().SetActive(false);
            
            //��ƼŬ Ȱ��ȭ
            PlayGameManager.Instance.EnableParticle(other.transform.position);

            //��Ȱ��ȭ�� ������Ʈ ����Ʈ�� �־���
            StageController.Instance.AddDisabled(other.gameObject.GetComponent<Jelly>().GetParent());
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

    //���� �ʱ�ȭ
    void Init_Buff()
    {
        _obstacleDamageBuff = 1;
        _timeDamageBuff = 1;
        _healBuff = 1;
    }

    void TriggerPortal()
    {
        //Heal(1.8f);
        
        StopMyCoroutine();
        _portalCoroutine = StartCoroutine(PortalTime(0.2f));
    }

    void TriggerGround()
    {
        //����ȭ �� ������Ʈ �����·� ����
        _camera.GetComponent<ShowPlayer>().SetOpaque();

        //��Ż �Ҹ�
        AudioManager.Instance.PlayPortalSFX();

        CanvasController.Instance.ChangeSpeedColor(0);

        //ƽ �������� ������ ��¡�� ���� ����
        PlayGameManager.Instance.ResetBonusTime();

        //�������� �غ�
        StageController.Instance.PrepareToStage();

        GroundDamage(20);
    }

    public void ChargeEnergy()
    {
        if (!_isCast)
        {
            if (_energy + _energyChargeSpeed > 100)
            {
                CanvasController.Instance.PlayerUpEnergy(100 - _energy);
                _energy = 100;
                
                if(!_isInvincible && !_isPoison) _colliderRange.PrepareToSkill();
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
            CanvasController.Instance.PlayerRestoreHP(amount);
            _hp += amount;
        }

        //Debug.Log("HP : " + _hp);
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
        if (!_isInvincible && !_isCast)
        {
            Handheld.Vibrate(); //�޴��� ����
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
        if (!_isPassPortal)
        {
            Handheld.Vibrate(); //�޴��� ����
            _camera.GetComponent<StressReceiver>().InduceStress(0.2f); //ī�޶� ����
            StageController.Instance.MinusPassThroughCount(); //���ǵ� �� ���� �Ѵܰ� �϶�

            SetGroundDamage(1);

            GetDamage(damage);
        }
    }

    public void UseSkill()
    {
        if (_energy == 100)
        {
            if (_skillCoroutine != null) StopCoroutine(_skillCoroutine);

            _skillCoroutine = StartCoroutine(CastTime(2));
            _energy = 0;
            CanvasController.Instance.PlayerDownEnergy(100);
        }
    }

    void HitObstacle(float time)
    {
        StageController.Instance.MinusPassThroughCount(); //���ǵ� �� ���� �Ѵܰ� �϶�

        StageController.Instance.SetVelocity(StageController.Instance.BasicSpeed);
        _invincibleCoroutine = StartCoroutine(InvincibleTime(time));
    }

    void SetGroundDamage(float time)
    {
        if (_invincibleCoroutine != null) StopCoroutine(_invincibleCoroutine);

        _invincibleCoroutine = StartCoroutine(InvincibleTime(time));
    }

    void EndGame()
    {
        //�ٶ� UI Disable
        CanvasController.Instance.OnSpeedPanel(false);

        //ChangeWindAlpha(0);
        _colliderRange.DisableRawImage();
        PlayGameManager.Instance.EndGame();

        StopAllCoroutines();
    }

    public void DestroyPlayer() { transform.gameObject.SetActive(false); }

    void ResetColor()
    {
        if (_isPoison) _colliderRange.SetPoison();
        else if (_energy < 100) _colliderRange.ResetColor();
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
        _isCast = true;
        _isInvincible = true;

        //��ų �ִϸ��̼� ����
        _animator.SetBool("UseSkill", true);

        //��ų ��ƼŬ Ȱ��ȭ
        _colliderRange.GetComponent<ColliderRange>().EnableSkillEffect();

        //��ų ���� UI �� �Ҹ� Ȱ��ȭ
        CanvasController.Instance.OnSpeedPanel(true);
        CanvasController.Instance.ChangeSpeedColor(1);
        AudioManager.Instance.PlaySkillSFX();

        //��ų ������ ����
        _colliderRange.PrepareToSkill();
        _colliderRange.SetSkill();

        //�ִ� �ӵ��� ����
        StageController.Instance.SetVelocity(StageController.Instance.MaxSpeed);

        yield return new WaitForSeconds(t);

        //���� �ִϸ��̼� ����
        _animator.SetBool("UseSkill", false);

        _colliderRange.GetComponent<ColliderRange>().DisableSkillEffect();

        CanvasController.Instance.ChangeSpeedColor(0);
        StageController.Instance.ResetVelocity();

        _isInvincible = false;
        _isCast = false;

        ResetColor();
    }

    //���� �� ��ų �ߴ�
    public void StopMyCoroutine()
    {
        ResetColor();

        if(_portalCoroutine != null) StopCoroutine(_portalCoroutine);

        if (_invincibleCoroutine != null)
        {
            StopCoroutine(_invincibleCoroutine);
            CanvasController.Instance.OnSpeedPanel(true);
        }

        if(_skillCoroutine != null)
        {
            _animator.speed = 1;
            _colliderRange.GetComponent<ColliderRange>().DisableSkillEffect();

            _animator.SetBool("UseSkill", false);
            CanvasController.Instance.ChangeSpeedColor(0);
            StageController.Instance.ResetVelocity();

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

    public void PlusMaxHp(float amount) 
    { 
        _maxHp += amount;
        _hp = _maxHp;
        _bloodHp = _maxHp * 0.15f;
        CanvasController.Instance.InitSetting(_maxHp);
    }

    public void SetHealBuff(float amount) { _timeDamageBuff += amount; }

    public void SetEnergyDeBuff()
    {
        _energy *= 0.8f;
        CanvasController.Instance.PlayerUpEnergy(-_energy * 0.2f);
    }

    public float GetHp() { return _hp; }
}