using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class LobbyMainPanel : MonoBehaviour
    {
        [Header("Login Panel")]
        public GameObject loginPanel;

        public InputField playerNameInput;

        public void Awake()
        {
            playerNameInput.text = "Player " + Random.Range(1000, 10000);
        }
   
        public void SetActivePanel(string activePanel)
        {
            loginPanel.SetActive(activePanel.Equals(loginPanel.name));
        }
    }
}