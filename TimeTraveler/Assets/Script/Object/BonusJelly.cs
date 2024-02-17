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

    int _bonusCount, _jellyLength;
    bool[] _getJelly;

    private void Awake()
    {
        _bonusCount = 0;
        _jellyLength = _bonus.Length;
        _getJelly = new bool[_jellyLength];

        _parent = transform.gameObject;

        _waitingBonusQueue = new Queue<GameObject>[_jellyLength];
        _usingBonusQueue = new Queue<GameObject>[_jellyLength];
        for (int i = 0; i < _jellyLength; i++)
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
        if(_bonusCount == _jellyLength)
        {
            _bonusCount = 0;
            for (int i = 0; i < _getJelly.Length; i++)
            {
                _getJelly[i] = false;
                CanvasController.Instance.isChangeCollectImg(i, false);
            }

            StageController.Instance.PrepareForBonusStage();
        }
    }

    public void TriggerBonusJelly(int index)
    {
        if(!_getJelly[index])
        {
            _getJelly[index] = true;
            CanvasController.Instance.isChangeCollectImg(index, true);
            _bonusCount++;

            CheckBonus();
        }
    }

    public GameObject GetBonusJelly()
    {
        int index = Random.Range(0, _jellyLength);
        StageController.Instance.SetBonusJellyIndex(index);

        GameObject obj;
        if (_waitingBonusQueue[index].Count == 0) obj = Instantiate(_bonus[index]);
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
