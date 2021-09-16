using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewards
{
    public class CurrencyView : MonoBehaviour
    {
        private const string GoldKey = nameof(GoldKey);
        private const string CrystalKey = nameof(CrystalKey);

        public static CurrencyView Instance { get; private set; }

        [SerializeField]
        private TMP_Text _goldAmmountText;

        [SerializeField]
        private TMP_Text _crystalAmmountText;

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private Button _resetButton;

        private int Gold
        {
            get => PlayerPrefs.GetInt(GoldKey, 0);
            set => PlayerPrefs.SetInt(GoldKey, value);
        }

        private int Crystal
        {
            get => PlayerPrefs.GetInt(CrystalKey, 0);
            set => PlayerPrefs.SetInt(CrystalKey, value);
        }
        public Button BackButton  => _backButton;
        public Button ResetButton  => _resetButton; 

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            RefreshText();
        }

        public void AddGold(int value)
        {
            Gold += value;

            RefreshText();
        }

        public void AddCrystal(int value)
        {
            Crystal += value;

            RefreshText();
        }

        private void RefreshText()
        {
            _goldAmmountText.text = Gold.ToString();
            _crystalAmmountText.text = Crystal.ToString();
        }
    }
}