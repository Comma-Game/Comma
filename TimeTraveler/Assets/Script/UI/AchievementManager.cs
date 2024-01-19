using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Story Panels")]
    [SerializeField] public GameObject[] stroyPanels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisablePanels(){
        for(int i=0; i < stroyPanels.Length; i++){
            stroyPanels[i].SetActive(false);
        }
	}

    public void ClickStory(int number){
        DisablePanels();
        stroyPanels[number].SetActive(true);
    }
}
