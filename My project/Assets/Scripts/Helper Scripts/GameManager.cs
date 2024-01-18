
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void EndGame()
    {
        SceneManager.LoadSceneAsync("LoseScreen");
        Debug.Log("Game OVer");
    }
    public void Level2()

    {
        Debug.Log("level 2 loading");
        SceneManager.LoadSceneAsync("Level02");
    }
}
