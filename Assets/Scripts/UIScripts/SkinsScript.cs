using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsScript : MonoBehaviour
{
    [System.Serializable] private struct SkinButton
    {
        public int price;
        public GameObject priceText;
        public GameObject skinImage;
        public Button skinButton;
    }

    [SerializeField] private SkinButton[] _skinButtons;
    private SkinButton _prevSkin;


    public void InitSkins()
    {
        string opened = PlayerPrefs.GetString("SkinsOpened", "1000");
        _prevSkin = _skinButtons[PlayerPrefs.GetInt("SelectedSkinID", 0)];
        _prevSkin.skinButton.interactable = false;

        for (int i = 0; i < _skinButtons.Length; i++)
        {
            if(opened[i] == '1')
            {
                _skinButtons[i].priceText.SetActive(false);
                _skinButtons[i].skinImage.SetActive(true);
            }
            else
            {
                _skinButtons[i].priceText.SetActive(true);
                _skinButtons[i].skinImage.SetActive(false);
            }
        }
    }

    public void PressSkinButton(int id)
    {
        SkinButton sb = _skinButtons[id];
        if (sb.skinImage.activeSelf)
        {
            _prevSkin.skinButton.interactable = true;
            sb.skinButton.interactable = false;
            PlayerPrefs.SetInt("SelectedSkinID", id);
            _prevSkin = sb;
        }
        else
        {
            int coins = PlayerPrefs.GetInt("Coins", 0);
            PlayerPrefs.SetInt("Coins", 54);
            Debug.Log(PlayerPrefs.GetInt("Coins", 0));
            if (sb.price <= coins)
            {
                sb.priceText.SetActive(false);
                sb.skinImage.SetActive(true);
                coins -= sb.price;
                PlayerPrefs.SetInt("Coins", coins);
                _prevSkin.skinButton.interactable = true;
                sb.skinButton.interactable = false;
                PlayerPrefs.SetInt("SelectedSkinID", id);
                _prevSkin = sb;

                System.Text.StringBuilder opened = new System.Text.StringBuilder(PlayerPrefs.GetString("SkinsOpened", "1000"));
                opened[id] = '1';
                PlayerPrefs.SetString("SkinsOpened", opened.ToString());
            }
        }
    }

}
