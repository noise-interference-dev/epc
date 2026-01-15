using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using EPC;

namespace EPC {
    public class MainMenuMove : MonoBehaviour {
        [SerializeField] private MainControl control;
        [SerializeField] private Animator anim;
        [SerializeField] private MenuPoint Main, PlayMode, Settings, Shop;

        private GameObject _activePanel;
        private CancellationTokenSource _cts;

        private void Start() {
            SetMain();
        }

        private async UniTaskVoid SwitchPoint(MenuPoint point) {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            
            if (_activePanel != null) _activePanel.SetActive(false);
            if (anim != null) anim.SetBool("open", false);

            await control.MoveAndRotateAsync(transform, point.Pos, Quaternion.Euler(point.Rot), point.Duration, _cts.Token);

            if (point.Panel != null) {
                point.Panel.SetActive(true);
                _activePanel = point.Panel;
            }
        }

        public void SetMain() {
            SwitchPoint(Main).Forget();
            if (anim != null) anim.SetBool("open", true);
        }
        public void SetPlayMode() => SwitchPoint(PlayMode).Forget();
        public void SetSettings() => SwitchPoint(Settings).Forget();
        public void SetShop() => SwitchPoint(Shop).Forget();
    }

    [Serializable]
    public class MenuPoint {
        public GameObject Panel;
        public Vector3 Pos;
        public Vector3 Rot;
        public float Duration = 1.0f;
    }
}