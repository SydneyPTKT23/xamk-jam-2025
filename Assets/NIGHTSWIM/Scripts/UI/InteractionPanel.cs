using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace slc.NIGHTSWIM.UI
{
    public class InteractionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltipLabel;

        public void SetLabel(string t_message)
        {
            tooltipLabel.SetText(t_message);
        }

        public void ResetUI()
        {
            tooltipLabel.SetText("");
        }
    }
}