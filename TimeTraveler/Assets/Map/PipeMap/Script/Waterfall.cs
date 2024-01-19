using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Waterfall : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.1f;

    private float curY;
    // Start is called before the first frame update
    void Start()
    {
        curY = GetComponent<MeshRenderer>().material.mainTextureOffset.y;
    }
    private void Update()
    {
        curY += Time.deltaTime * speed;
        GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(GetComponent<MeshRenderer>().material.mainTextureOffset.x, curY));
    }
}
