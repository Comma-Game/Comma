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
    Queue<int> _memoryStageQueue; //�ι�° stage�� ��� �ִ� Queue 
    int _stageIndex; //Player�� ���� ��ġ�� ��Ÿ���� ���� ����

    private void Awake()
    {
        _concept = -1;
        _memoryStageQueue = new Queue<int>();
        _stageIndex = 0;
    }

    //UI ��Ȱ��ȭ
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

    //��Ż�� ����ϰ� UI �ֽ�ȭ
    public void SetInfo(int concept, int secondStage)
    {
        _concept = concept;
        _stageIndex = 0;

        //�ι�° ���������� �־��ش�
        _memoryStageQueue.Enqueue(secondStage);

        EnableUI();
        SetCurrentStage();
    }

    //UI Ȱ��ȭ
    void EnableUI()
    {
        _curText.enabled = true;
        _curText.text = _name[_concept];

        for (int i = 0; i < _image.Length; i++) _image[i].enabled = true;

        int stage = _memoryStageQueue.Dequeue();

        //���丮�� �ر��� �ȵ��� ��
        if (!SaveLoadManager.Instance.GetUnlockedMemory()[_concept].CheckStory(stage)) _unlock.enabled = true;
        else _unlock.enabled = false;
    }

    //Concept�� �ʱ�ȭ �ϱ� �� ��Ż�� ����� �� ȣ���
    public void PlusStageIndex()
    {
        ResetCurrentStage(_stageIndex);
        _stageIndex++;
    }

    //Player�� �ִ� UI�� ���������� ǥ��
    void SetCurrentStage()
    {
        Color color = _image[_stageIndex].color;
        color = Color.red;
        color.a = 1;
        _image[_stageIndex].color = color;
    }

    //���������� ǥ�õǾ� �ִ� indedx�� �ʱ�ȭ
    void ResetCurrentStage(int index)
    {
        Color color = _image[index].color;
        color = Color.white;
        color.a = 0.2f;
        _image[index].color = color;
    }
}
