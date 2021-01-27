using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Skin[] _skins;
    [SerializeField] private Text _coinsText;
    
    [System.Serializable] struct SkinButton
    {
        public Image image;
        public Text priceText;
        public Button button;
        public int price;
    }

    [SerializeField] private SkinButton[] _skinButtons;

    private int _previousSkin;

    public void OpenSkinsMenu()
    {
        int currentSkin = PlayerPrefs.GetInt("CurrentSkin", 0);
        _skinButtons[currentSkin].button.interactable = false;
        _previousSkin = currentSkin;

        string unlockedSkins = PlayerPrefs.GetString("UnlockedSkins", "1000");
        for (int i = 0; i < unlockedSkins.Length; i++)
        {
            if (unlockedSkins[i] == '1')
            {
                _skinButtons[i].image.gameObject.SetActive(true);
                _skinButtons[i].image.gameObject.SetActive(false);
            }
            else
            {
                _skinButtons[i].image.gameObject.SetActive(false);
                _skinButtons[i].image.gameObject.SetActive(true);
            }
        }
    }

    public void SkinButtonPress(int index)
    {
        string unlockedSkins = PlayerPrefs.GetString("UnlockedSkins", "1000");
        int coins = PlayerPrefs.GetInt("Coins", 0);

        if (unlockedSkins[index] == '1')
        {
            _skinButtons[_previousSkin].button.interactable = true;
            PlayerPrefs.SetInt("CurrentSkin", index);
            _skinButtons[index].button.interactable = false;
        }
        else if (coins >= _skinButtons[index].price)
        {
            PlayerPrefs.SetInt("Coins", coins - _skinButtons[index].price);

            _skinButtons[_previousSkin].button.interactable = true;
            PlayerPrefs.SetInt("CurrentSkin", index);
            _skinButtons[index].button.interactable = false;

        }
    }

}
