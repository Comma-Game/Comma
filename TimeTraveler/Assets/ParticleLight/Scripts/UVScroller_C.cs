using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UVScroller_C : MonoBehaviour
{
    //@script ExecuteInEditMode()
    // Scroll main texture based on time

    public float scrollSpeed = 1.0f;
    public float MainoffsetX = 0.0f;
    public float MainoffsetY = 0.0f;

    public bool UseCustomTex = false;
    public string CustomTexName = "";

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        if (UseCustomTex) {
            GetComponent<Image>().material.SetTextureOffset(CustomTexName, new Vector2(MainoffsetX * offset, MainoffsetY * offset));
        }
        else {
            GetComponent<Image>().material.SetTextureOffset("_MainTex", new Vector2(MainoffsetX * offset, MainoffsetY * offset));

        }
    }
}