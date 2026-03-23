using System;
using UnityEngine;

public class WebGLFix : MonoBehaviour
{
    public void pulseEnd()
    {
        foreach(Transform child in this.transform)
        {
            try
            {
               child.GetComponent<RunestoneFunctionality>().pulseTooltipEnd(); 
            }
            catch(Exception e)
            {
                //Let us just ignore the error...
                continue;
            }
        }
    }
}
