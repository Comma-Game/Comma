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
            init();
            return _instance;
        }
    }

    static GameObject _stageController;

    GameObject[] _stagePrefab;
    GameObject _stage, _nextStage, _parent;

    void Start()
    {
        init();
        _stagePrefab = Resources.LoadAll<GameObject>("Stage");
        _parent = GameObject.Find("Stage");
        instantiateStage(90);
        instantiateStage(0);
    }

    void Update()
    {
        
    }

    static void init()
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

    void instantiateStage(int y)
    {
        int next_stage_num = Random.Range(0, _stagePrefab.Length);

        if(_nextStage) _stage = _nextStage;
        Vector3 pos = new Vector3(0, y, 0);
        _nextStage = Instantiate(_stagePrefab[next_stage_num], pos, Quaternion.identity);
        _nextStage.transform.SetParent(_parent.transform);
    }

    public void Destroy_Stage()
    {
        Destroy(_stage);
        instantiateStage(-10);
    }

    public void Game_Over()
    {
        //어떻게 구성할 지 생각 필요
        //Destroy(_parent);
        //_instance.GetComponent<StageController>().enabled = false;
    }
}
