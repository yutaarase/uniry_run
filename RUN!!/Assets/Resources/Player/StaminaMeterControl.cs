using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerのHPBarを操作するクラス
/// </summary>
public class StaminaMeterControl : MonoBehaviour
{
    [Header("体力")] public float maxStamina;
    public bool poop = false;
    public bool CntJump = false;
    public Slider StaminaMeter;
    public Image sliderImage;
    private float currentStamina;
    private float currentX;
    private float minX, maxX;
    private float dY; // デフォルトY座標
    private float dX; // デフォルトY座標



    // Start is called before the first frame update
    void Start()
    {
        StaminaMeter.value = 1;
        currentStamina = maxStamina;
        dY = StaminaMeter.transform.localPosition.y;
        dX = StaminaMeter.transform.localPosition.x;
    }

    public float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    }

    //HPBar更新
    public void StaminaMeterUpdate(float diff)
    {

        currentStamina = currentStamina - diff;

        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }

        StaminaMeter.value = currentStamina / maxStamina;

        //X軸方向のどの位置まで移動しなければならないか計算
        currentX = MapValues(currentStamina, 0, maxStamina, minX, maxX);
        
        StaminaMeter.transform.localPosition = new Vector3(dX, dY, 0);

        //50%以上であれば緑から黄色へ、HPが50%未満であれば黄色から赤色へ変化する
        if (currentStamina > maxStamina / 2)
        {
            //色はRGB表記
            //最初は(R=0,G=255,B=0)で開始、Rを0→255に変化させて緑→黄色
            sliderImage.color = new Color32((byte)MapValues(currentStamina, maxStamina / 2, maxStamina, 255, 0), 255, 0, 255);
        }
        else
        {
            //50%未満では(R=255,G=255,B=0)で開始、Gを255→0に変化させて黄色→赤
            sliderImage.color = new Color32(255, (byte)MapValues(currentStamina, 0, maxStamina / 2, 0, 255), 0, 255);
        }

        if (currentStamina <= 0)
        {
            poop = true;
        }
        else
        {
            if(currentStamina < 20)
            {
                CntJump = true;
            }
            else
            {
                CntJump = false;
            }
            poop = false;
        }
    }
}
