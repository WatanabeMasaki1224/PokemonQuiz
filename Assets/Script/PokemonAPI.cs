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

    /// <summary>
    /// APIから所得したデータを、使いやすい形にしたデータ
    /// </summary>
    [Serializable]
    public class PokemonQuizData
    {
        public string name;
        public string imageURL;
        public string[] types;
    }

    /// <summary>
    /// PokeAPIから受け取ったJSONを一時的に受け取るためのクラス
    /// </summary>
    [Serializable]
    public class ResponseData
    {
        public string name;
        public Sprites sprites;
        public TypeData[] types;
    }

    /// <summary>
    /// 画像の所得
    /// </summary>
    [Serializable]
    public class Sprites
    {
        public string front_default;
    }

    /// <summary>
    /// タイプ情報を受け取る
    /// </summary>
    [Serializable]
    public class TypeData
    {
        public TypeInfo type;
    }

    /// <summary>
    /// タイプの名前を受け取る
    /// </summary>
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

    /// <summary>
    ///　APIからデータ所得
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAPI()
    {
        int randomID = UnityEngine.Random.Range(1, 1026);
        //API
        string urlAPI = "https://pokeapi.co/api/v2/pokemon/" + randomID;
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

    /// <summary>
    /// 画像を所得
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            //ダウンロードした画像を Texture2Dで保存
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            //spriteに変換してimageに表示
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
