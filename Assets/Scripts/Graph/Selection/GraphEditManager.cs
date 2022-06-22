#nullable enable
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.UI;

namespace Assets.Scripts.Graph.Selection
{
    public class GraphEditManager : MonoBehaviour
    {
        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                if (_isEditable == false)
                {
                    CloseExprEditor();
                }
            }
        }

        private readonly List<GraphEditClient> _clients = new();
        private bool _isEditable = true;
        private EquationInputUi? _equationInputUi;
        private SimulationControlUi? _simulationControlUi;

        // ReSharper disable once UnusedMember.Global
        public void SetUi(IReadOnlyDictionary<string, List<UnityEngine.Object>> dict)
        {
            _equationInputUi = (EquationInputUi) dict["GameUI"][0];
            _simulationControlUi = (SimulationControlUi) dict["GameUI"][1];
        }

        public void AddClient(GraphEditClient client)
        {
            _clients.Add(client);

            client.OnClick += _onGraphClick;
            client.OnGraphUpdate += OnGraphUpdate;
        }

        private void _onGraphClick(GraphEditClient graphEditClient)
        {
            if (_isEditable)
            {
                OpenExprEditor(graphEditClient);
            }
        }

        private void OnGraphUpdate(GraphEditClient graphEditClient)
        {
            if (_simulationControlUi == null)
            {
                throw new Exception("_simulationControlUi is null");
            }

            var isThereExprError = false;
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < _clients.Count; ++i)
            {
                if (!_clients[i].HasExprError) continue;
                isThereExprError = true;
                break;
            }

            _simulationControlUi.CanPlay = !isThereExprError;
        }

        private void OpenExprEditor(GraphEditClient graphEditClient)
        {
            if (_equationInputUi == null)
            {
                throw new Exception("EquationInputUi is null");
            }

            if (graphEditClient.EquationInputContext == null)
            {
                throw new Exception("EquationInputContext is null");
            }

            _equationInputUi.ChangeContext(graphEditClient.EquationInputContext);
        }

        private void CloseExprEditor()
        {
            if (_equationInputUi == null)
            {
                throw new Exception("EquationInputUi is null");
            }

            _equationInputUi.ChangeContext(null);
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            foreach (var client in _clients)
            {
                client.OnClick -= _onGraphClick;
            }
        }
    }
}
