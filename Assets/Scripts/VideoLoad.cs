using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class VideoLoad : MonoBehaviour
{
    private string serverUrl = "http://10.188.66.73:3000";
    public TMP_InputField usernameField ;
    public TMP_InputField passwordField ;
    private string username;
    private string password;
    private string token;
    private string savePath;

    public void StartLogin()
    {
        username = usernameField.text;
        password = passwordField.text;
        // Définir le chemin de sauvegarde des vidéos
        savePath = Path.Combine(Application.persistentDataPath, "media", username);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            Debug.Log("Dossier créé : " + savePath);
        }

        // Lancer le processus de connexion et récupération des vidéos
        StartCoroutine(LoginAndFetchVideos());
        
    }

    IEnumerator LoginAndFetchVideos()
    {
        yield return StartCoroutine(Login());
        if (!string.IsNullOrEmpty(token))
        {
            yield return StartCoroutine(GetVideos());
            
        }
        else
        {
            Debug.LogError("Impossible de récupérer les vidéos, token manquant !");
        }
        
    }

    IEnumerator Login()
    {
        string loginUrl = serverUrl + "/login";
        string jsonBody = "{\"username\":\"" + username + "\", \"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 45;
        Debug.Log("Tentative de connexion...");
        yield return request.SendWebRequest();
        Debug.Log("connecté");
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Réponse du serveur : " + jsonResponse);

            // Extraire le token du JSON
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);
            if (!string.IsNullOrEmpty(response.token))
            {
                token = response.token;
                PlayerPrefs.SetString("token", token);
                PlayerPrefs.SetString("username", username);
                PlayerPrefs.Save();

                Debug.Log("Token JWT sauvegardé : " + token);
            }
            else
            {
                Debug.LogError("Erreur : Aucun token reçu !");
            }
        }
        else
        {
            Debug.LogError("Erreur de connexion : " + request.error);
        }
    }

    IEnumerator GetVideos()
    {
        string videoUrl = serverUrl + "/videos";
        UnityWebRequest request = UnityWebRequest.Get(videoUrl);
        request.SetRequestHeader("Authorization", "Bearer " + token);

        Debug.Log("Récupération des vidéos...");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Liste des vidéos récupérées : " + jsonResponse);

            // Extraire les URLs des vidéos et les télécharger
            VideoListResponse response = JsonUtility.FromJson<VideoListResponse>("{\"videos\":" + jsonResponse + "}");
            foreach (string videoPath in response.videos)
            {
                string fullUrl = serverUrl + videoPath;
                string fileName = Path.GetFileName(videoPath);
                StartCoroutine(DownloadVideo(fullUrl, fileName));
            }
           
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des vidéos : " + request.error);
        }
    }

    IEnumerator DownloadVideo(string url, string fileName)
    {
        string localPath = Path.Combine(savePath, fileName);
        UnityWebRequest request = UnityWebRequest.Get(url);

        Debug.Log("Téléchargement de " + fileName + "...");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            File.WriteAllBytes(localPath, request.downloadHandler.data);
            Debug.Log("Vidéo téléchargée : " + localPath);
        }
        else
        {
            Debug.LogError("Échec du téléchargement de " + fileName + " : " + request.error);
        }
    }
}

// Définition des classes pour la désérialisation JSON
[System.Serializable]
public class LoginResponse
{
    public string token;
}

[System.Serializable]
public class VideoListResponse
{
    public string[] videos;
}
