using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static ApiManager;

public class ApiLoginer : MonoBehaviour
{
    public string bearerToken;
    private const string LoginUrl = "https://default-ios-backend-49037e2d368a.herokuapp.com/login";
    private const string Username = "admin";
    private const string Password = "Rd3LEiq8G2gg";
    private const string CreatePlayerUrl = "https://default-ios-backend-49037e2d368a.herokuapp.com/api/players";

    public void LoginInApi()
    {
        StartCoroutine(Login());
    }
    private IEnumerator Login()
    {
        string credentials = $"{Username}:{Password}";
        string base64Credentials = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(LoginUrl, ""))
        {
            request.SetRequestHeader("Authorization", "Basic " + base64Credentials);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login response: " + request.downloadHandler.text);
                var response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                bearerToken = response.token;
                if (IsFirstRun())
                {
                    Debug.Log("Это первый запуск игры.");

                    StartCoroutine(CreatePlayer("username", 0, "base"));
                }
                else
                {
                    Debug.Log("Это не первый запуск игры.");

                }

            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    private IEnumerator CreatePlayer(string name, int score, string imageURL)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);
        form.AddField("imageURL", imageURL);

        using (UnityWebRequest request = UnityWebRequest.Post(CreatePlayerUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + bearerToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Create player response: " + request.downloadHandler.text);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);

                SaveManager.PlayerPrefs.SaveInt(GameKeys.PlayerID, playerData.id);
                SaveManager.Resources.SaveResource(ResourceTypes.Points, 0);
                Debug.Log("ID: " + playerData.id);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    private bool IsFirstRun()
    {
        if (!PlayerPrefs.HasKey("FirstGameRunKey"))
        {
            PlayerPrefs.SetInt("FirstGameRunKey", 1);
            return true;
        }
        else
        {
            return false;
        }
    }
}
