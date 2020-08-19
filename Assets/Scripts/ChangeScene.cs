using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangingScene(int aScene)
    {
        SceneManager.LoadScene(aScene);
    }
}
