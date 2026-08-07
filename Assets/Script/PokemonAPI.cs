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
    //API
    string urlAPI = "https://pokeapi.co/api/v2/pokemon/225";

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
                Debug.Log(" ポケモン名" + responseData.name);
                StartCoroutine(GetTexture(responseData.sprites.front_default));
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
