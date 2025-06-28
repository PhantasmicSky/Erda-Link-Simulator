using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private Dictionary<string, RunestoneFunctionality> skills = new Dictionary<string, RunestoneFunctionality> { };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
}
