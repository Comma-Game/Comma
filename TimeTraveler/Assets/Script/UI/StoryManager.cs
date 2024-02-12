using System.Collections;
using System.Collections.Generic;
using Doublsb.Dialog;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] public DialogManager DialogManager;
    [SerializeField] public GameObject background;
    [SerializeField] public GameObject dialogPanel;
    [SerializeField] public GameObject[] Example;
    private List<DialogData> dialogTexts = new List<DialogData>();
    public bool isPlay = false;
    private bool isDialogActive = false;
    private int dialogCount = 0;

    private static StoryManager instance;
    // singleton
    public static StoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StoryManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("StoryManager");
                    instance = obj.AddComponent<StoryManager>();
                }
            }
            return instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(DialogManager.state);
        if(isPlay){
            ForestStroy();
            isPlay = false;
        }

        if(isDialogActive){
            if(DialogManager.state == State.Deactivate){
                dialogCount += 1;
                if(dialogCount > 3){
                    isPanelOn(false);
                }
            }
        }
    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }

    private void ForestStroy(){
        dialogTexts.Clear();
        isPanelOn(true);
        isDialogActive = true;
        dialogCount = 0;
        dialogTexts.Add(new DialogData("I am Sa. Popped out to let you know Asset can show other characters."));
        dialogTexts.Add(new DialogData("I am Sa. "));
        dialogTexts.Add(new DialogData("Popped out to let you know Asset can show other characters."));
        DialogManager.Show(dialogTexts);
    }

    public void isPanelOn(bool isActive){
        if(isActive){
            background.SetActive(true);
            dialogPanel.SetActive(true);
        }else{
            background.SetActive(false);
            dialogPanel.SetActive(false);
        }
    }

    public void ShowStory(int conceptNum, int storyNum){
        ForestStroy();
    }

    ////////////////////////////////////////
    ///

    
}
