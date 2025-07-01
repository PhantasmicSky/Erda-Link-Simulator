using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Data.Common;
using UnityEngine.UI;

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
    [SerializeField] private List<int> lightLevel = new List<int> { 0, 0, 0, 0 }; //clockwise starting from top i.e. Top Right Bottom Left
    [SerializeField] private List<int> lightLevelRequirement = new List<int> { 0, 0, 0, 0 }; //clockwise starting from top i.e. Top Right Bottom Left
    [SerializeField] private TextMeshPro tooltipText;
    [SerializeField] private RectTransform tooltipBG;
    const float skillLocationX = 0.16f;
    const float skillLocationY = -0.23f;
    [SerializeField] private bool calcInclude;
    [SerializeField] private List<int> calcRange = new List<int> { 0, 1 };
    [SerializeField] private GameObject calcIdentifier;
    [SerializeField] private TextMeshPro calcText;

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

        //Show Tooltip
        if (controller.showTooltip)
        {
            if (nodeType != 3 && nodeType != 4 && nodeType != 5 && skillId != 300 && skillId != 304)
            {
                tooltipText.text = controller.skillInformation[$"rid{skillId}"].skillDescription;
            }
            else
            {
                tooltipText.text = controller.skillInformation[$"rid{skillId}"].skillName;
            }
            if (nodeType != 5)
            {
                tooltipText.GetComponent<RectTransform>().localPosition = new Vector3(0.32f, 0.1f, 0);
            }
            else
            {
                tooltipText.GetComponent<RectTransform>().localPosition = new Vector3(0.21f, -0.512f, 0);
            }
            tooltipText.enabled = true;
            tooltipBG.gameObject.SetActive(true);
            tooltipBG.localScale = new Vector3(tooltipText.preferredWidth, 0.16f, 1);
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

        if (controller.showTooltip)
        {
            tooltipText.enabled = false;
            tooltipBG.gameObject.SetActive(false);
        }
        //Debug.Log($"Left {skillId} at location {locId}");
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!controller.calcMode) //Link Mode
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                controller.populateInformation(skillId, currLevel, imageStates[0]);
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
        }
        else //Free Calculator Mode
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (skillId <= 10000)
                {
                    calcInclude = true;
                    toggleCalcGraphic(true, false);
                }
                // show calc Include graphic
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                calcInclude = false;
                toggleCalcGraphic(false, false);
                //hide calcInclude Graphic
            }
            controller.populateTotals();
        }
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
        if (treatAsDelta)
        {
            if (number > 0)
            {
                if (currLevel < maxLevel)
                {
                    currLevel++;
                }
            }
            else if (number < 0 && nodeType != 5)
            {
                if (currLevel > 0)
                {
                    currLevel--;
                }
            }
            else if (number < 0 && nodeType == 5)
            {
                if (currLevel > 1)
                {
                    currLevel--;
                }
            }
        }
        else
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
        controller.lockCheck(); //LOCK CHECK HERE
        controller.computeLight();
        controller.lightCheck();
        controller.populateTotals();
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

    public List<int> getLightLevel()
    {
        return lightLevel;
    }

    public List<int> getLightCondition()
    {
        return lightLevelRequirement;
    }

    public void toggleLockGraphic(bool toggle)
    {
        lockObject.SetActive(toggle);
    }

    public void toggleCalcGraphic(bool state, bool checkState)
    {
        if (checkState)
        {
            calcIdentifier.SetActive(calcInclude);
            //Conditional if calc graphic will show check status of includeCalc
        }
        else
        {
            calcIdentifier.SetActive(state);
            //make calc graphic all go to state's state.
        }
    }

    public bool isPartOfCalc()
    {
        return calcInclude;
    }

    public List<int> calcLv()
    {
        return calcRange;
    }

    public void changeCalcRange(int lower, int higher)
    {
        calcRange[0] = lower;
        calcRange[1] = higher;
        calcText.text = $"{lower} > {higher}";
        controller.populateTotals();
    }

    public Sprite getPhoto()
    {
        return imageStates[0];
    }

    public int getMaxLevel()
    {
        return maxLevel;
    }
}
