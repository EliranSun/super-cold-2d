using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadSceneNumber(int number)
    {
        SceneManager.LoadScene(number - 1);
    }
}