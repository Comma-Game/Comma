using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConceptButton : MonoBehaviour
{
    private static TestConceptButton _instance;
    // singleton
    public static TestConceptButton Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    static void Init_Instance()
    {
        // �ν��Ͻ��� ������ ����
        if (_instance == null)
        {
            GameObject gameData = GameObject.Find("TestConceptButton");

            // ���� Scene�� GameManager�� ������ ���� ����
            if (gameData == null)
            {
                gameData = new GameObject();
                gameData.name = "TestConceptButton";
                gameData.AddComponent<TestConceptButton>();
            }

            _instance = gameData.GetComponent<TestConceptButton>();
            DontDestroyOnLoad(gameData);
        }
    }

    List<int> _testConcept = new List<int>();

    public void AddTestConcept(int num) { _testConcept.Add(num); } //�׽�Ʈ�� Concept Add
    public List<int> GetTestConcept() { return _testConcept; } //�׽�Ʈ �ϰ� ���� Concept List ��ȯ
    public void ResetTestConcept() { _testConcept.Clear(); } //Concept List Reset
}
