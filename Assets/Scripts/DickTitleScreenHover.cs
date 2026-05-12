using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DickTitleScreenHover : MonoBehaviour
{   
    [SerializeField] TextMeshProUGUI phrase;
    public string[] dickDescs = new string[]
    {
        "Dick likes the ocean.",
        "Dick likes peanut butter and jelly.",
        "Dick pronounces it 'gif'."
    };

    void OnEnable()
    {
        int index = Random.Range(0, dickDescs.Length);
        phrase.text = dickDescs[index];
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = mousePos;
    }

}
