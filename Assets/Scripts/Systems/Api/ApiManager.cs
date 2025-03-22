using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
   
    [SerializeField] private PlayerList _playerList;
    [SerializeField] private ApiLoginer _apiLoginer;
    // Get Players
    private const string PlayersUrl = "https://default-ios-backend-49037e2d368a.herokuapp.com/api/players";

    // Create Player
    private const string CreatePlayerUrl = "https://default-ios-backend-49037e2d368a.herokuapp.com/api/players";
    // Update Player
    private const string UpdatePlayerUrl = "https://default-ios-backend-49037e2d368a.herokuapp.com/api/players";

    // Delete Player
    private const string DeletePlayerUrl = "https://default-ios-backend-49037e2d368a.herokuapp.com/api/players";


    private void Start()
    {
        _apiLoginer.LoginInApi();
       
    }
    [System.Serializable]
    public class LoginResponse
    {
        public string token;
    }

    public void TokenReceived(string token)
    {
        _apiLoginer.bearerToken = token;
        StartCoroutine(CreatePlayer("username", 0, "base"));
    }
    public async Task<PlayerList> GetPlayerList()
    {
        await GetPlayers();
        return _playerList;
    }

    public void ChangeName(string newName)
    {
        
        
        StartCoroutine(UpdatePlayer(SaveManager.PlayerPrefs.LoadInt(GameKeys.PlayerID), newName, SaveManager.Resources.LoadResource(ResourceTypes.Points), "base"));
    }

    public void ChangeScore(int newScore)
    {
        StartCoroutine(UpdatePlayer(SaveManager.PlayerPrefs.LoadInt(GameKeys.PlayerID), SaveManager.PlayerPrefs.LoadString(GameKeys.Name), newScore, "base"));
    }



    private async Task GetPlayers()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(PlayersUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + _apiLoginer.bearerToken);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Players response: " + request.downloadHandler.text);
                _playerList = JsonUtility.FromJson<PlayerList>("{\"players\":" + request.downloadHandler.text + "}");
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
            request.SetRequestHeader("Authorization", "Bearer " + _apiLoginer.bearerToken);
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

    private IEnumerator UpdatePlayer(int playerId, string name, int score, string imageURL)
    {
        string url = $"{UpdatePlayerUrl}/{playerId}";

        using (UnityWebRequest request = new UnityWebRequest(url, "PATCH"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + _apiLoginer.bearerToken);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            // Создаем содержимое запроса
            WWWForm form = new WWWForm();
            form.AddField("name", name);
            form.AddField("score", score.ToString());
            form.AddField("imageURL", imageURL);

            // Устанавливаем содержимое запроса
            request.uploadHandler = new UploadHandlerRaw(form.data);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Update player response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }

    }
}
