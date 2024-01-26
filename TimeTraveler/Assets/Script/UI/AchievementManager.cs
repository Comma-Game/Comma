using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Story Panels")]
    [SerializeField] public GameObject[] stroyPanels;

    private void DisablePanels(){
        for(int i=0; i < stroyPanels.Length; i++){
            stroyPanels[i].SetActive(false);
        }
	}

    public void ClickStory(int number){
        Debug.Log("ClickStory : " + number);
        DisablePanels();
        stroyPanels[number].SetActive(true);
    }
}
