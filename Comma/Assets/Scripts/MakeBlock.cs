using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBlock : MonoBehaviour
{
    [SerializeField]
    GameObject[] _block;
    Block _blockControl;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void make_block(int type)
    {
        GameObject block = Instantiate(_block[type], transform.position, Quaternion.identity);
        block.transform.transform.SetParent(transform);
        GameManager.Instance.Cur_Block = block.GetComponent<Block>();
    }
}
