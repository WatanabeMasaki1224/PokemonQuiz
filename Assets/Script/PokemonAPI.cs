using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Audio.ProcessorInstance;

public class PokemonAPI : MonoBehaviour
{
    [SerializeField] private Image _pokemonImage;
    [SerializeField] private PokemonQuizManager _quizManager;
    //API
    string urlAPI = "https://pokeapi.co/api/v2/pokemon/3";

    [Serializable]
    public class PokemonQuizData
    {
        public string name;
        public string imageURL;
        public string[] types;
    }

    [Serializable]
    public class ResponseData
    {
        public string name;
        public Sprites sprites;
        public TypeData[] types;
    }

    [Serializable]
    public class Sprites
    {
        public string front_default;
    }

    [Serializable]
    public class TypeData
    {
        public TypeInfo type;
    }

    [Serializable]
    public class TypeInfo
    {
        public string name;
    }

    public void OnClickGetPokemon()
    {
        Debug.Log("おされた");
        StartCoroutine(GetAPI());
    }

    IEnumerator GetAPI()
    {
        UnityWebRequest request  = UnityWebRequest.Get(urlAPI);
        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");
                Debug.Log(request.downloadHandler.text);
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
                //APIデータ　→　クイズ用データーに変換
                PokemonQuizData quizData = new PokemonQuizData();
                quizData.name = responseData.name;
                quizData.imageURL = responseData.sprites.front_default;
                quizData.types = new string[responseData.types.Length];
                for(int i = 0; i < responseData.types.Length; i++)
                {
                    quizData.types[i] = responseData.types[i].type.name;
                }
                //確認
                Debug.Log("名前：" + quizData.name);
                for (int i = 0; i < quizData.types.Length; i++)
                {
                    Debug.Log("タイプ：" + quizData.types[i]);
                }
                //4択を作る
                _quizManager.CreateChoices(quizData);
                //画像所得
                StartCoroutine(GetTexture(quizData.imageURL));
                break;

            default:
                Debug.Log("エラー；" + request.error);
                break;
        }
        request.Dispose();
    }

    IEnumerator GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            _pokemonImage.sprite =
                Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(1f, 1f)
                    );
        }
        request.Dispose();
    }
}
