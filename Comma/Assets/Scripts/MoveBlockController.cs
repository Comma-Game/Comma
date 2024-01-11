using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockController : MonoBehaviour
{
    private float timer = 0f;
    public float delayTime = 1; // 블럭이 내려가는 딜레이 타임
    public bool _isUpMove = true; // 위로 움직이는지? 아래로 움직이는지?
    // public bool IsUpMove
    // {
    //     get { return _isUpMove; }
    //     set { _isUpMove = value; }
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delayTime)
        {
            if(GameManager.Instance.Cur_Block != null && GameManager.Instance.Cur_Block.IsFirst){
                if(_isUpMove) GameManager.Instance.Cur_Block.move(1, 0);
                else GameManager.Instance.Cur_Block.move(-1, 0);
            }
            timer = 0f;
        }
    }
}
