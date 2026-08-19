using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PokemonQuizUI : MonoBehaviour
{
    [SerializeField] private Button[] _choiceButtons;
    private bool[] _selected;

    private void Start()
    {
        _selected = new bool[4];
    }


    public void SetChoice(string[] choice)
    {
        for(int i = 0; i < choice.Length; i++)
        {
            TMP_Text text = _choiceButtons[i].GetComponentInChildren<TMP_Text>();
            text.text = choice[i];
            _choiceButtons[i].gameObject.SetActive(true);
            _selected[i] = false;
            _choiceButtons[i].image.color = Color.white;
            int index = i;
            _choiceButtons[i].onClick.RemoveAllListeners();
            _choiceButtons[i].onClick.AddListener(() => SelectChoise(index));
        }
    }

    void SelectChoise(int index)
    {
        _selected[index] = !_selected[index];

        if (_selected[index])
        {
            _choiceButtons[index].image.color = new Color(0.7f, 0.7f, 0.7f);
        }
        else
        {
            _choiceButtons[index].image.color = Color.white;
        }
    }

    public string[] GetSelectedTypes()
    {
        List<string> selectedTypes = new List<string>();

        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            if (_selected[i])
            {
                TMP_Text text =
                    _choiceButtons[i].GetComponentInChildren<TMP_Text>();

                selectedTypes.Add(text.text);
            }
        }

        return selectedTypes.ToArray();
    }
}
