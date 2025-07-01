using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    [SerializeField] private string charName;
    public Dictionary<string, RunestoneFunctionality> skills = new Dictionary<string, RunestoneFunctionality> { };
    public TextAsset skillInformationJSON;
    public Dictionary<string, SkillInfo> skillInformation;
    private List<string> checkOrder = new List<string> { "rs0", "rs1", "rs2", "rs3", "rs4", "rs5", "rs6", "rs7", "rs8", "rs9", "rs10", "rs11", "rs12", "rs13", "rs14", "rs200", "rs201", "rs202", "rs203", "rs204", "rs205", "rs206", "rs207", "rs208", "rs209", "rs210", "rs211", "rs212", "rs213", "rs214", "rs400", "rs401", "rs402", "rs403", "rs404", "rs405", "rs406", "rs407", "rs408", "rs409", "rs410", "rs411", "rs412", "rs413", "rs414", "rs600", "rs601", "rs602", "rs603", "rs604", "rs605", "rs606", "rs607", "rs608", "rs609", "rs610", "rs611", "rs612", "rs613", "rs614", "rs1000"};
    private List<string> checkOrderFull = new List<string> { "rs0", "rs1", "rs2", "rs3", "rs4", "rs5", "rs6", "rs7", "rs8", "rs9", "rs10", "rs11", "rs12", "rs13", "rs14", "rs200", "rs201", "rs202", "rs203", "rs204", "rs205", "rs206", "rs207", "rs208", "rs209", "rs210", "rs211", "rs212", "rs213", "rs214", "rs400", "rs401", "rs402", "rs403", "rs404", "rs405", "rs406", "rs407", "rs408", "rs409", "rs410", "rs411", "rs412", "rs413", "rs414", "rs600", "rs601", "rs602", "rs603", "rs604", "rs605", "rs606", "rs607", "rs608", "rs609", "rs610", "rs611", "rs612", "rs613", "rs614", "rs1000", "rs1001", "rs1002", "rs1003", "rs1004", "rs1005" };
    private List<string> lightAffectedNodes = new List<string> { "rs15", "rs16", "rs215", "rs415", "rs416", "rs417", "rs615", "rs616" };
    [SerializeField] TextMeshProUGUI uiSkillName, uiSkillDescription, uiLevels, uiSECost, uiSEFCost;
    [SerializeField] Image skillIcon;
    [SerializeField] Image stoneType;
    [SerializeField] TextMeshProUGUI uiTotSE, uiTotSEF;
    [SerializeField] List<Sprite> stoneTypeImg;
    private int runningTotalSE = 0;
    private int runningTotalSEF = 0;
    [SerializeField] PathController pc;
    [SerializeField] private List<int> currLightLevels = new List<int> { 0, 0, 0, 0 };
    const float centerX = 8.32f;
    const float centerY = -5.825f;
    const float expansion = 0.9f;
    const float slope = 2.0f;
    const float baseSize = 3.18f;
    const float b = 19.82f;
    //const float test = 18.88f;
    [SerializeField] private GameObject lightBoard;
    [SerializeField] private TextMeshProUGUI summary;
    [SerializeField] private TextMeshProUGUI stoneTypeTextOut;
    [SerializeField] private List<string> stoneTypeTxt = new List<string> {"Rush Stone", "Rush Stone (End)", "Boost Stone", "Skill Stone", "Ultimate Stone", "Origin Stone"};
    [SerializeField] private List<string> stoneTypeCC = new List<string> { "#339944", "#1166CC", "#DD2133", "#EE8822", "#C52197", "#9D46CC" };
    public bool showTooltip = true;
    public bool calcMode = false;
    public List<Color32> bgModeColorsL = new List<Color32>
    {
        new Color32(70,70,128,255), //Link Mode Color
        new Color32(21,22,37, 255), //Link Mode BG Color
        new Color32(128,70,128,255), //Calc Mode Color
        new Color32(37,21,37,255), //Calc Mode BG Color

    };
    [SerializeField] private Button modeButton;
    [SerializeField] private TextMeshProUGUI modeText;
    [SerializeField] private Image modeBG;

    private Dictionary<string, string> statName = new Dictionary<string, string>
    {
        { "int","INT" },
        { "luk", "LUK" },
        { "astat", "All Stat" },
        {"matk","Magic ATT"},
        { "dmg", "Damage" },
        { "critd","Critical Damage"},
        { "nmd", "Normal Monster DMG" },
        {  "ied", "IED" },
        {"boss","Boss DMG"},
        { "exp", "EXP" },
        { "meso","Meso"},
        {"idr", "Drop Rate" },
        {"sac","Sacred Power"},
        { "buff","Buff Duration"},
        {"smn","Summon Duration"},
        {"abn","Abnormal Status DMG"},
        {"janus","Sol Janus"},
        {"sirius","Sirius Boost"},
        {"shine","Shine Boost"},
        {"sadal","Sadalsuud Boost"},
        {"savior","Savior's Circle Boost"},
        {"fom","Fruits of Mastery"},
        {"fdt","Fragment of Distorted Time"},
        {"siaM1","SHINE Ray/Antares"},
        {"siaO1","Celestial Design"}
    };

    private Dictionary<string, int> statNumber = new Dictionary<string, int>
    {
        { "int",0 },
        { "luk", 0 },
        { "astat", 0 },
        {"matk",0},
        { "dmg", 0 },
        { "critd",0},
        { "nmd", 0},
        {  "ied", 0},
        {"boss",0},
        { "exp", 0},
        { "meso",0},
        {"idr", 0},
        {"sac",0},
        { "buff",0},
        {"smn",0},
        {"abn",0},
        {"janus",0},
        {"sirius",0},
        {"shine",0},
        {"sadal",0},
        {"savior",0},
        {"fom",0},
        {"fdt",0},
        {"siaM1",0},
        {"siaO1",0}
    };
    [SerializeField] private GameObject calcModeBlocker;

    private void Awake()
    {
        //JsonConvert
        skillInformation = JsonConvert.DeserializeObject<Dictionary<string, SkillInfo>>(skillInformationJSON.text);
        //Debug.Log(skillInformation["rid1"].skillName);
    }
    void Start()
    {
        initialLockCheck();
        /*lockCheck();*/ //LOCK CHECK 1
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

    private void initialLockCheck()
    {
        bool lockFlag = false;
        foreach (string key in checkOrder)
        {
            //Check Only Non-Light Affected Nodes
            if (!lightAffectedNodes.Contains(key))
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
            }
            lockFlag = false;
        }
        computeLight();
        lightCheck();
    }

    public void lockCheck()
    {
        bool lockFlag = false;
        foreach (string key in checkOrder)
        {
            //Check Only Non-Light Affected Nodes
            if (!lightAffectedNodes.Contains(key))
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
            }
            lockFlag = false;
        }
        computeLight();
        lightCheck();
    }

    /*public void lockCheck()
    {
        //Check Normal Node Locks
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
        lightCheck();
        pc.pathCheck();
    }*/

    public void computeLight()
    {
        //Check Light Levels
        List<int> _tempHighest = new List<int> { 0, 0, 0, 0 };
        foreach (string key in checkOrder)
        {
            if (skills[key].isUnlocked() && skills[key].getSkillLevel() > 0)
            {
                List<int> _tempLL = skills[key].getLightLevel();
                if (_tempLL[0] > _tempHighest[0])
                {
                    _tempHighest[0] = _tempLL[0];
                }
                if (_tempLL[1] > _tempHighest[1])
                {
                    _tempHighest[1] = _tempLL[1];
                }
                if (_tempLL[2] > _tempHighest[2])
                {
                    _tempHighest[2] = _tempLL[2];
                }
                if (_tempLL[3] > _tempHighest[3])
                {
                    _tempHighest[3] = _tempLL[3];
                }
            }
        }
        currLightLevels = _tempHighest;
        drawLight();
    }
    public void lightCheck()
    {
        //Check Light Affected Nodes
        foreach (string key in lightAffectedNodes)
        {
            bool unlock = true;
            List<int> _tempLL = skills[key].getLightCondition();
            for (int i = 0; i < 4; i++)
            {
                if (_tempLL[i] > currLightLevels[i])
                {
                    unlock = false;
                    skills[key].setLock(true);
                }
            }
            if (unlock)
            {
                skills[key].setLock(false);
            }
            else
            {
                skills[key].setLock(true);
            }
        }
    }

    private void drawLight()
    {
        float _newWidth = 3.18f + (0.9f * (currLightLevels[1] + currLightLevels[3]));
        float _newHeight = 3.18f + (0.9f * (currLightLevels[0] + currLightLevels[2]));
        lightBoard.GetComponent<SpriteRenderer>().size = new UnityEngine.Vector2(_newWidth, _newHeight);
        //Compute New Center
        float newX = centerX;
        float newY = centerY;
        float xMov = 0.0f;
        float yMov = 0.0f;

        //Compute NewX

        if (currLightLevels[1] > currLightLevels[3])
        {
            //Use Right Equation
            xMov = currLightLevels[1] - currLightLevels[3];
            newX = (3.18f + (0.9f * xMov) + 13.46f) / 2.0f;
        }
        else
        {
            //Use LEFT
            xMov = currLightLevels[3] - currLightLevels[1];
            newX = (3.18f + (0.9f * xMov) - 19.82f) / -2.0f;
        }

        //Compute New Y
        if (currLightLevels[0] > currLightLevels[2])
        {
            //Use Up Equation
            yMov = currLightLevels[0] - currLightLevels[2];
            newY = (3.18f + (0.9f * yMov) - 14.83f) / 2.0f;
        }
        else
        {
            //Use Down
            yMov = currLightLevels[2] - currLightLevels[0];
            newY = (3.18f + (0.9f * yMov) + 8.47f) / -2.0f;

        }
        lightBoard.transform.position = new UnityEngine.Vector3(newX, newY, 0);
    }
    /*public void lockCheckOld()
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
    }*/

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
        //stoneType.sprite = stoneTypeImg[skillInformation[$"rid{skillId}"].nodeType];
        stoneTypeTextOut.text = $"<color={stoneTypeCC[skillInformation[$"rid{skillId}"].nodeType]}>{stoneTypeTxt[skillInformation[$"rid{skillId}"].nodeType]}</color>";
        populateTotals();
    }

    public void populateTotals()
    {
        computeTotalSpent();
        uiTotSE.text = $"{runningTotalSE}";
        uiTotSEF.text = $"{runningTotalSEF}";
        updateSummary();
    }

    private void computeTotalSpent()
    {
        runningTotalSE = 0;
        runningTotalSEF = 0;
        if (!calcMode)
        {
            foreach (KeyValuePair<string, RunestoneFunctionality> item in skills)
            {
                int _tempSkillId = item.Value.getSkillId();
                int _tempLevel = item.Value.getSkillLevel();
                if (_tempLevel > 0)
                {
                    for (int i = 0; i < _tempLevel; i++)
                    {
                        runningTotalSE += skillInformation[$"rid{_tempSkillId}"].solErdaCost[i];
                        runningTotalSEF += skillInformation[$"rid{_tempSkillId}"].solErdaFragCost[i];
                    }
                }
            }
        }
        else
        {
            foreach (KeyValuePair<string, RunestoneFunctionality> item in skills)
            {
                if (item.Value.isPartOfCalc())
                {
                    int _tempSkillId = item.Value.getSkillId();
                    List<int> calRange = item.Value.calcLv();
                    for (int i = calRange[0]; i < calRange[1]; i++)
                    {
                        runningTotalSE += skillInformation[$"rid{_tempSkillId}"].solErdaCost[i];
                        runningTotalSEF += skillInformation[$"rid{_tempSkillId}"].solErdaFragCost[i];
                    }
                }
            }
        }
    }

    private void updateSummary()
    {
        statNumber = new Dictionary<string, int>{
            { "int",0 },
            { "luk", 0 },
            { "astat", 0 },
            {"matk",0},
            { "dmg", 0 },
            { "critd",0},
            { "nmd", 0},
            {  "ied", 0},
            {"boss",0},
            { "exp", 0},
            { "meso",0},
            {"idr", 0},
            {"sac",0},
            { "buff",0},
            {"smn",0},
            {"abn",0},
            {"janus",0},
            {"sirius",0},
            {"shine",0},
            {"sadal",0},
            {"savior",0},
            {"fom",0},
            {"fdt",0},
            {"siaM1",0},
            {"siaO1",0}
        };

        if (!calcMode)
        {
            foreach (KeyValuePair<string, RunestoneFunctionality> item in skills)
            {
                if (skills[item.Key].isUnlocked() && skills[item.Key].getSkillLevel() > 0)
                {
                    if (skillInformation[$"rid{skills[item.Key].getSkillId()}"].nodeType <= 2)
                    {
                        statNumber[skillInformation[$"rid{skills[item.Key].getSkillId()}"].effectStr] += skillInformation[$"rid{skills[item.Key].getSkillId()}"].effectValue;
                    }
                    else if (skills[item.Key].getSkillId() <= 10000)
                    {
                        statNumber[skillInformation[$"rid{skills[item.Key].getSkillId()}"].effectStr] += skills[item.Key].getSkillLevel();
                    }
                }
            }
            string finale = "";
            foreach (KeyValuePair<string, string> header in statName)
            {
                if (header.Key != "janus" && header.Key != "sirius" && header.Key != "shine" && header.Key != "sadal" && header.Key != "savior" && header.Key != "siaM1" && header.Key != "siaO1" && header.Key != "fom" && header.Key != "fdt")
                {
                    if (statNumber[header.Key] > 0)
                    {
                        if (header.Key == "str" || header.Key == "dex" || header.Key == "int" || header.Key == "luk" || header.Key == "atk" || header.Key == "matk" || header.Key == "sac")
                        {
                            finale += $"{header.Value}:\t+{statNumber[header.Key]}\n";
                        }
                        else
                        {
                            finale += $"{header.Value}:\t+{statNumber[header.Key]}%\n";
                        }
                    }
                }
                else if (header.Key == "fom" || header.Key == "fdt")
                {
                    if (statNumber[header.Key] > 0)
                    {
                        finale += $"{header.Value}\n";
                    }
                }
                else
                {
                    if (statNumber[header.Key] > 0)
                    {
                        finale += $"{header.Value}:\tLv.{statNumber[header.Key]}\n";
                    }
                }
                summary.text = finale;
            }
        }
        else
        {
            Dictionary<string, int> pastVal = new Dictionary<string, int>
            {
                {"janus",0},
                {"sirius",0},
                {"shine",0},
                {"sadal",0},
                {"savior",0},
                {"fom",0},
                {"fdt",0},
                {"siaM1",0},
                {"siaO1",0}
            };
            foreach (KeyValuePair<string, RunestoneFunctionality> item in skills)
            {
                if (skills[item.Key].isPartOfCalc())
                {
                    if (skillInformation[$"rid{skills[item.Key].getSkillId()}"].nodeType <= 2)
                    {
                        statNumber[skillInformation[$"rid{skills[item.Key].getSkillId()}"].effectStr] += skillInformation[$"rid{skills[item.Key].getSkillId()}"].effectValue;
                    }
                    else if (skills[item.Key].getSkillId() <= 10000)
                    {
                        statNumber[skillInformation[$"rid{skills[item.Key].getSkillId()}"].effectStr] += skills[item.Key].calcLv()[1];
                        pastVal[skillInformation[$"rid{skills[item.Key].getSkillId()}"].effectStr] += skills[item.Key].calcLv()[0];
                    }
                }
            }
            string finale = "";
            foreach (KeyValuePair<string, string> header in statName)
            {
                if (header.Key != "janus" && header.Key != "sirius" && header.Key != "shine" && header.Key != "sadal" && header.Key != "savior" && header.Key != "siaM1" && header.Key != "siaO1" && header.Key != "fom" && header.Key != "fdt")
                {
                    if (statNumber[header.Key] > 0)
                    {
                        if (header.Key == "str" || header.Key == "dex" || header.Key == "int" || header.Key == "luk" || header.Key == "atk" || header.Key == "matk" || header.Key == "sac")
                        {
                            finale += $"{header.Value}:\t+{statNumber[header.Key]}\n";
                        }
                        else
                        {
                            finale += $"{header.Value}:\t+{statNumber[header.Key]}%\n";
                        }
                    }
                }
                else if (header.Key == "fom" || header.Key == "fdt")
                {
                    if (statNumber[header.Key] > 0)
                    {
                        finale += $"{header.Value}\n";
                    }
                }
                else
                {
                    if (statNumber[header.Key] > 0)
                    {
                        finale += $"{header.Value}:\tLv.{pastVal[header.Key]} > Lv.{statNumber[header.Key]}\n";
                    }
                }
                summary.text = finale;
            }
        }
    }

    public void toggleTooltip(bool state)
    {
        showTooltip = state;
    }

    public void swapMode()
    {
        if (calcMode)
        {
            modeText.text = "Link Mode";
            ColorBlock cb = modeButton.colors;
            cb.normalColor = bgModeColorsL[0];
            cb.selectedColor = bgModeColorsL[0];
            modeButton.colors = cb;
            modeBG.color = bgModeColorsL[1];
            calcModeBlocker.SetActive(false);
        }
        else
        {
            modeText.text = "Free Calc Mode";
            ColorBlock cb = modeButton.colors;
            cb.normalColor = bgModeColorsL[2];
            cb.selectedColor = bgModeColorsL[2];
            modeButton.colors = cb;
            modeBG.color = bgModeColorsL[3];
            calcModeBlocker.SetActive(true);
        }
        calcMode = !calcMode;
        swapLockGraphic();
        populateTotals();
        updateSummary();
    }

    private void swapLockGraphic()
    {
        foreach(string item in checkOrder)
        {
            if (calcMode)
            {
                skills[$"{item}"].toggleLockGraphic(false);
                skills[$"{item}"].toggleCalcGraphic(true, true);
            }
            else
            {
                skills[$"{item}"].toggleLockGraphic(!skills[$"{item}"].isUnlocked());
                skills[$"{item}"].toggleCalcGraphic(false, false);
            }

        }
        foreach (string item in lightAffectedNodes)
        {
            if (calcMode)
            {
                skills[$"{item}"].toggleLockGraphic(false);
                skills[$"{item}"].toggleCalcGraphic(true, true);
            }
            else
            {
                skills[$"{item}"].toggleLockGraphic(!skills[$"{item}"].isUnlocked());
                skills[$"{item}"].toggleCalcGraphic(false, false);
            }

        }

    }

}
