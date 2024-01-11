using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    bool _isMove = false;
    int _h, _w;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(_isMove)
        {
            transform.position += new Vector3(_w, _h, 0);
            if(!VaildMove())
            {
                transform.position -= new Vector3(_w, _h, 0);
                AddToGrid();
                this.enabled = false;
                GameManager.Instance.check_delete_grid();
            }
        }
    }
    public void move(int h, int w)
    {
        _isMove = true;
        _h = h;
        _w = w;
    }

    public void stop()
    {
        _isMove = false;
    }

    public void turn()
    {
        if (!this.enabled) return;

        Vector3 eulerAngle = transform.rotation.eulerAngles;
        eulerAngle.z = round90(eulerAngle.z + 90.0f);
        transform.rotation = Quaternion.Euler(eulerAngle);
    }

    float round90(float f)
    {
        float r = f % 90;
        return r < 45 ? f - r : f - r + 90;
    }

    bool VaildMove()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);
            
            if (!check_XY(roundX, roundY)) return false;
        }

        return true;
    }

    bool check_XY(int x, int y)
    {
        if (x < 0 || y < 0 || x >= GameManager.Width || y >= GameManager.Height || GameManager.Grid[y, x]) return false;
        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            GameManager.Grid[roundY, roundX] = child;
        }
    }
}
