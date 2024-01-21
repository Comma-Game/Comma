using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    static StageController _instance;
    public static StageController Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    [SerializeField]
    private float _speed = 10f;
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    [SerializeField]
    private float _accSpeed = 2f;
    public float AccSpeed
    {
        get
        {
            return _accSpeed;
        }
        set
        {
            _accSpeed = value;
        }
    }

    [SerializeField]
    private float _maxSpeed = 100f;
    public float MaxSpeed
    {
        get
        {
            return _maxSpeed;
        }
        set
        {
            _maxSpeed = value;
        }
    }

    string[] _concept =
    {
        "PipeMap"
    };

    static GameObject _stageController;
    GameObject[] _stagePrefab;
    GameObject _stage, _nextStage, _parent;

    void Start()
    {
        Init_Instance();
        Init();
    }

    void Update()
    {
        
    }

    static void Init_Instance()
    {
        _stageController = GameObject.Find("StageController");
        if(!_stageController)
        {
            _stageController = new GameObject();
            _stageController.name = "StageController";
            _stageController.AddComponent<StageController>();
        }

        DontDestroyOnLoad(_stageController);
        _instance = _stageController.GetComponent<StageController>();
    }

    void Init()
    {
        _stagePrefab = Resources.LoadAll<GameObject>("PipeMap");
        _parent = GameObject.Find("Stage");
        InstantiateStage(110);
        InstantiateStage(0);
    }

    void InstantiateStage(int y)
    {
        int next_stage_num = Random.Range(0, _stagePrefab.Length);

        if(_nextStage) _stage = _nextStage;
        Vector3 pos = new Vector3(0, y, 0);
        _nextStage = Instantiate(_stagePrefab[next_stage_num], pos, Quaternion.identity);
        _nextStage.transform.SetParent(_parent.transform);

        int nextAngle = Random.Range(0, 360);
        _nextStage.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
    }

    public void DestroyStage()
    {
        UnsetAcceleration();
        Destroy(_stage);
        InstantiateStage(-20);
    }

    public void SetAcceleration()
    {
        _stage.GetComponent<GateMovement>().SetAcceleration();
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    public void UnsetAcceleration()
    {
        _nextStage.GetComponent<GateMovement>().UnsetAcceleration();
    }
}
