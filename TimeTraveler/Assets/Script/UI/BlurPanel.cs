using System.Collections;
using System.Collections.Generic;
using Krivodeling.UI.Effects;
using UnityEngine;

public class BlurPanel : MonoBehaviour
{
    [SerializeField] private UIBlur image1;
    [SerializeField] private UIBlur image2;
    [SerializeField] private UIBlur image3;
    [SerializeField] private UIBlur image4;

    [SerializeField] private float blurNum;

    [SerializeField] private bool isBlurChange;

    // Update is called once per frame
    void Update()
    {
        if(isBlurChange){
            isBlurChange = false;
            ChangeBlurNum(blurNum);
        }
    }

    public void ChangeBlurNum(float blurNum){
        image1.Multiplier = blurNum;
        image2.Multiplier = blurNum;
        image3.Multiplier = blurNum;
        image4.Multiplier = blurNum;
    }
}
