using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    [Header("heart Panel")]
    [SerializeField] public HeartPanel heartPanelCS;

    public void ChangeHeartTime(float time){
        heartPanelCS.SetFillCooldownMinutes(time);
    }
}
