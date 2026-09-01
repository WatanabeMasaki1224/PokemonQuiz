using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonQuizManager : MonoBehaviour
{
    [SerializeField] private PokemonQuizUI _quizUI;
    [SerializeField] private GameObject _correctPanel;
    [SerializeField] private GameObject _incorrectPanel;
    private PokemonAPI.PokemonQuizData _currentQuizData;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private PokemonAPI _pokemonAPI;
    [SerializeField] private TMPro.TMP_Text _scoreText;

    private string[] allType =
    {
        "normal",
        "fire",
        "water",
        "electric",
        "grass",
        "ice",
        "fighting",
        "poison",
        "ground",
        "flying",
        "psychic",
        "bug",
        "rock",
        "ghost",
        "dragon",
        "dark",
        "steel",
        "fairy"
    };

    public void CreateChoices(PokemonAPI.PokemonQuizData quizData)
    {
        _currentQuizData = quizData;
        List<string> choices = new List<string>();

        //正解タイプを入れる
        for(int i = 0; i < quizData.types.Length; i++)
        {
            choices.Add(quizData.types[i]);
        }
        //４個になるまでランダムに追加
        while (choices.Count < 4 )
        {
            string randoumType = allType[Random.Range(0, allType.Length)];
            //すでに入ってるタイプなら追加しない
            if(!choices.Contains(randoumType))
            {
                choices.Add(randoumType);
            }
        }

        //４択をシャッフルする
        for(int i = 0;i < choices.Count;i++)
        {
            int randomIndex = Random.Range(i, choices.Count);
            string temp = choices[i];
            choices[i] = choices[randomIndex];
            choices[randomIndex] = temp;
        }

        _quizUI.SetChoice(choices.ToArray());

        for(int i = 0; i < choices.Count;i++)
        {
            Debug.Log((i + 1) + ":" + choices[i]);
        }
    }

    public void CheckAnswer()
    {
        string[] selectedTypes = _quizUI.GetSelectedTypes();
        // 正解タイプと数が違えば不正解
        if (selectedTypes.Length != _currentQuizData.types.Length)
        {
            ShowIncorrect();
            Debug.Log("ふ正解");
            return;
        }

        // 選択したタイプが全部正解タイプに含まれているか確認
        for (int i = 0; i < selectedTypes.Length; i++)
        {
            if (!_currentQuizData.types.Contains(selectedTypes[i]))
            {
                ShowIncorrect() ;
                Debug.Log("ふ正解");
                return;
            }
        }
        ShowCorrect();
        Debug.Log("正解");
    }
    
    public void ShowCorrect()
    {
        _correctPanel.SetActive(true);
        _scoreManager.AddScore();
    }

    public void ShowIncorrect()
    {
        _incorrectPanel.SetActive(true);
        _scoreText.text = "スコア：" + _scoreManager.GetScore();
    }

    public void  NextQuestion()
    {
        _correctPanel.SetActive(false);
        _incorrectPanel.SetActive(false );
        _pokemonAPI.OnClickGetPokemon();
    }
}
