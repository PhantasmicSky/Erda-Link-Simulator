using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class changeBoxFx : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputLow;
    [SerializeField] private TMP_InputField inputHigh;
    [SerializeField] private calcModeFunctions parent;
    [SerializeField] private Image displayPic;
    [SerializeField] private int maxLevel;
    [SerializeField] private TextMeshProUGUI maxlev;
    [SerializeField] private TextMeshProUGUI err;

    public void initializeBox(int maxLv, Sprite pic, calcModeFunctions pnt)
    {
        parent = pnt;
        displayPic.sprite = pic;
        maxLevel = maxLv;
        maxlev.text = $"Max Lv. {maxLevel}";
    }

    public void checkDigitAndPass()
    {
        if (Convert.ToInt32(inputLow.text) < Convert.ToInt32(inputHigh.text))
        {
            parent.passFunction(Convert.ToInt32(inputLow.text), Convert.ToInt32(inputHigh.text));
        }
        else
        {
            err.text = "Invalid Inputs. Start Lv. must be lower than End Lv.";
        }
    }

    public void canceledBox()
    {
        DestroyImmediate(this.gameObject);
    }

    public void checkDigit(string pos)
    {
        if (pos == "low")
        {
            if (Convert.ToInt16(inputLow.text) < 0)
            {
                inputLow.text = "0";
            }
            else if (Convert.ToInt16(inputLow.text) > maxLevel - 1)
            {
                inputLow.text = $"{maxLevel - 1}";
            }
        }
        else if (pos == "hi")
        {
            if (Convert.ToInt16(inputHigh.text) < 1)
            {
                inputHigh.text = "1";
            }
            else if (Convert.ToInt16(inputHigh.text) > maxLevel)
            {
                inputHigh.text = $"{maxLevel}";
            }
        }
    }
}
