using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderRange : MonoBehaviour
{
    public void EnableRawImage()
    {
        gameObject.GetComponent<RawImage>().color = Color.red;
    }

    public void DisableRawImage()
    {
        gameObject.GetComponent<RawImage>().color = Color.white;
        gameObject.GetComponent<RawImage>().enabled = false;
    }

    public void SetInvincible()
    {
        gameObject.GetComponent<RawImage>().color = Color.yellow;
    }

    public void SetSkill()
    {
        gameObject.GetComponent<RawImage>().color = Color.blue;
    }

    public void ReSetColor()
    {
        gameObject.GetComponent<RawImage>().color = Color.red;
    }
}
