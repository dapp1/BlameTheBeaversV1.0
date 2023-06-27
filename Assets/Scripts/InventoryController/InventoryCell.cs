using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _durabilityBar;
    [SerializeField] private Button _buyBtn;
    [SerializeField] private Animator _buyBtnAnim;
    [SerializeField] private Image _buyActivationBar;
    [SerializeField] private InventoryItemType _itemType;
    
    public void Start()
    {
        if (_buyBtn != null)
        {
            _buyBtn.onClick.AddListener(() =>
            {
                if (!InventoryController.Instance.BuyItem(_itemType))
                    return;
                
                _buyBtn.interactable = false;
                _buyBtnAnim.SetBool("IsInteractable", false);

                Tween.Value(0f, 1f, (value) =>
                {
                    _buyActivationBar.fillAmount = 1 - value;
                }, 5, 0, completeCallback: () =>
                {
                    _buyBtn.interactable = true;
                    _buyBtnAnim.SetBool("IsInteractable", true);
                    InventoryController.Instance.DeliveryItem(_itemType);
                });
            });
        }
    }
    
    public void Select(bool value)
    {
        if (value)
            _icon.transform.localScale = new Vector3(1.2f, 1.2f);
        else
            _icon.transform.localScale = new Vector3(0.7f, 0.7f);
    }

    public void SetActive(bool value, InventoryItem settings)
    {
        if (!value)
            _icon.sprite = settings.UIIconUnavailable;
        else
            _icon.sprite = settings.UIIcon;

        Select(false);
    }

    public void SetDurability(float durabilityPercent)
    {
        _durabilityBar.fillAmount = durabilityPercent;
    }
}
