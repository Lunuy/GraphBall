#nullable enable
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    // ReSharper disable once UnusedMember.Global
    public class EquationInputUi : MonoBehaviour
    {
        public GameObject InputPanel = null!;
        public TMP_InputField EquationInputField = null!;
        public TMP_Text LeftCharacterText = null!;
        public TMP_Text ParseErrorText = null!;
    }
}
