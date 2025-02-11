using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _pearlsText;
    [SerializeField] private Text _shotsText;

    public void UpdatePearlsText(int pearls)
    {
        _pearlsText.text = pearls.ToString();
    }

    public void UpdateShotsText(int shots)
    {
        _shotsText.text = shots.ToString();
    }
}
