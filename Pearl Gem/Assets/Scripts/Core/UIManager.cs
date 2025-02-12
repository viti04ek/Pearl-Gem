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

    public void UpdatePearlsText(int pearls)
    {
        _pearlsText.text = pearls.ToString();
    }

    public void UpdateShotsText(int shots)
    {
        _shotsText.text = shots.ToString();
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
}
