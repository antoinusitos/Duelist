using UnityEngine;
using UnityEngine.UI;

public class TEMPSCRIPT : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().text = Application.dataPath;
    }
}
