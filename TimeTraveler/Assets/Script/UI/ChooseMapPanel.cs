using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class ChooseMapPanel : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI chooseMapText;
    private int[] map_Check = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private List<int> chooseMapList = new List<int>();
    public List<int> GetChooseMapList() {return chooseMapList;}
    //private bool isCheck = true;

    private static ChooseMapPanel instance;
    // singleton
    public static ChooseMapPanel Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<ChooseMapPanel>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("MapChooseAllPanelObj");
                    instance = obj.AddComponent<ChooseMapPanel>();
                }
            }
            return instance;
        }
    }

    private string[] map_Text = {
                "Forest",
                "Sea",
                "School",
                "Playground",
                "House",
                "Toy",
                "Dino",
                "Pipe",
                "Lab",
                "Planet"
    };

    public void ChooseMap(int num){
        AudioManager.Instance.PlayGameButtonClick();
        if(map_Check[num] == 0){
            Debug.Log("GetTestConcept : " +  TestConceptButton.Instance.GetTestConcept().Count);
            // 처음 클릭이고
            // if(isCheck){
            //     if(TestConceptButton.Instance.GetTestConcept().Count > 0) {
            //         TestConceptButton.Instance.ResetTestConcept();
            //         Debug.Log("GetTestConcept Reset");
            //     }
            //     isCheck = false;
            // }
            map_Check[num] = 1;
            chooseMapList.Add(num);
            TestConceptButton.Instance.AddTestConcept(num);
            if(chooseMapText.text == "") chooseMapText.text = map_Text[num];
            else chooseMapText.text = chooseMapText.text + " -> " + map_Text[num];
            //PrintIntList(chooseMapList);
        }
    }

    public void ResetChooseMap(){
        AudioManager.Instance.PlayGameButtonClick();
        for(int i=0; i<map_Check.Length; i++){
            map_Check[i] = 0;
            chooseMapText.text = "";
        }
        TestConceptButton.Instance.ResetTestConcept();
        chooseMapList.Clear();
        //PrintIntList(chooseMapList);
    }

    private void PrintIntList(List<int> intList){
        for(int i=0; i<intList.Count; i++){
            Debug.Log(intList[i]);
        }
        if(intList.Count == 0) Debug.Log("intList.Count : " + 0);
    }
}
