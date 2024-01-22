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
    private float _speed = 30f;
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
    private float _accSpeed = 0.15f;
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
    int _prevConcept;

    private void OnEnable()
    {

    }

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
        _score = 0;
        _prevConcept = -1;
        SetConceptIndex();

        _stagePrefab = new GameObject[_concept.Length][];
        for(int i = 0; i < _concept.Length; i++) _stagePrefab[i] = Resources.LoadAll<GameObject>(_concept[i]);
        Reset_ConceptObject();
        _queue = new Queue<StageInfo>();

        InsertStageToQueue();
        InsertStageToQueue();
        InstantiateStage(400);
        InstantiateStage(0); 
        
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

        
        if(_stage)
        {
            Debug.Log("cur pos : " + _stage.transform.position.y);
            Debug.Log("next pos : " + y);
        }
        
    }

    //�������� ������ ȣ��
    public void DestroyStage()
    {
        UnsetAcceleration();
        Destroy(_stage);
        InstantiateStage(_stage.transform.position.y - 800);
        _speed++;
    }

    //HP�� 0 ���� �϶� ȣ��
    public void EndGame()
    {
        Destroy(_parent);
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
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    //���ӵ� ����
    public void UnsetAcceleration()
    {
        _nextStage.GetComponent<GateMovement>().UnsetAcceleration();
    }

    public void ScoreUp(int value)
    {
        _score += value;
    }
    IEnumerator ScoreTime()
    {
        while(true)
        {
            _score += 1;
            //CanvasController.Instance.ChangeScoreText(_score);
            Debug.Log("Score : " + _score);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
