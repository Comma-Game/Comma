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
    private float _basicSpeed = 30f;
    
    private float _speed;
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
    private float _accSpeed = 0.3f;
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
    private float _maxSpeed = 80f;
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

    [SerializeField]
    private float _maxFirstSpeed = 40f;
    public float MaxFirstSpeed
    {
        get
        {
            return _maxFirstSpeed;
        }
        set
        {
            _maxFirstSpeed = value;
        }
    }

    struct StageInfo
    {
        public int concept_index, stage_index;

        public StageInfo(int concept_index, int stage_index)
        {
            this.concept_index = concept_index;
            this.stage_index = stage_index;
        }
    }

    //Resources�� �ִ� ���� �̸�
    string[] _concept =
    {
        //"TestStage",
        "PipeMap",
        "ToyMap",
        "ForestMap",
        "DinoMap",
        "HouseMap",
        "LabMap",
        "PlanetMap",
        "PlaygroundMap",
        "SchoolMap",
        "SeaMap"
    };

    static GameObject _stageController;
    GameObject[][] _stagePrefab;
    GameObject _stage, _nextStage, _parent;
    Queue<StageInfo> _queue; //������ ������������ ����(���� 2��, �� ���������� 6��)
    List<int> _conceptIndex;
    int _prevConcept, _stageCount;
    List<GameObject> _disabled, _explode;
    bool[,] _iniStage;

    private void Awake()
    {
        _basicSpeed = 30f;
        _speed = _basicSpeed;
        _accSpeed = 0.3f;
        _maxSpeed = 80f;
        _maxFirstSpeed = 40f;

        _stageCount = 1;
        _prevConcept = -1;

        _stage = null;
        _nextStage = null;
        _iniStage = new bool[_concept.Length, 3];

        _disabled = new List<GameObject>();
        _explode = new List<GameObject>();

        SetConceptIndex();
        ResetConceptObject();
        InstantiateStage();

        _queue = new Queue<StageInfo>();
    }

    void Start()
    {
        Init_Instance();
        Init();
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

        _instance = _stageController.GetComponent<StageController>();
    }

    void Init()
    {
        InsertStageToQueue();
        InsertStageToQueue();

        _nextStage = SetNextStage();
        SetCurrentStage();
        SetStagesVelocity();
    }

    //�ʱ� Concept Index ����
    void SetConceptIndex()
    {
        _conceptIndex = new List<int>();
        for (int i = 0; i < _concept.Length; i++) _conceptIndex.Add(i);
    }

    //�ʿ� ��Ÿ�� ���������� ť�� �־��ִ� ����
    void InsertStageToQueue()
    {
        int conceptIndex = Random.Range(0, _conceptIndex.Count);
        
        //������ �̰�, �̹� ����� ������ ť�� �־���
        if(_prevConcept != -1) _conceptIndex.Add(_prevConcept);
        _prevConcept = _conceptIndex[conceptIndex];

        //�� ������ Stage�� ������ 3���� ������ �ֱ� ������ {0, 1, 2}�� List�� ����
        List<int> stageIndex = new List<int>(new int[]{ 0, 1, 2 });
        
        //�������� stageIndex�� index�� �޾Ƽ� ť�� concept index�� stageIndex[index]�� �־��ְ�,
        //stageIndex[index]�� List���� ���༭ �ߺ��� ���� �ʰ� �Ѵ�.
        for (int i = 3; i > 0; i--)
        {
            int next_stage_num = Random.Range(0, i);
            _queue.Enqueue(new StageInfo(_conceptIndex[conceptIndex], stageIndex[next_stage_num]));

            int concept = _conceptIndex[conceptIndex], stage = stageIndex[next_stage_num];

            if (!_iniStage[concept, stage])
            {
                _iniStage[concept, stage] = true;

                _stagePrefab[concept][stage] = Instantiate(_stagePrefab[concept][stage]);
                _stagePrefab[concept][stage].transform.SetParent(_parent.transform);
                _stagePrefab[concept][stage].SetActive(false);
            }

            stageIndex.RemoveAt(next_stage_num);
        }

        //ť�� ������ ���������� List���� ����
        _conceptIndex.RemoveAt(conceptIndex);
    }


    //���� �������� ����
    void SetCurrentStage()
    {
        _stage = _nextStage;

        _stage.transform.GetComponent<GateMovement>().StopMove();
        _stage.transform.position = new Vector3(0, 400, 0); //ù��° ���������� �ι�° �������� ��ġ ���� ����
        //_stage.SetActive(true);

        _nextStage = SetNextStage();
    }

    //���� �������� ����
    GameObject SetNextStage()
    {
        GameObject ret;

        StageInfo next_stage_info = _queue.Dequeue(); //ť���� ���� ���Ұ� ���� ������ ��������
        if (_queue.Count <= 3) InsertStageToQueue(); //ť�� ũ�Ⱑ 3 ���ϸ� ���ο� ���� ť�� ����

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;
        ret = _stagePrefab[concept_index][stage_index];

        int nextAngle = Random.Range(0, 360);
        ret.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
        ret.SetActive(true);

        return ret;
    }

    //��Ȱ��ȭ�� ��� ������Ʈ Ȱ��ȭ
    void ReturnStage()
    {
        _stage.SetActive(false);
        EnableObject();
        DeleteExploder();

        _stage.transform.position = new Vector3(0, 0, 0);
    }

    //Scene�� ��� ���������� ���� �� ��Ȱ��ȭ
    void InstantiateStage()
    {
        _stagePrefab = new GameObject[_concept.Length][];
        for (int i = 0; i < _concept.Length; i++) _stagePrefab[i] = Resources.LoadAll<GameObject>(_concept[i]);
    }

    //�������� ������ ȣ��
    public void DisableStage()
    {
        ReturnStage();

        int conceptCount = _stageCount++ / 3;

        _speed = _basicSpeed + conceptCount * 2;
        _speed = _speed > _maxFirstSpeed ? _maxFirstSpeed : _speed;

        SetCurrentStage();
        SetStagesVelocity();
    }

    //Scene�� �ִ� Stage �ӵ� ����
    public void SetStagesVelocity()
    {
        _stage.GetComponent<GateMovement>().Move();
        _nextStage.GetComponent<GateMovement>().Move();
    }

    //���� ���ǵ� ��ȯ
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    public void SetVelocity(float speed)
    {
        _stage.GetComponent<GateMovement>().SetVelocity(speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(speed);
    }

    public void ResetVelocity()
    {
        _stage.GetComponent<GateMovement>().SetVelocity(_speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(_speed);
    }

    //HP�� 0 ���� �϶� ȣ��
    public void EndGame()
    {
        Destroy(_parent);
    }

    //Stage�� �� �θ� ����
    void ResetConceptObject()
    {
        _parent = new GameObject();
        _parent.name = "Concept";
    }

    //���ӵ� ����
    public void SetAcceleration()
    {
        Debug.Log("���� Stage : " + _stage.name);
        Debug.Log("���� Stage : " + _nextStage.name);
        _stage.GetComponent<GateMovement>().SetAcceleration();
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    public void AddDisabled(GameObject gameObject)
    {
        _disabled.Add(gameObject);
    }

    public void SetAccDeBuff()
    {
        _accSpeed *= 1.2f;
    }

    public void SetBasicSpeedDeBuff()
    {
        _basicSpeed *= 1.2f;
        _speed = _basicSpeed;
    }

    public void MakeExploder(Transform parent, GameObject explode)
    {
        explode.transform.SetParent(parent);
        _explode.Add(explode);
    }

    void DeleteExploder()
    {
        foreach (GameObject explode in _explode)
        {
            if (explode != null) Debug.Log("Explode ���� : " + explode.gameObject.name);
                Destroy(explode);
        }
        _explode.Clear();
    }

    void EnableObject()
    {
        foreach (GameObject obj in _disabled) obj.SetActive(true);
        _disabled.Clear();
    }

    public int GetStageCount() { return _stageCount; }
}
