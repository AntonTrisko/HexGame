using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PricesDropdown : MonoBehaviour
{
    public Button buildButton;
    [SerializeField]
    private Transform _requirementsLayout;
    [SerializeField]
    private Transform _productionLayout;
    private bool _isReady;

    public void SetUp(Dictionary<ResourceType, int> price, Dictionary<ResourceType, int> production)
    {
        if (!_isReady)
        {
            if (price != null)
            {
                WindowManager.Instance.SetUpLayout(price, _requirementsLayout, 1);
            }
            if (production != null)
            {
                WindowManager.Instance.SetUpLayout(production, _productionLayout, 1);
            }
        }
        _isReady = true;
    }
}