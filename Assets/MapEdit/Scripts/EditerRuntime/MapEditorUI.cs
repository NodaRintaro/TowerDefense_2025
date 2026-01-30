using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TD.Map;
using TMPro;

namespace TD.EditorRuntime
{
    /// <summary>
    /// uGUIの簡易マップエディタUI。
    /// - MapId 入力 + Load/Save
    /// - Route選択 + 追加
    /// - Tool/Brush選択
    /// - Waypoint選択 + 上下移動
    /// </summary>
    public sealed class MapEditorUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private MapEditorController3D _controller;
        
        [Header("Map IO")]
        [SerializeField] private TMP_InputField _mapNameInput;
        [SerializeField] private TMP_InputField _mapIdInput;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;

        [Header("Route")]
        [SerializeField] private TMP_Dropdown _routeDropdown;
        [SerializeField] private Button _addRouteButton;
        [SerializeField] private Button _RemoveRouteButton;

        [Header("Edit")]
        [SerializeField] private TMP_Dropdown _toolDropdown;
        [SerializeField] private TMP_Dropdown _brushDropdown;

        [Header("Waypoints")]
        [SerializeField] private TMP_Dropdown _waypointDropdown;
        [SerializeField] private Button _wpUpButton;
        [SerializeField] private Button _wpDownButton;

        [Header("Status")]
        [SerializeField] private TMP_Text _statusText;

        private void Awake()
        {
            // Tool Dropdown
            _toolDropdown.ClearOptions();
            _toolDropdown.AddOptions(new System.Collections.Generic.List<string>
            {
                "Q Paint",
                "W AddSpawn",
                "E AddGoal",
                "R AddWaypoint",
                "T RemovePoint",
            });

            // Brush Dropdown
            _brushDropdown.ClearOptions();
            _brushDropdown.AddOptions(new System.Collections.Generic.List<string>
            {
                "Empty",
                "Road",
                "HighPlatform",
                "Block",
            });
        }

        private void OnEnable()
        {
            if (_controller != null)
            {
                _controller.OnMapChanged += HandleMapChanged;
                _controller.OnRoutesChanged += HandleRoutesChanged;
            }

            // UI events
            _loadButton.onClick.AddListener(OnClickLoad);
            _saveButton.onClick.AddListener(OnClickSave);

            _routeDropdown.onValueChanged.AddListener(OnRouteChanged);
            _addRouteButton.onClick.AddListener(OnClickAddRoute);
            _RemoveRouteButton.onClick.AddListener(OnClickRemoveRoute);

            _toolDropdown.onValueChanged.AddListener(OnToolChanged);
            _brushDropdown.onValueChanged.AddListener(OnBrushChanged);

            _waypointDropdown.onValueChanged.AddListener(OnWaypointChanged);
            _wpUpButton.onClick.AddListener(OnClickWpUp);
            _wpDownButton.onClick.AddListener(OnClickWpDown);

            // Initial sync
            SyncAllUI();
        }

        private void OnDisable()
        {
            if (_controller != null)
            {
                _controller.OnMapChanged -= HandleMapChanged;
                _controller.OnRoutesChanged -= HandleRoutesChanged;
            }

            _loadButton.onClick.RemoveListener(OnClickLoad);
            _saveButton.onClick.RemoveListener(OnClickSave);

            _routeDropdown.onValueChanged.RemoveListener(OnRouteChanged);
            _addRouteButton.onClick.RemoveListener(OnClickAddRoute);
            _RemoveRouteButton.onClick.RemoveListener(OnClickRemoveRoute);

            _toolDropdown.onValueChanged.RemoveListener(OnToolChanged);
            _brushDropdown.onValueChanged.RemoveListener(OnBrushChanged);

            _waypointDropdown.onValueChanged.RemoveListener(OnWaypointChanged);
            _wpUpButton.onClick.RemoveListener(OnClickWpUp);
            _wpDownButton.onClick.RemoveListener(OnClickWpDown);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnToolChanged(0);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                OnToolChanged(1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                OnToolChanged(2);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                OnToolChanged(3);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                OnToolChanged(4);
            }
        }

        private void HandleMapChanged() => SyncStatus();
        private void HandleRoutesChanged() => SyncAllUI();

        private void SyncAllUI()
        {
            if (_controller == null) return;
            
            SyncRouteDropdown();
            SyncWaypointDropdown();
            SyncStatus();
        }

        private void SyncRouteDropdown()
        {
            _routeDropdown.ClearOptions();

            var routes = _controller.Routes;
            var opts = new System.Collections.Generic.List<string>();
            for (int i = 0; i < routes.Count; i++)
            {
                var id = routes[i]?.routeId ?? $"(null:{i})";
                opts.Add($"{i}: {id}");
            }

            if (opts.Count == 0) opts.Add("0: (none)");
            _routeDropdown.AddOptions(opts);

            var active = Mathf.Clamp(_controller.ActiveRouteIndex, 0, opts.Count - 1);
            _routeDropdown.SetValueWithoutNotify(active);
        }

