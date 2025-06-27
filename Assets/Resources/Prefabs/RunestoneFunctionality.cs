using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class RunestoneFunctionality : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private int skillId;
    [SerializeField] private int locId;
    [SerializeField] private List<int> unlocks;
    [SerializeField] private List<int> requires;
    [SerializeField] private int nodeType; // 0 = Green 1 = Blue 2 = Red 3 = Orange 4 = Purple
    [SerializeField] private List<Sprite> imageStates; //Active, Inactive, Hover
    [SerializeField] private GameObject skillPhoto;
    [SerializeField] private GameObject skillLevel;
    [SerializeField] private string unlockCondition;
    [SerializeField] private List<int> andUnlockCondition;
    [SerializeField] private List<int> orUnlockCondition;
    const float skillLocationX = 0.16f;
    const float skillLocationY = -0.23f; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(nodeType > 2)
        {
            skillPhoto.transform.localPosition = new Vector3(skillLocationX, skillLocationY, 0);
        }
        else
        {
            skillLevel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Hovering over {skillId} at location {locId}");
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Left {skillId} at location {locId}");
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.button);
        Debug.Log(eventData);
        //throw new System.NotImplementedException();
    }
}
