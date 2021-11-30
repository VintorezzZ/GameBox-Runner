using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleplayerLoader : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadSingleplayer);
    }

    private void LoadSingleplayer()
    {
        SceneManager.LoadScene("Gameplay");
    }

}
