using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderRange : MonoBehaviour
{
    [SerializeField]
    Texture[] _textures;

    public void SetInvincible()
    {
        gameObject.GetComponent<RawImage>().texture = _textures[1];
    }

    public void SetSkill()
    {
        gameObject.GetComponent<RawImage>().texture = _textures[2];
    }

    public void ReSetColor()
    {
        gameObject.GetComponent<RawImage>().texture = _textures[0];
    }
}
