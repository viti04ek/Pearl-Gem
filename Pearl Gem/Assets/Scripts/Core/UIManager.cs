using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _pearlsText;
    [SerializeField] private Text _shotsText;
    [SerializeField] private List<Image> _starBars;
    [SerializeField] private Image _starImg;
    [SerializeField] private Sprite _fullStarBar;
    [SerializeField] private Sprite _emptyStarBar;
    [SerializeField] private Sprite _fullStar;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _checkSoundImg;
    [SerializeField] private GameObject _checkVibrationImg;
    [SerializeField] private Image _currentBallImg;
    [SerializeField] private Image _nextBallImg;
    [SerializeField] private Text _shotsOnBallText;

    private AimController _aimController;

    public void FindAimController()
    {
        _aimController = FindObjectOfType<AimController>();
        var screenPos = Camera.main.WorldToScreenPoint(_aimController.gameObject.transform.position);
        _currentBallImg.rectTransform.position = screenPos;
    }

    public void UpdatePearlsText(int pearls)
    {
        _pearlsText.text = pearls.ToString();
    }

    public void UpdateShotsText(int shots)
    {
        _shotsText.text = shots.ToString();
        _shotsOnBallText.text = shots.ToString();
    }

    public void AddStarBar(int index)
    {
        _starBars[index].sprite = _fullStarBar;
    }

    public void ResetStarBars()
    {
        foreach (var bar in _starBars)
        {
            bar.sprite = _emptyStarBar;
        }
    }

    public void AddFullStar()
    {
        _starImg.sprite = _fullStar;
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void CheckSound()
    {
        _checkSoundImg.SetActive(!_checkSoundImg.activeSelf);
    }

    public void CheckVibration()
    {
        _checkVibrationImg.SetActive(!_checkVibrationImg.activeSelf);
    }

    public void UpdateBallsColor()
    {
        var newColor = _aimController.BallColor;
        newColor.a = 255;
        _currentBallImg.color = newColor;
        
        newColor = _aimController.NextBallColor;
        newColor.a = 255;
        _nextBallImg.color = newColor;
    }

    public void ChangeBallColor()
    {
        _aimController.ChangeColor();
        UpdateBallsColor();
    }
}
