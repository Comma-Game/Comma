using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Player _instance;
    public static Player Instance
    {
        get
        {
            return _instance;
        }
    }
    
    int _hp;
    bool _isInvincible;
    StageController _stageController;
    Coroutine coroutine;
    RaycastHit _hit;
    string _obstacle;

    void Start()
    {
        init();
        _stageController = StageController.Instance;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit))
        {
            if (_hit.transform.gameObject.CompareTag("Obstacle"))
            {
                _obstacle = _hit.transform.gameObject.name;
                Debug.Log("Ÿ�� : " + _obstacle);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�浹 : " + collision.gameObject.name);
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

    void init()
    {
        _instance = transform.GetComponent<Player>();
        _hp = SaveLoadManager.Instance.PlayerData.hp;
        _isInvincible = false;
    }

    void TriggerPortal()
    {
        coroutine = StartCoroutine(Invincible());
    }

    void TriggerGround()
    {
        _stageController.DestroyStage();
        Damage(80);
    }
    
    public void Damage(int damage)
    {
        if (!_isInvincible) _hp -= damage;
        Debug.Log("HP : " + _hp);
    }

    IEnumerator Invincible()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(0.5f);
        _isInvincible = false;
    }

    private void OnApplicationQuit()
    {
        SaveLoadManager.Instance.SavePlayer(_hp, 0);
    }
}
