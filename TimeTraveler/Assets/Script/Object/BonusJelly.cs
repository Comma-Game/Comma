using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusJelly : MonoBehaviour
{
    static BonusJelly _instance;
    public static BonusJelly Instance
    {
        get
        {
            Init_Instance();

            return _instance;
        }
    }

    static void Init_Instance()
    {
        if(_instance == null)
        {
            GameObject obj = GameObject.Find("Bonus");
            _instance = obj.GetComponent<BonusJelly>();
        }
    }

    [SerializeField]
    GameObject[] _bonus;

    [SerializeField]
    GameObject _bonusMap;

    GameObject _parent;

    Queue<GameObject>[] _waitingBonusQueue, _usingBonusQueue;

    int _bonusCount;
    bool[] _getJelly;

    private void Awake()
    {
        _bonusCount = 0;
        _getJelly = new bool[5];

        _parent = transform.gameObject;

        _waitingBonusQueue = new Queue<GameObject>[5];
        _usingBonusQueue = new Queue<GameObject>[5];
        for (int i = 0; i < 5; i++)
        {
            _waitingBonusQueue[i] = new Queue<GameObject>();
            _waitingBonusQueue[i].Enqueue(_bonus[i]);

            _usingBonusQueue[i] = new Queue<GameObject>();
        }
    }

    private void Start()
    {
        Init_Instance();
    }

    void CheckBonus()
    {
        if(_bonusCount == 5)
        {
            _bonusCount = 0;
            for (int i = 0; i < _getJelly.Length; i++) _getJelly[i] = false;
            StageController.Instance.EnableBonusStage();
        }
    }

    public void TriggerBonusJelly(int index)
    {
        if(!_getJelly[index])
        {
            _getJelly[index] = true;
            _bonusCount++;

            CheckBonus();
        }
    }

    public GameObject GetBonusJelly()
    {
        int index = Random.Range(0, 5);
        StageController.Instance.SetBonusJellyIndex(index);

        GameObject obj;
        if (_waitingBonusQueue[index].Count == 0)
        {
            obj = Instantiate(_bonus[index]);
            obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else obj = _waitingBonusQueue[index].Dequeue();

        _usingBonusQueue[index].Enqueue(obj);

        return obj; 
    }

    public void ReturnBonusJelly(int index)
    {
        GameObject obj = _usingBonusQueue[index].Dequeue();
        obj.transform.SetParent(_parent.transform);
        obj.SetActive(false);

        _waitingBonusQueue[index].Enqueue(obj);
    }

    public GameObject GetBonusStage() { return _bonusMap; }
}