        private void SyncWaypointDropdown()
        {
            _waypointDropdown.ClearOptions();

            var routes = _controller.Routes;
            if (routes.Count == 0)
            {
                _waypointDropdown.AddOptions(new System.Collections.Generic.List<string> { "(no route)" });
                _waypointDropdown.SetValueWithoutNotify(0);
                SetWaypointButtonsInteractable(false);
                return;
            }

            var r = routes[_controller.ActiveRouteIndex];
            var wps = r.waypoints;

            if (wps == null || wps.Count == 0)
            {
                _waypointDropdown.AddOptions(new System.Collections.Generic.List<string> { "(no waypoint)" });
                _waypointDropdown.SetValueWithoutNotify(0);
                SetWaypointButtonsInteractable(false);
                return;
            }

            var opts = new System.Collections.Generic.List<string>();
            for (int i = 0; i < wps.Count; i++)
                opts.Add($"{i}: ({wps[i].x},{wps[i].y})");

            _waypointDropdown.AddOptions(opts);
            _waypointDropdown.SetValueWithoutNotify(Mathf.Clamp(_waypointDropdown.value, 0, opts.Count - 1));
            SetWaypointButtonsInteractable(true);
        }

        private void SetWaypointButtonsInteractable(bool enable)
        {
            if (_wpUpButton != null) _wpUpButton.interactable = enable;
            if (_wpDownButton != null) _wpDownButton.interactable = enable;
        }

        private void SyncStatus()
        {
            if (_statusText == null || _controller == null) return;

            if (_controller.Map == null)
                _controller.CreateNew(1, 1);
            var sb = new StringBuilder();

            sb.AppendLine($"MapId: {_controller.MapId}");
            sb.AppendLine($"Size : {_controller.Map.Width} x {_controller.Map.Height}");

            if (_controller.Routes.Count > 0)
            {
                var r = _controller.Routes[_controller.ActiveRouteIndex];
                int s = r.spawns?.Count ?? 0;
                int g = r.goals?.Count ?? 0;
                int w = r.waypoints?.Count ?? 0;
                sb.AppendLine($"Route: {r.routeId}  Spawns:{s} Goals:{g} WPs:{w}");
            }
            
            _statusText.text = sb.ToString();
        }

        // ===== Button / UI handlers =====

        private void OnClickLoad()
        {
            if (_controller == null) return;

            var mapId = _mapIdInput != null ? int.Parse(_mapIdInput.text) : _controller.MapId;
            _controller.MapId = mapId;
            var mapName = _mapNameInput != null ? _mapNameInput.text : _controller.MapName;
            _controller.MapName = mapName;

            var ok = _controller.TryLoad(mapId);
            if (!ok)
            {
                if (_statusText != null) _statusText.text = $"Load failed: {mapId}";
                return;
            }
            _mapNameInput.text = _controller.MapName;

            // Loadの中でイベント呼ぶのが理想だけど、保険で同期
            SyncAllUI();
        }

        private void OnClickSave()
        {
            if (_controller == null) return;

            if (_mapIdInput != null)
                _controller.MapId = int.Parse(_mapIdInput.text);
            if(_mapNameInput != null)
                _controller.MapName = _mapNameInput.text;

            if (_controller.TrySaveUser(out var error))
            {
                if (_statusText != null) _statusText.text = "Saved (persistentDataPath).";
            }
            else
            {
                if (_statusText != null) _statusText.text = $"Save failed: {error}";
            }
        }

        private void OnClickAddRoute()
        {
            if (_controller == null) return;
            _controller.AddRouteAuto();
            SyncAllUI();
        }
        
        private void OnClickRemoveRoute()
        {
            if (_controller == null) return;
            _controller.RemoveActiveRoute();
            SyncAllUI();
        }

        private void OnRouteChanged(int index)
        {
            if (_controller == null) return;
            _controller.SetActiveRouteIndex(index);
            SyncAllUI();
        }

        private void OnToolChanged(int index)
        {
            if (_controller == null) return;

            // MapEditorController3D.EditTool の順番と一致させる
            var tool = (MapEditorController3D.EditTool)index;
            _controller.SetTool(tool);
            _toolDropdown.SetValueWithoutNotify(index);
            SyncStatus();
        }

        private void OnBrushChanged(int index)
        {
            if (_controller == null) return;

            var brush = (TileType)index; // Empty=0,Road=1,Block=2 の並び前提
            _controller.SetBrush(brush);
            SyncStatus();
        }

        private void OnWaypointChanged(int _)
        {
            SyncStatus();
        }

        private void OnClickWpUp()
        {
            if (_controller == null) return;

            var idx = _waypointDropdown.value;
            if (idx <= 0) return;

            _controller.MoveWaypoint(idx, idx - 1);
            _waypointDropdown.SetValueWithoutNotify(idx - 1);
            SyncAllUI();
        }

        private void OnClickWpDown()
        {
            if (_controller == null) return;

            var r = _controller.Routes[_controller.ActiveRouteIndex];
            var wps = r.waypoints;
            if (wps == null) return;

            var idx = _waypointDropdown.value;
            if (idx >= wps.Count - 1) return;

            _controller.MoveWaypoint(idx, idx + 1);
            _waypointDropdown.SetValueWithoutNotify(idx + 1);
            SyncAllUI();
        }
    }
}
