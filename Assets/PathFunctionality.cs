using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class PathFunctionality : MonoBehaviour
{
    [SerializeField] private PathController controller;
    [SerializeField] private string identifier;
    [SerializeField] private List<int> andUnlockCondition = new List<int> { };
    [SerializeField] private SpriteRenderer spr;
    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Main Control").GetComponent<PathController>();
        controller.subscribeToController(this, identifier);

    }

    public List<int> getAndCondition()
    {
        return andUnlockCondition;
    }

    public void drawPath(bool draw)
    {
            spr.enabled = draw;
    }

}
