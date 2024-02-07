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
        // 인스턴스가 없으면 생성
        if (_instance == null)
        {
            GameObject gameData = GameObject.Find("TestConceptButton");

            // 만약 Scene에 GameManager가 없으면 새로 생성
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

    public void AddTestConcept(int num) { _testConcept.Add(num); } //테스트할 Concept Add
    public List<int> GetTestConcept() { return _testConcept; } //테스트 하고 싶은 Concept List 반환
    public void ResetTestConcept() { _testConcept.Clear(); } //Concept List Reset
}
