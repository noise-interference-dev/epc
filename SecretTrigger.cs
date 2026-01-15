using System.Collections;
using UnityEngine;

public class SecretTrigger : MonoBehaviour {
    [SerializeField] private float timeToWait;
    [SerializeField] private Vector3 newPos;
    private Coroutine _moveCoroutine;
    private void OnTriggerEnter(Collider _coll) {
        if (_coll.CompareTag("Player")) {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MovePlayer(_coll.gameObject));
        }
    }
    private void OnTriggerExit(Collider _coll) {
        if (_coll.CompareTag("Player")) {
            if (_moveCoroutine != null) {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
        }
    }
    private IEnumerator MovePlayer(GameObject player) {
        yield return new WaitForSecondsRealtime(timeToWait);
        if (player != null) player.transform.position = newPos;
        _moveCoroutine = null;
    }
}