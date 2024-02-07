using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] public GameObject tutorialPanel;

    public void OnTutorialPanel(bool isActive){
        AudioManager.Instance.PlayGameButtonClick();
        tutorialPanel.SetActive(isActive);
    }
}
