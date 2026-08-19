using System.Collections.Generic;
using UnityEngine;

public class PokemonQuizManager : MonoBehaviour
{
    [SerializeField] private PokemonQuizUI _quizUI;
    private PokemonAPI.PokemonQuizData _currentQuizData;

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

   
}
