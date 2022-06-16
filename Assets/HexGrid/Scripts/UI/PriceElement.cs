using UnityEngine;
using UnityEngine.UI;

public class PriceElement : MonoBehaviour
{
    [SerializeField]
    private Text _priceText;
    [SerializeField]
    private Image _resourceImage;

    public void SetUpElement(ResourceType resourceType, int amount)
    {
        _resourceImage.sprite = Resources.Load<Sprite>(resourceType.ToString());
        _priceText.text = amount.ToString();
    }
}