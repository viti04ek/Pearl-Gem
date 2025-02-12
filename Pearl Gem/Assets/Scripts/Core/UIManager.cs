using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Top panel")]
    [SerializeField] private Text _pearlsText;
    [SerializeField] private Text _shotsText;
    [SerializeField] private List<Image> _starBars;
    [SerializeField] private Image _starImg;
    [SerializeField] private Sprite _fullStarBar;
    [SerializeField] private Sprite _emptyStarBar;
    [SerializeField] private Sprite _fullStar;
    
    [Header("Settings panel")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _checkSoundImg;
    [SerializeField] private GameObject _checkVibrationImg;
    
    [Header("Down panel")]
    [SerializeField] private Image _currentBallImg;
    [SerializeField] private Image _nextBallImg;
    [SerializeField] private Text _shotsOnBallText;

    [Header("Win panel")] 
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _winPanelCoinText;

    [Header("Lose panels")] 
    [SerializeField] private GameObject _losePanel1;
    [SerializeField] private Text _losePanel1CoinText;
    [SerializeField] private GameObject _losePanel2;
    [SerializeField] private Text _losePanel2CoinText;

    private AimController _aimController;
    private const float _tweenDuration = 0.7f;

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
        _settingsPanel.GetComponent<RectTransform>().DOAnchorPosX(0, _tweenDuration);
    }

    public void CloseSettings()
    {
        _settingsPanel.GetComponent<RectTransform>().DOAnchorPosX(-Screen.width, _tweenDuration);
        Invoke(nameof(HideSettingsPanel), _tweenDuration);
    }

    private void HideSettingsPanel()
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

    public void ShowWinPanel()
    {
        _winPanel.SetActive(true);
        _levelText.text = $"Level {Services.GameManager.Level} completed!";
        _winPanelCoinText.text = Services.GameManager.Coins.ToString();
        _winPanel.GetComponent<RectTransform>().DOAnchorPosX(0, _tweenDuration);
    }

    public void ShowLosePanel1()
    {
        _losePanel1.SetActive(true);
        _losePanel1CoinText.text = Services.GameManager.Coins.ToString();
        _losePanel1.GetComponent<RectTransform>().DOAnchorPosX(0, _tweenDuration);
    }

    public void ShowLosePanel2()
    {
        _losePanel1.SetActive(false);
        _losePanel2.SetActive(true);
        _losePanel2CoinText.text = Services.GameManager.Coins.ToString();
        _losePanel2.GetComponent<RectTransform>().DOAnchorPosX(0, _tweenDuration);
    }
}
