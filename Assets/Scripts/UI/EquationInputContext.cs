#nullable enable
using System;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class EquationInputContext
    {
        public Action<string>? OnInputChanged;

        public string InputEquation
        {
            get => _inputEquation;
            set
            {
                _inputEquation = value;
                OnInputChanged?.Invoke(_inputEquation);
            }
        }

        private string _inputEquation = string.Empty;

        public Action<IReadOnlyList<string>?>? OnErrorListUpdate;

        public IReadOnlyList<string>? ErrorList
        {
            get => _errorList;
            set
            {
                _errorList = value;
                OnErrorListUpdate?.Invoke(value);
            }
        }

        private IReadOnlyList<string>? _errorList;

        public Action<uint>? OnMaxCharacterCountChanged;

        public uint MaxCharacterCount
        {
            get => _maxCharacterCount;
            set
            {
                _maxCharacterCount = value;
                OnMaxCharacterCountChanged?.Invoke(value);
            }
        }
        
        private uint _maxCharacterCount = 10;
    }
}
