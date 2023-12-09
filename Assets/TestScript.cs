using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    private Button button;
    private bool clicked = false;

    private void Loaded() => Debug.Log("Finished loading!");

    private void Clicked()
    {
        if (clicked)
            return;
        
        clicked = true;
        SceneController.Instance.LoadScene("FrameworkTesting", Loaded);
    }

    private void Start() 
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Clicked);
    }
}
