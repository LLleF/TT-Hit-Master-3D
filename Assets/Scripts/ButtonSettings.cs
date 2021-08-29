using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSettings : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
