using UnityEngine;
using UnityEngine.UI;

public class Final : MonoBehaviour
{
    [SerializeField] private Button finalButton;

    private void Update()
    {
        if(PlayerPrefs.HasKey("CardYellow") && PlayerPrefs.HasKey("CardRed") && PlayerPrefs.HasKey("CardBlue"))
        {
            finalButton.interactable = true;
        }
        else
        {
            finalButton.interactable = false;
        }
    }

}
