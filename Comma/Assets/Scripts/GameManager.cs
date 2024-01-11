using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            init();
            return _instance;
        }
    }

    Block _block;
    public Block Cur_Block
    {
        get
        {
            return _block;
        }
        set
        {
            _block = value;
        }
    }

    static int _height = 20, _width = 10;
    public static int Height
    {
        get
        {
            return _height;
        }
    }
    public static int Width
    {
        get
        {
            return _width;
        }
    }
    
    static int _moveSpeed = 1;
    public static int MoveSpeed
    {
        get
        {
            return _moveSpeed;
        }
    }

    static Transform[,] _grid = new Transform[GameManager.Height, GameManager.Width];
    public static Transform[,] Grid
    {
        get
        {
            return _grid;
        }
    }

    void Start()
    {
        init();   
    }

    void Update()
    {
        
    }

    static void init()
    {
        if (!_instance)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (!gameManager)
            {
                gameManager = new GameObject();
                gameManager.name = "GameManager";
                gameManager.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(gameManager);
            _instance = gameManager.GetComponent<GameManager>();
        }
    }

    public void speed_up()
    {
        _moveSpeed++;
    }

    bool has_line(int row)
    {
        for(int i = 0; i < _width; i++)
        {
            if (!_grid[row, i]) return false;
        }

        return true;
    }

    void delete_line(int row)
    {
        for(int i = 0; i < _width; i++)
        {
            Destroy(_grid[row, i].gameObject);
            _grid[row, i] = null;
        }
    }

    void row_down(int row)
    {
        for(int i = row; i < _height; i++)
        {
            for(int j = 0; j < _width; j++)
            {
                if(!_grid[i, j])
                {
                    _grid[i - 1, j] = _grid[i, j];
                    _grid[i, j] = null;
                    _grid[i - 1, j].transform.position -= Vector3.up;
                }
            }
        }
    }

    public void check_delete_grid()
    {
        for(int i = 0; i < _height; i++)
        {
            if(has_line(i))
            {
                delete_line(i);
                row_down(i);
            }
        }
    }
}
