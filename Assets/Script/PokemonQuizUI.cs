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
    private string[] _choiceTypes;

    private Dictionary<string,string> _typeJapanese = new Dictionary<string,string>()
    {
        { "normal", "ノーマル" },
        { "fire", "ほのお" },
        { "water", "みず" },
        { "electric", "でんき" },
        { "grass", "くさ" },
        { "ice", "こおり" },
        { "fighting", "かくとう" },
        { "poison", "どく" },
        { "ground", "じめん" },
        { "flying", "ひこう" },
        { "psychic", "エスパー" },
        { "bug", "むし" },
        { "rock", "いわ" },
        { "ghost", "ゴースト" },
        { "dragon", "ドラゴン" },
        { "dark", "あく" },
        { "steel", "はがね" },
        { "fairy", "フェアリー" }
    };

private void Start()
    {
        _selected = new bool[4];
    }

    /// <summary>
    /// クイズの４択ボタンに選択肢を表示
    /// </summary>
    /// <param name="choice"></param>
    public void SetChoice(string[] choice)
    {
        //答え合わせで使うため英語も保存しておく
        _choiceTypes = choice;

        for (int i = 0; i < choice.Length; i++)
        {
            TMP_Text text = _choiceButtons[i].GetComponentInChildren<TMP_Text>();
            // 英語のタイプ名を日本語に変換して表示
            text.text = _typeJapanese[choice[i]];
            _choiceButtons[i].gameObject.SetActive(true);
            // 新しい問題なので、選択状態を解除する
            _selected[i] = false;
            _choiceButtons[i].image.color = Color.white;
            int index = i;
            // 前の問題で登録したクリック処理を削除する
            _choiceButtons[i].onClick.RemoveAllListeners();
            _choiceButtons[i].onClick.AddListener(() => SelectChoise(index));
        }
    }

    /// <summary>
    /// 選択肢を押したときの処理
    /// </summary>
    /// <param name="index"></param>
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

    /// <summary>
    /// 選択したタイプを受け取る
    /// </summary>
    /// <returns></returns>
    public string[] GetSelectedTypes()
    {
        List<string> selectedTypes = new List<string>();

        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            if (_selected[i])
            {
                selectedTypes.Add(_choiceTypes[i]);
            }
        }

        return selectedTypes.ToArray();
    }
}
