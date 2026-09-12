using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private GameObject _endPanel;
    

    public void StartGame()
    {
        _startPanel.SetActive(false);
    }

    public void ClearGame()
    {
        _clearPanel.SetActive(false);
    }

    public void EndGame()
    {
        _endPanel.SetActive(false);
    }
}
