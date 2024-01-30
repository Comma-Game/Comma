using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private string[] _buffTexts = {
                "������ ����",
                "ü�°�ȭ + 50",
                "���ھ� 2��",
                "��ֹ� �浹���� 50% ����",
                "ü�°��� 15% ����",
                "���� ȸ���� 20% ����",
                "���ӵ� +20%",
                "�⺻�ӵ� +20%",
                "������ ������ ���� -20%"
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
