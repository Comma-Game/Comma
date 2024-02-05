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
    private float _basicSpeed; //���� �����ϰ� �ִ� ���������� �⺻ �ӵ�
    public float BasicSpeed
    {
        get { return _basicSpeed; }
    }

    private float _speed; //�ǽð� �ӵ�
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
    private float _accSpeed; //���ӵ�
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
    private float _maxSpeed; //�ִ� �ӵ�
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
    private float _maxBasicSpeed; //�⺻ �ӵ��� �ִ�ġ
    public float MaxBasicSpeed
    {
        get
        {
            return _maxBasicSpeed;
        }
        set
        {
            _maxBasicSpeed = value;
        }
    }

    //ť�� �� �ڷᱸ��
    struct StageInfo
    {
        public int concept_index, stage_index;

        public StageInfo(int concept_index, int stage_index)
        {
            this.concept_index = concept_index;
            this.stage_index = stage_index;
        }
    }

    static GameObject _stageController;

    static void Init_Instance()
    {
        _stageController = GameObject.Find("StageController");
        if (!_stageController)
        {
            _stageController = new GameObject();
            _stageController.name = "StageController";
            _stageController.AddComponent<StageController>();
        }

        _instance = _stageController.GetComponent<StageController>();
    }

    GameObject[][] _stagePrefab; //���ҽ� ���Ͽ��� ������ Stage ������
    Queue<StageInfo> _queue; //������ ������������ ����(���� 2��, �� ���������� 6��)
    GameObject _stage, _nextStage, _prevStage, _stageParent; //_stageParent : ������������ �θ� ������Ʈ
    List<int> _conceptIndex; //�������� ������ ������ ������ ����Ʈ
    int _prevConcept, _stageCount; //_prevConcept : �ٷ� ���� ����� ����, _conceptIndex�� ���� concept�� �־��ֱ� ���� ����
    List<GameObject> _disabled, _explode; //��Ȱ��ȭ�� ������Ʈ��� �ı��� ������� ������ ����Ʈ
    GameObject _stageEnd;
    GetStage _conceptInfo;
    int _passThroughCount, _conceptCount; //_passThroughCount : ��ֹ��� �ε����� �ʰ� ��� �����ϴ� Ƚ��, _conceptCount : �÷��� ������ concept ����
    bool[,] _isInstantiateStage;

    private void Awake()
    {
        //���ǵ� �ʱ�ȭ
        _basicSpeed = 7.5f;
        _speed = _basicSpeed;

        //�׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _accSpeed = 0.5f;

        _maxSpeed = 25f;
        _maxBasicSpeed = 12.5f;

        //stage ���� �ʱ�ȭ
        _stageCount = 1;
        _prevConcept = -1;
        _stage = null;
        _nextStage = null;
        _prevStage = null;
        _stageParent = null;

        //������ Obstacle ����Ʈ �ʱ�ȭ
        _disabled = new List<GameObject>();
        _explode = new List<GameObject>();

        _queue = new Queue<StageInfo>();

        _stageEnd = GameObject.Find("StageEnd");
        _conceptInfo = GameObject.Find("Concept").GetComponent<GetStage>();
    }

    void Start()
    {
        Init_Instance();
        Init();
    }

    void Init()
    {
        _conceptCount = SaveLoadManager.Instance.GetUnlockedConcept() + 1;
        _isInstantiateStage = new bool[_conceptCount, 3];
        
        SetConceptIndex();
        SetStageParent();
        InstantiateStage();

        InsertStageToQueue();
        InsertStageToQueue();

        _nextStage = SetNextStage();
        SetCurrentStage();
        MoveStage();
    }

    //�ʱ� Concept Index ����
    void SetConceptIndex()
    {
        Debug.Log((_conceptCount - 1) + " �������� Ȱ��ȭ");

        _conceptIndex = new List<int>();
        for (int i = 0; i < _conceptCount; i++) _conceptIndex.Add(i);
    }

    //�ʿ� ��Ÿ�� ���������� ť�� �־��ִ� ����
    void InsertStageToQueue()
    {
        int conceptIndex = Random.Range(0, _conceptIndex.Count);
        
        //�׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //while (_conceptIndex[conceptIndex] == 5 || _conceptIndex[conceptIndex] == 7 || _conceptIndex[conceptIndex] == 8) conceptIndex = Random.Range(0, _conceptIndex.Count);
        
        
        //������ �̰�, �̹� ����� ������ ť�� �־���
        if (_prevConcept != -1) _conceptIndex.Add(_prevConcept);
        _prevConcept = _conceptIndex[conceptIndex];

        //�� ������ Stage�� ������ 3���� ������ �ֱ� ������ {0, 1, 2}�� List�� ����
        List<int> stageIndex = new List<int>(new int[]{ 0, 1, 2 });
        
        //�������� stageIndex�� index�� �޾Ƽ� ť�� concept index�� stageIndex[index]�� �־��ְ�,
        //stageIndex[index]�� List���� ���༭ �ߺ��� ���� �ʰ� �Ѵ�.
        for (int i = 3; i > 0; i--)
        {
            int next_stage_num = Random.Range(0, i);
            _queue.Enqueue(new StageInfo(_conceptIndex[conceptIndex], stageIndex[next_stage_num]));
            stageIndex.RemoveAt(next_stage_num);
        }

        //ť�� ������ ���������� List���� ����
        _conceptIndex.RemoveAt(conceptIndex);
    }

    void TestStage(int index)
    {
        //�� ������ Stage�� ������ 3���� ������ �ֱ� ������ {0, 1, 2}�� List�� ����
        List<int> stageIndex = new List<int>(new int[] { 0, 1, 2 });

        //�������� stageIndex�� index�� �޾Ƽ� ť�� concept index�� stageIndex[index]�� �־��ְ�,
        //stageIndex[index]�� List���� ���༭ �ߺ��� ���� �ʰ� �Ѵ�.
        for (int i = 3; i > 0; i--)
        {
            int next_stage_num = Random.Range(0, i);
            _queue.Enqueue(new StageInfo(_conceptIndex[index], stageIndex[next_stage_num]));
            stageIndex.RemoveAt(next_stage_num);
        }

        //ť�� ������ ���������� List���� ����
        _conceptIndex.RemoveAt(index);
    }

    //���� �������� ����
    void SetCurrentStage()
    {
        _stage = _nextStage;
        PlayGameManager.Instance.SetStage(_stage);

        _stage.transform.GetComponent<GateMovement>().StopMove();
        _stage.transform.position = new Vector3(0, 95.1f, 0); //ù��° ���������� �ι�° �������� ��ġ ���� ����

        _stageEnd.transform.SetParent(null);
        _stageEnd.transform.position = new Vector3(-0.42f, 52.48f, -0.13f);
        _stageEnd.transform.SetParent(_stage.transform);

        _nextStage = SetNextStage();
    }

    //���� �������� ����
    GameObject SetNextStage()
    {
        GameObject ret;

        StageInfo next_stage_info = _queue.Dequeue(); //ť���� ���� ���Ұ� ���� ������ ��������
        if (_queue.Count <= 3) InsertStageToQueue(); //ť�� ũ�Ⱑ 3 ���ϸ� ���ο� ���� ť�� ����

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;
        
        if(!_isInstantiateStage[concept_index, stage_index])
        {
            _isInstantiateStage[concept_index, stage_index] = true;
            _stagePrefab[concept_index][stage_index] = Instantiate(_stagePrefab[concept_index][stage_index]);
            _stagePrefab[concept_index][stage_index].transform.SetParent(_stageParent.transform);
        }
        
        ret = _stagePrefab[concept_index][stage_index];
        ret.transform.position = new Vector3(0, 0, 0);

        int nextAngle = Random.Range(0, 360);
        ret.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
        ret.SetActive(true);

        return ret;
    }

    //��Ȱ��ȭ�� ��� ������Ʈ Ȱ��ȭ
    void ReturnStage()
    {
        _prevStage = _stage;
        _prevStage.GetComponent<GateMovement>().StopMove();
        _prevStage.transform.position = new Vector3(0, 191.1f, 0);

        EnableObject();
        DeleteExploder();
    }

    public void DisablePrevStage() 
    {
        if (_prevStage == null) return;

        _prevStage.SetActive(false);
        //_prevStage.transform.position = new Vector3(0, 0, 0);
    }

    //Resources ���Ͽ� �ִ� Prefab�� ������
    void InstantiateStage()
    {
        _stagePrefab = new GameObject[_conceptCount][];
       
        for (int i = 0; i < _conceptCount; i++)
        {
            _stagePrefab[i] = _conceptInfo.GetStagePrefab(i);
            for (int j = 0; j < 3; j++) _stagePrefab[i][j].SetActive(false);
        }
    }

    void SetStageParent()
    {
        _stageParent = new GameObject();
        _stageParent.name = "StageParent";
    }

    //�������� ������ ȣ��
    public void PrepareToStage()
    {
        if (_stageCount++ % 3 == 0) _passThroughCount = _passThroughCount >= 5 ? 5 : _passThroughCount + 1;

        _speed = _basicSpeed + _passThroughCount;
        _speed = _speed > _maxBasicSpeed ? _maxBasicSpeed : _speed;

        //���� ���� UI ����
        CanvasController.Instance.ChangeState(_stageCount);
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);

        ReturnStage();
        SetCurrentStage();
        MoveStage();
    }

    public void MinusPassThroughCount() 
    { 
        _passThroughCount = _passThroughCount > 0 ? --_passThroughCount : 0;
        
        //��� UI ����
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);
    }
    public int GetPassThroughCount() { return _passThroughCount; }


    //Scene�� �ִ� Stage �ӵ� ����
    public void MoveStage()
    {
        if(_prevStage != null) _prevStage.GetComponent<GateMovement>().Move();
        _stage.GetComponent<GateMovement>().Move();
        _nextStage.GetComponent<GateMovement>().Move();
    }

    //���� ���ǵ� ��ȯ
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    //�������� �ӵ� ����
    public void SetVelocity(float speed)
    {
        _stage.GetComponent<GateMovement>().SetVelocity(speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(speed);
    }

    //�������� �ӵ� ����
    public void ResetVelocity()
    {
        _speed = _basicSpeed;
        _stage.GetComponent<GateMovement>().SetVelocity(_speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(_speed);
    }

    public void EndGame() 
    { 
        _stageEnd.SetActive(false);
        _stageParent.SetActive(false);    
    }

    //���ӵ� ����
    public void SetAcceleration()
    {
        Debug.Log("���� Stage : " + _stage.name);
        Debug.Log("���� Stage : " + _nextStage.name);
        _stage.GetComponent<GateMovement>().SetAcceleration();
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    //�ε��� ��ֹ� ����Ʈ�� �־��ֱ�
    public void AddDisabled(GameObject gameObject)
    {
        _disabled.Add(gameObject);
    }

    //���ӵ� ��������
    public void SetAccDeBuff()
    {
        _accSpeed *= 1.2f;
    }

    //�⺻ �ӵ� �������� ����
    public void SetBasicSpeedDeBuff()
    {
        _basicSpeed *= 1.2f;
        _speed = _basicSpeed;
    }
    
    //���� ����� ����Ʈ�� �־��ֱ�
    public void MakeExploder(Transform parent, GameObject explode)
    {
        explode.transform.SetParent(parent);
        _explode.Add(explode);
    }

    //����Ʈ�� �ִ� ���� ����� ����
    void DeleteExploder()
    {
        foreach (GameObject explode in _explode) Destroy(explode);
        _explode.Clear();
    }

    //����Ʈ�� ����ִ� ��ֹ� Ȱ��ȭ
    void EnableObject()
    {
        foreach (GameObject obj in _disabled) obj.SetActive(true);
        _disabled.Clear();
    }

    //���� �������� ��ȯ
    public int GetStageCount() { return _stageCount; }
}
