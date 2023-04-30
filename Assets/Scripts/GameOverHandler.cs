using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private AsteroidSpawner spawner;
    public void EndGame()
    {
        spawner.enabled = false;
        gameOverCanvas.SetActive(true);
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(1);
    }
    public void ContinueButton()
    {
        Debug.Log("ContinueButton");
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }


}
