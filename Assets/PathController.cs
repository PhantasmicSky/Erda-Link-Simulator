using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private Dictionary<string, PathFunctionality> paths = new Dictionary<string, PathFunctionality> { };
    [SerializeField] private SkillController sc;
    public void subscribeToController(PathFunctionality pf, string loc)
    {
        paths.Add(loc, pf);
    }

    public void pathCheck()
    {
        foreach (KeyValuePair<string, PathFunctionality> item in paths)
        {
            bool toDrawPath = true;
            List<int> _tempCondition = item.Value.getAndCondition();
            foreach (int condition in _tempCondition)
            {
                if (!sc.skills[$"rs{condition}"].isUnlocked() || sc.skills[$"rs{condition}"].getSkillLevel() == 0)
                {
                    toDrawPath = false;
                    break;
                }
            }
            item.Value.drawPath(toDrawPath);
        }
    }
}
