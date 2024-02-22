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
    Queue<StageInfo> _queue; //�ι�° stage�� ��� �ִ� Queue 
    int _stageIndex; //Player�� ���� ��ġ�� ��Ÿ���� ���� ����

    private void Awake()
    {
        _queue = new Queue<StageInfo>();
        _unlock.enabled = false;
        _stageIndex = -1;
    }

    //UI ��Ȱ��ȭ
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

    //��� ���� ���������� ť�� �־��ش�.
    public void SetInfo(int concept, int secondStage) { _queue.Enqueue(new StageInfo(concept, secondStage)); }

    //UI Ȱ��ȭ
    void EnableUI()
    {
        StageInfo info = _queue.Dequeue();
        int concept = info.concept, stage = info.stage;

        _curText.enabled = true;
        _curText.text = _name[concept];

        for (int i = 0; i < _image.Length; i++) _image[i].SetActive(true);

        Debug.Log("���丮 �ر�? : " + SaveLoadManager.Instance.GetUnlockedMemory()[concept].CheckStory(stage));
        //���丮�� �ر��� �ȵ��� ��
        if (!SaveLoadManager.Instance.GetUnlockedMemory()[concept].CheckStory(stage)) _unlock.enabled = true;
        else _unlock.enabled = false;
    }

    //Concept�� �ʱ�ȭ �ϱ� �� ��Ż�� ����� �� ȣ���
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

    //Player�� �ִ� UI�� ���������� ǥ��
    void SetCurrentStage()
    {
        Color color = _image[_stageIndex].GetComponent<RawImage>().color;
        color = Color.red;
        color.a = 1;
        _image[_stageIndex].GetComponent<RawImage>().color = color;
    }

    //���������� ǥ�õǾ� �ִ� index�� �ʱ�ȭ
    void ResetCurrentStage(int index)
    {
        Color color = _image[index].GetComponent<RawImage>().color;
        color = Color.white;
        color.a = 0.2f;
        _image[index].GetComponent<RawImage>().color = color;
    }
}
