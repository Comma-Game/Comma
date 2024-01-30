using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPanel : MonoBehaviour
{
    [SerializeField] private GameObject speedPanelObj;
    [SerializeField] private MaterialPropertyController materialPropertyController;
    
    public void OnSpeedPanel(bool isActive){
        speedPanelObj.SetActive(isActive);
    }

    public void ChangeSpeedColor(int num){
        if(num == 0){
            materialPropertyController.materials[1].SetColor("_Color", Color.gray);
        }else if(num == 1){
            materialPropertyController.materials[1].SetColor("_Color", Color.blue);
        }
    }

    public void ChangeSpeed(float num){
        materialPropertyController.materials[1].SetFloat("_Speed", num);
    }
}

//  53 53 53