using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    private Dictionary<string, RunestoneFunctionality> skills = new Dictionary<string, RunestoneFunctionality> { };
    public TextAsset skillInformationJSON;
    public Dictionary<string, SkillInfo> skillInformation;
    private List<string> checkOrder = new List<string> { "rs0", "rs1", "rs2", "rs3", "rs4", "rs5", "rs6", "rs7", "rs8", "rs9", "rs10", "rs11", "rs12", "rs13", "rs14", "rs15", "rs16", "rs200", "rs201", "rs202", "rs203", "rs204", "rs205", "rs206", "rs207", "rs208", "rs209", "rs210", "rs211", "rs212", "rs213", "rs214", "rs215", "rs400", "rs401", "rs402", "rs403", "rs404", "rs405", "rs406", "rs407", "rs408", "rs409", "rs410", "rs411", "rs412", "rs413", "rs414", "rs415", "rs416", "rs417", "rs600", "rs601", "rs602", "rs603", "rs604", "rs605", "rs606", "rs607", "rs608", "rs609", "rs610", "rs611", "rs612", "rs613", "rs614", "rs615", "rs616", "rs1000", "rs1001", "rs1002", "rs1003", "rs1004", "rs1005" };
    [SerializeField] TextMeshProUGUI uiSkillName, uiSkillDescription, uiLevels, uiSECost, uiSEFCost;
    [SerializeField] Image skillIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        //JsonConvert
        skillInformation = JsonConvert.DeserializeObject<Dictionary<string, SkillInfo>>(skillInformationJSON.text);
        Debug.Log(skillInformation["rid1"].skillName);
    }
    void Start()
    {
        lockCheck();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void subscribeToController(RunestoneFunctionality rf, string loc)
    {
        skills.Add(loc, rf);
    }

    public void setAsActive(bool active, int locId)
    {
        foreach (KeyValuePair<string, RunestoneFunctionality> item in skills)
        {
            if (active)
            {
                if (item.Key == $"rs{locId}")
                {
                    item.Value.setAsActive(true);
                }
                else
                {
                    item.Value.setAsActive(false);
                }
            }
            //code
        }
    }

    public void lockCheck()
    {
        bool lockFlag = false;
        foreach (string key in checkOrder)
        {
            if (skills[key].getAndCondition().Count > 0)
            {
                List<int> _tempCondition = skills[key].getAndCondition();
                foreach (int condition in _tempCondition)
                {
                    if (!skills[$"rs{condition}"].isUnlocked() || skills[$"rs{condition}"].getSkillLevel() == 0)
                    {
                        lockFlag = true;
                        break;
                    }
                }
            }
            if (skills[key].getOrCondition().Count > 0)
            {
                bool foundUnlocked = false;
                List<int> _tempCondition = skills[key].getOrCondition();
                foreach (int condition in _tempCondition)
                {
                    if (skills[$"rs{condition}"].isUnlocked() && skills[$"rs{condition}"].getSkillLevel() > 0)
                    {
                        foundUnlocked = true;
                        break;
                    }
                }
                if (!foundUnlocked)
                {
                    lockFlag = true;
                }
            }
            if (key == "rs1001" || key == "rs1002" || key == "rs1003" || key == "rs1004" || key == "rs1005")
            {
                lockFlag = true;
            }
            //Debug.Log($"Lock Status for {item.Key} : {lockFlag} = {item.Value.getAndCondition().Count} {item.Value.getOrCondition().Count}");
            if (!lockFlag)
            {
                skills[key].setLock(false);
            }
            else
            {
                skills[key].setLock(true);
            }
            lockFlag = false;
        }
    }
    public void lockCheckOld()
    {
        bool lockFlag = false;
        foreach (KeyValuePair<string, RunestoneFunctionality> item in skills)
        {
            if (item.Value.getAndCondition().Count > 0)
            {
                List<int> _tempCondition = item.Value.getAndCondition();
                foreach (int condition in _tempCondition)
                {
                    if (!skills[$"rs{condition}"].isUnlocked() || skills[$"rs{condition}"].getSkillLevel() == 0)
                    {
                        lockFlag = true;
                        break;
                    }
                }
            }
            if (item.Value.getOrCondition().Count > 0)
            {
                bool foundUnlocked = false;
                List<int> _tempCondition = item.Value.getOrCondition();
                foreach (int condition in _tempCondition)
                {
                    if (skills[$"rs{condition}"].isUnlocked() && skills[$"rs{condition}"].getSkillLevel() > 0)
                    {
                        foundUnlocked = true;
                        break;
                    }
                }
                if (!foundUnlocked)
                {
                    lockFlag = true;
                }
            }
            if (item.Key == "rs1001" || item.Key == "rs1002" || item.Key == "rs1003" || item.Key == "rs1004" || item.Key == "rs1005")
            {
                lockFlag = true;
            }
            //Debug.Log($"Lock Status for {item.Key} : {lockFlag} = {item.Value.getAndCondition().Count} {item.Value.getOrCondition().Count}");
            if (!lockFlag)
            {
                item.Value.setLock(false);
            }
            else
            {
                item.Value.setLock(true);
            }
            lockFlag = false;
        }
    }

    public void populateInformation(int skillId, int skillLev, Sprite sIcon)
    {
        uiSkillName.text = skillInformation[$"rid{skillId}"].skillName;
        uiSkillDescription.text = skillInformation[$"rid{skillId}"].skillDescription;
        uiLevels.text = $"Lv.{skillLev} / Lv.{skillInformation[$"rid{skillId}"].maxLevel}";
        try
        {
            uiSECost.text = $"{skillInformation[$"rid{skillId}"].solErdaCost[skillLev]}";
            uiSEFCost.text = $"{skillInformation[$"rid{skillId}"].solErdaFragCost[skillLev]}";
        }
        catch
        {
            uiSECost.text = "--";
            uiSEFCost.text = "--";
        }
        skillIcon.sprite = sIcon;
    }

}
