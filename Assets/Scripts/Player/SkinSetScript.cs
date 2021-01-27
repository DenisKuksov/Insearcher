using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSetScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private Sprite[] _skins;

    private void Start()
    {
        _playerSprite.sprite = _skins[PlayerPrefs.GetInt("SelectedSkinID", 0)];
    }

}
