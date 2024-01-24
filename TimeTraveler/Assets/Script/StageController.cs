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
    private float _firstSpeed;
    
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
    private float _accSpeed;
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
    private float _maxSpeed;
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

    static int _score;
    public static int Score
    {
        get
        {
            return _score;
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
        //"LabMap"
        "ToyMap"
    };

    static GameObject _stageController;
    GameObject[][] _stagePrefab;
    GameObject _stage, _nextStage, _parent;
    Queue<StageInfo> _queue; //������ ������������ ����(���� 2��, �� ���������� 6��)
    Coroutine _coroutine;
    List<int> _conceptIndex;
    Player _player;
    int _prevConcept, _stageCount;

    private void OnEnable()
    {

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
        _player = GameObject.Find("Player").GetComponent<Player>();

        _firstSpeed = 30f;
        _speed = _firstSpeed;
        _accSpeed = 0.15f;
        _maxSpeed = 100f;

        _stageCount = 1;
        _score = 0;
        _prevConcept = -1;

        _stage = null;
        _nextStage = null;
        SetConceptIndex();

        _stagePrefab = new GameObject[_concept.Length][];
        for(int i = 0; i < _concept.Length; i++) _stagePrefab[i] = Resources.LoadAll<GameObject>(_concept[i]);
        Reset_ConceptObject();

        _queue = new Queue<StageInfo>();
        InsertStageToQueue();
        InsertStageToQueue();

        InstantiateStage(400);
        InstantiateStage(0);

        _stage.GetComponent<GateMovement>().enabled = true;
        SetStagesVelocity();

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ScoreTime());
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
            stageIndex.RemoveAt(next_stage_num);
        }

        //ť�� ������ ���������� List���� ����
        _conceptIndex.RemoveAt(conceptIndex);
    }

    //Scene�� ť�� �ִ� ���������� ����
    void InstantiateStage(float y)
    {
        StageInfo next_stage_info = _queue.Dequeue(); //ť���� ���� ���Ұ� ���� ������ ��������
        if (_queue.Count <= 3) InsertStageToQueue(); //ť�� ũ�Ⱑ 3 ���ϸ� ���ο� ���� ť�� ����

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;

        //�ι�° ���������� �ִٸ� �ι�° ���������� ���� ���������� ����
        if (_nextStage)
        {
            _stage = _nextStage;
            _stage.transform.position = new Vector3(0, 400, 0); //ù��° ���������� �ι�° �������� ��ġ ���� ���� 
        }

        Vector3 pos = new Vector3(0, y, 0);
        _nextStage = Instantiate(_stagePrefab[concept_index][stage_index], pos, Quaternion.identity);
        _nextStage.transform.SetParent(_parent.transform);

        int nextAngle = Random.Range(0, 360);
        _nextStage.transform.rotation = Quaternion.Euler(0, nextAngle, 0);

        /*
        if(_stage)
        {
            Debug.Log("cur pos : " + _stage.transform.position.y);
            Debug.Log("next pos : " + y);
        }
        */
    }

    //�������� ������ ȣ��
    public void DestroyStage()
    {
        UnsetAcceleration();
        Destroy(_stage);

        _speed = _firstSpeed + _stageCount;
        _stageCount++;

        InstantiateStage(_stage.transform.position.y - 800);

        _stage.GetComponent<GateMovement>().enabled = true;
        SetStagesVelocity();
    }

    //Scene�� �ִ� Stage �ӵ� ����
    public void SetStagesVelocity()
    {
        _stage.GetComponent<GateMovement>().Move();
    }

    //���� ���ǵ� ��ȯ
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    //HP�� 0 ���� �϶� ȣ��
    public void EndGame()
    {
        Destroy(_parent);
        EnableGameOverUI();
        StopCoroutine(_coroutine);
    }

    //Stage�� �� �θ� ����
    void Reset_ConceptObject()
    {
        _parent = new GameObject();
        _parent.name = "Concept";
    }

    //���ӵ� ����
    public void SetAcceleration()
    {
        _stage.GetComponent<GateMovement>().SetAcceleration();
    }

    //���ӵ� ����
    public void UnsetAcceleration()
    {
        _stage.GetComponent<GateMovement>().UnsetAcceleration();
    }

    public void ScoreUp(int value)
    {
        _score += value;
    }

    IEnumerator ScoreTime()
    {
        while(true)
        {
            _score += 1 + _stageCount / 3;
            CanvasController.Instance.ChangeScoreText(_score);
            _player.TimeDamage();
            _player.ChargeEnergy();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void EnableGameOverUI()
    {
        CanvasController.Instance.OpenGameOverPanel(true);
        CanvasController.Instance.ChangeResultScoreText(_score);
        CanvasController.Instance.ChangeResultCoinText(_score / 10);
    }
}
