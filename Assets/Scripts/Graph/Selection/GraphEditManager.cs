#nullable enable
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.UI;

namespace Assets.Scripts.Graph.Selection
{
    public class GraphEditManager : MonoBehaviour
    {
        public bool IsEditable {
            get => _isEditable;
            set {
                _isEditable = value;
                if(_isEditable == false) {
                    _closeExprEditor();
                }
            } 
        }

        private List<GraphEditClient> _clients = new();
        private bool _isEditable = true;
        private EquationInputUi? _equationInputUi;
        private SimulationControlUi? _simulationControlUi;

        public void SetUi(IReadOnlyDictionary<string, List<UnityEngine.Object>> dict) {
            _equationInputUi = (EquationInputUi)dict["GameUI"][0];
            _simulationControlUi = (Assets.Scripts.UI.SimulationControlUi)dict["GameUI"][1];
        }

        public void AddClient(GraphEditClient client)
        {
            _clients.Add(client);

            client.OnClick += _onGraphClick;
            client.OnGraphUpdate += _onGraphUpdate;
        }

        private void _onGraphClick(GraphEditClient graphEditClient)
        {
            if(_isEditable) {
                _openExprEditor(graphEditClient);
            }
        }

        private void _onGraphUpdate(GraphEditClient graphEditClient) {
            if(_simulationControlUi == null) {
                throw new Exception("_simulationControlUi is null");
            }

            bool isThereExprError = false;
            for(int i = 0; i < _clients.Count; i++) {
                if(_clients[i].HasExprError) {
                    isThereExprError = true;
                    break;
                }
            }

            if(isThereExprError) {
                _simulationControlUi.CanPlay = false;
            } else {
                _simulationControlUi.CanPlay = true;
            }
        }

        private void _openExprEditor(GraphEditClient graphEditClient) {
            if(_equationInputUi == null) {
                throw new Exception("EquationInputUi is null");
            }
            if(graphEditClient.EquationInputContext == null) {
                throw new Exception("EquationInputContext is null");
            }
            
            _equationInputUi.ChangeContext(graphEditClient.EquationInputContext);
        }

        private void _closeExprEditor() {
            Debug.Log("CLOSE EDITOR");
        }

        public void OnDestroy()
        {
            foreach (var client in _clients)
            {
                client.OnClick -= _onGraphClick;
            }
        }
    }
}
