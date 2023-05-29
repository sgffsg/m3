using UnityEngine;
using UnityEngine.Assertions;
using Match3.Scripts.Levels;
using UnityEngine.UI;
using TMPro;

namespace Match3.Scripts.UI
{
    public class GameUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI limitTitleText;
        [SerializeField] private TextMeshProUGUI limitText;

        private void Awake()
        {
            Assert.IsNotNull(limitTitleText);
            Assert.IsNotNull(limitText);
        }

        public void SetLimitType(LimitType type)
        {
            limitTitleText.text = type == LimitType.Moves ? "Moves" : "Seconds";
        }

        public void SetLimit(int amount)
        {
            limitText.text = amount.ToString();
        }

        public void SetLimit(string amount)
        {
            limitText.text = amount;
        }
    }
}
