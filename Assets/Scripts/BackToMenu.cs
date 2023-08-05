using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class BackToMenu : MonoBehaviour
    {
        public void MapLevel()
        {
            SceneManager.LoadScene(1);
        }
        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}