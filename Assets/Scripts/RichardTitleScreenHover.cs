using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class RichardTitleScreenHover : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI phrase;

    public string[] richardDescs = new string[]
    {
        "Richard likes the sea.",
        "Richard likes jelly and peanut butter.",
        "Richard pronounces it 'jif'."
    };

    void OnEnable()
    {
        int index = Random.Range(0, richardDescs.Length);
        phrase.text = richardDescs[index];

    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = mousePos;
    }
}
