using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : MonoBehaviour
{
    public void speed_up()
    {
        GameManager.Instance.speed_up();
    }

    public void move_right()
    {
        GameManager.Instance.Cur_Block.move(0, 1);
    }
    public void move_left()
    {
        GameManager.Instance.Cur_Block.move(0, -1);
    }
    public void move_up()
    {
        GameManager.Instance.Cur_Block.move(1, 0);
    }
    public void move_down()
    {
        GameManager.Instance.Cur_Block.move(-1, 0);
    }
    public void stop()
    {
        GameManager.Instance.Cur_Block.stop();
    }
    public void turn()
    {
        GameManager.Instance.Cur_Block.turn();
    }
}
