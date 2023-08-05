using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace TowerDefense 
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private GameObject newGameConfirmMenu;
        private void Start()
        {
            continueButton.enabled = FileHandler.HasFile(MapCompletion.filename);
        }
        public void NewGame()
        {
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);            
            SceneManager.LoadScene(1);

        }
        public void Continue()
        {
            SceneManager.LoadScene(1);
        }
        public void Quit()
        {
            Application.Quit();
        }
        public void ResetGame()
        {
            FileHandler.Reset(MapCompletion.filename);            
        }
        public void ConfirmMenu()
        {
            if (FileHandler.HasFile(MapCompletion.filename))
            {
                newGameConfirmMenu.SetActive(true);
            }
            else
            {
                NewGame();
            }
        }
    }
}