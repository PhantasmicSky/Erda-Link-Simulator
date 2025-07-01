using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class calcModeFunctions : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] RunestoneFunctionality parent;
    [SerializeField] GameObject menuPrefab;
    [SerializeField] GameObject spawnedPrefab;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] TextMeshPro levtext;

    private void Awake()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("Canvas");
        if (parent.getMaxLevel() == 1)
        {
            levtext.text = "";
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (parent.getMaxLevel() > 1)
        {
            spawnedPrefab = Instantiate(menuPrefab, mainCanvas.transform);
            spawnedPrefab.GetComponent<changeBoxFx>().initializeBox(parent.getMaxLevel(), parent.getPhoto(), this);
        }
    }

    public void passFunction(int low, int high)
    {
        Destroy(spawnedPrefab);
        parent.changeCalcRange(low, high);
    }
}
