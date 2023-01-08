using TMPro;
using UnityEngine;

namespace Code
{
    public class GameHud : MonoBehaviour
    {
        public TextMeshProUGUI text;
        
        public void UpdateSpice(float spice)
        {
            text.text = $"{spice:0}";
        }
    }
}