using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyLoader : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadMultiplayer);
    }

    private void LoadMultiplayer()
    {
        SceneManager.LoadScene("MyLobbyScene");
    }
}
