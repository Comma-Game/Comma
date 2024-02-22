using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _curText;

    [SerializeField]
    RawImage[] _image;

    [SerializeField]
    RawImage _unlock;

    string[] _name = { "Forest", "Sea", "School", "Playground", "Home", "Toy", "Dinosaur", "Pipe", "Lab", "Planet" };
    int _concept;
    Queue<int> _memoryStageQueue; //두번째 stage만 담고 있는 Queue 
    int _stageIndex; //Player의 현재 위치를 나타내기 위한 변수

    private void Awake()
    {
        _concept = -1;
        _memoryStageQueue = new Queue<int>();
        _stageIndex = 0;
    }

    //UI 비활성화
    public void DisableUI()
    {
        _curText.enabled = false;
        for (int i = 0; i < _image.Length; i++)
        {
            ResetCurrentStage(i);
            _image[i].enabled = false;
        }

        _memoryStageQueue.Clear();
    }

    //포탈을 통과하고 UI 최신화
    public void SetInfo(int concept, int secondStage)
    {
        _concept = concept;
        _stageIndex = 0;

        //두번째 스테이지만 넣어준다
        _memoryStageQueue.Enqueue(secondStage);

        EnableUI();
        SetCurrentStage();
    }

    //UI 활성화
    void EnableUI()
    {
        _curText.enabled = true;
        _curText.text = _name[_concept];

        for (int i = 0; i < _image.Length; i++) _image[i].enabled = true;

        int stage = _memoryStageQueue.Dequeue();

        //스토리가 해금이 안됐을 때
        if (!SaveLoadManager.Instance.GetUnlockedMemory()[_concept].CheckStory(stage)) _unlock.enabled = true;
        else _unlock.enabled = false;
    }

    //Concept을 초기화 하기 전 포탈을 통과할 때 호출됨
    public void PlusStageIndex()
    {
        ResetCurrentStage(_stageIndex);
        _stageIndex++;
    }

    //Player가 있는 UI를 빨간색으로 표시
    void SetCurrentStage()
    {
        Color color = _image[_stageIndex].color;
        color = Color.red;
        color.a = 1;
        _image[_stageIndex].color = color;
    }

    //빨간색으로 표시되어 있던 indedx를 초기화
    void ResetCurrentStage(int index)
    {
        Color color = _image[index].color;
        color = Color.white;
        color.a = 0.2f;
        _image[index].color = color;
    }
}
