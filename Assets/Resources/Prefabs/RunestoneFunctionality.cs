using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Data.Common;

public class RunestoneFunctionality : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private int skillId;
    [SerializeField] private int locId;
    [SerializeField] private List<int> unlocks;
    [SerializeField] private List<int> requires;
    [SerializeField] private int nodeType; // 0 = Green 1 = Blue 2 = Red 3 = Orange 4 = Purple 5 = Center
    [SerializeField] private List<Sprite> imageStates; //Active, Inactive, Hover
    [SerializeField] private GameObject skillPhoto;
    [SerializeField] private GameObject skillLevel;
    [SerializeField] private string unlockCondition;
    [SerializeField] private List<int> andUnlockCondition = new List<int> { };
    [SerializeField] private List<int> orUnlockCondition = new List<int> { };
    [SerializeField] private List<int> legacyOrUnlockCondition = new List<int> { };
    [SerializeField] private bool locked;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private int currLevel = 0;
    [SerializeField] private int maxLevel;
    [SerializeField] private bool nodeActive;
    [SerializeField] private SkillController controller;
    [SerializeField] private GameObject highlight;
    const float skillLocationX = 0.16f;
    const float skillLocationY = -0.23f;

    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Main Control").GetComponent<SkillController>();
        controller.subscribeToController(this, $"rs{locId}");

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (nodeType > 2 && nodeType != 5)
        {
            skillPhoto.transform.localPosition = new Vector3(skillLocationX, skillLocationY, 0);
            skillLevel.GetComponent<TextMeshPro>().text = formatVisualLevel(currLevel);
        }
        else if (nodeType == 5)
        {
            skillLevel.GetComponent<TextMeshPro>().text = formatVisualLevel(currLevel);
        }
        else
        {
            skillLevel.SetActive(false);
        }
        if (!locked)
        {
            lockObject.SetActive(false);
        }
        if (currLevel < 1)
        {
            skillPhoto.GetComponent<SpriteRenderer>().sprite = imageStates[2];
        }
        else if (currLevel > 0)
        {
            skillPhoto.GetComponent<SpriteRenderer>().sprite = imageStates[0];
        }
        if (locId > 1000)
        {
            setLock(true);
            skillLevel.SetActive(false);
        }
    }

    private string formatVisualLevel(int level)
    {
        if (level < 10)
        {
            return $"0{currLevel}";
        }
        else
        {
            return $"{currLevel}";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currLevel > 0)
        {
            skillPhoto.GetComponent<SpriteRenderer>().sprite = imageStates[1];
        }
        //Debug.Log($"Hovering over {skillId} at location {locId}");
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currLevel > 0)
        {
            skillPhoto.GetComponent<SpriteRenderer>().sprite = imageStates[0];
        }
        //Debug.Log($"Left {skillId} at location {locId}");
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!nodeActive)
            {
                controller.setAsActive(true, locId);
            }
            else if (nodeActive && isUnlocked())
            {
                changeSkillLevel(1);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (nodeActive && isUnlocked())
            {
                changeSkillLevel(-1);
            }
        }
        controller.populateInformation(skillId, currLevel, imageStates[0]);
        //Debug.Log(eventData.button);
        //Debug.Log(eventData);
        //throw new System.NotImplementedException();
    }

    public int getLocId()
    {
        return locId;
    }

    public int getSkillLevel()
    {
        return currLevel;
    }
    public bool isUnlocked()
    {
        return !locked;
    }

    public void setLock(bool lockState, bool stopRecursion = false)
    {
        locked = lockState;
        if (locked)
        {
            lockObject.SetActive(true);
            if (currLevel != 0)
            {
                if (!stopRecursion)
                {
                    changeSkillLevel(0, false);
                }
                else
                {
                    changeSkillLevel(0, false, false);
                }
            }
        }
        else
        {
            lockObject.SetActive(false);
        }
    }

    public void changeSkillLevel(int number, bool treatAsDelta = true, bool lockCheck = true)
    {
        //Update the variable
        if (number > 0 && treatAsDelta)
        {
            if (currLevel < maxLevel)
            {
                currLevel++;
            }
        }
        else if (number < 0 && treatAsDelta && nodeType != 5)
        {
            if (currLevel > 0)
            {
                currLevel--;
            }
        }
        else if (number < 0 && treatAsDelta && nodeType == 5)
        {
            if (currLevel > 1)
            {
                currLevel--;
            }
        }
        else if (!treatAsDelta)
        {
            currLevel = number;
        }
        //Change Photo
        if (currLevel > 0)
        {
            skillPhoto.GetComponent<SpriteRenderer>().sprite = imageStates[1];
        }
        else
        {
            skillPhoto.GetComponent<SpriteRenderer>().sprite = imageStates[2];
        }
        //Update Text
        if (nodeType > 2)
        {
            skillLevel.GetComponent<TextMeshPro>().text = formatVisualLevel(currLevel);
        }
        if (lockCheck)
        {
            controller.lockCheck();
        }


    }

    public void setAsActive(bool active)
    {
        nodeActive = active;
        if (nodeActive)
        {
            highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
        }
    }

    public List<int> getAndCondition()
    {
        if (andUnlockCondition.Count > 0)
        {
            return andUnlockCondition;
        }
        else
        {
            return new List<int> { };
        }
    }

    public List<int> getOrCondition()
    {
        if (orUnlockCondition.Count > 0)
        {
            return orUnlockCondition;
        }
        else
        {
            return new List<int> { };
        }
    }

    public int getSkillId()
    {
        return skillId;
    }
}
