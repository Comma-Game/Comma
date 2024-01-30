using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private string[] _buffTexts = {
                "아이템 없음",
                "체력강화 + 50",
                "스코어 2배",
                "장애물 충돌피해 50% 감소",
                "체력감소 15% 느림",
                "물약 회복량 20% 증가",
                "가속도 +20%",
                "기본속도 +20%",
                "에너지 축적량 감소 -20%"
        };

    Player _player;
    int _buff;
    TextMeshProUGUI _buffText;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _buffText = GameObject.Find("BuffText").GetComponent<TextMeshProUGUI>();
        _buff = SaveLoadManager.Instance.GetBuff();

        Debug.Log("Buff : " + _buff);
        SaveLoadManager.Instance.SetBuff(0);
        UseBuff();
    }

    void UseBuff()
    {
        switch (_buff)
        {
            case 1:
                _player.Heal(50);
                break;
            case 2:
                PlayGameManager.Instance.SetScoreBuff();
                break;
            case 3:
                _player.SetObstacleDamageBuff();
                break;
            case 4:
                _player.SetTimeDamageBuff();
                break;
            case 5:
                _player.SetHealBuff(0.3f);
                break;
            case 6:
                StageController.Instance.SetAccDeBuff();
                break;
            case 7:
                StageController.Instance.SetBasicSpeedDeBuff();
                break;
            case 8:
                _player.SetEnergyDeBuff();
                break;
        }
        _buffText.text = _buffTexts[_buff];
    }
}
