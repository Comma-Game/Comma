using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    struct StageInfo
    {
        public int concept, stage;

        public StageInfo(int concept, int stage) 
        {
            this.concept = concept;
            this.stage = stage;
        } 
    }
    [SerializeField]
    TextMeshProUGUI _curText;

    [SerializeField]
    GameObject[] _image;

    [SerializeField]
    RawImage _unlock;

    string[] _name = { "Home", "School", "Toy", "Playground", "Forest", "Dinosaur", "Planet", "Pipe", "Lab", "Sea" };
    Queue<StageInfo> _queue; //두번째 stage만 담고 있는 Queue 
    int _stageIndex; //Player의 현재 위치를 나타내기 위한 변수

    private void Awake()
    {
        _queue = new Queue<StageInfo>();
        _unlock.enabled = false;
        _stageIndex = -1;
    }

    //UI 비활성화
    public void DisableUI()
    {
        _stageIndex = -1;

        _curText.enabled = false;
        for (int i = 0; i < _image.Length; i++)
        {
            ResetCurrentStage(i);
            _image[i].SetActive(false);
        }

        _queue.Clear();
    }

    //기억 조각 스테이지를 큐에 넣어준다.
    public void SetInfo(int concept, int secondStage) { _queue.Enqueue(new StageInfo(concept, secondStage)); }

    //UI 활성화
    void EnableUI()
    {
        StageInfo info = _queue.Dequeue();
        int concept = info.concept, stage = info.stage;

        _curText.enabled = true;
        _curText.text = _name[concept];

        for (int i = 0; i < _image.Length; i++) _image[i].SetActive(true);

        Debug.Log("스토리 해금? : " + SaveLoadManager.Instance.GetUnlockedMemory()[concept].CheckStory(stage));
        //스토리가 해금이 안됐을 때
        if (!SaveLoadManager.Instance.GetUnlockedMemory()[concept].CheckStory(stage)) _unlock.enabled = true;
        else _unlock.enabled = false;
    }

    //Concept을 초기화 하기 전 포탈을 통과할 때 호출됨
    public void PlusStageIndex()
    {
        if(_stageIndex != -1) ResetCurrentStage(_stageIndex);

        if (_stageIndex == -1 || _stageIndex == 2)
        {
            _stageIndex = -1;
            EnableUI();
        }

        _stageIndex++;
        SetCurrentStage();
    }

    //Player가 있는 UI를 빨간색으로 표시
    void SetCurrentStage()
    {
        Color color = _image[_stageIndex].GetComponent<RawImage>().color;
        color = Color.red;
        color.a = 1;
        _image[_stageIndex].GetComponent<RawImage>().color = color;
    }

    //빨간색으로 표시되어 있던 index를 초기화
    void ResetCurrentStage(int index)
    {
        Color color = _image[index].GetComponent<RawImage>().color;
        color = Color.white;
        color.a = 0.2f;
        _image[index].GetComponent<RawImage>().color = color;
    }
}
