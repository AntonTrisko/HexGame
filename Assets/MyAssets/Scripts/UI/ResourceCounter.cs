using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    private ResourceType _productionType;
    [SerializeField]
    private Text _countText;
    [SerializeField]
    private Text _incomeText;

    private void Start()
    {
        SetCounterText();
        EventManager.UpdateResources += SetCounterText;
    }

    private void SetCounterText()
    {
        _countText.text = CellsManager.Instance.GetProductionAmount(_productionType,PlayerManager.Instance.currentPlayer).ToString();
    }

    private void OnDestroy()
    {
        EventManager.UpdateResources -= SetCounterText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _incomeText.gameObject.SetActive(true);
        ShowIncome();
    }

    private void ShowIncome()
    {
        int income = CellsManager.Instance.GetProductionIncome(_productionType, PlayerManager.Instance.currentPlayer);
        if (income > 0)
        {
            _incomeText.color = Color.green;
        }
        else
        {
            _incomeText.color = Color.red;
        }
        _incomeText.text = income.ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _incomeText.gameObject.SetActive(false);
    }

}