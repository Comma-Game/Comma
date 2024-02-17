using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    Player _player;
    int _buff;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _buff = SaveLoadManager.Instance.GetBuff();

        SaveLoadManager.Instance.SetBuff(0);
        UseBuff();
    }

    void UseBuff()
    {
        switch (_buff)
        {
            case 1:
                _player.PlusMaxHp(50);
                break;
            case 2:
                PlayGameManager.Instance.SetCoinBuff();
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
                _player.SetEnergyBuff();
                break;
        }

        CanvasController.Instance.ChangeBuffImage(_buff);
    }
}
