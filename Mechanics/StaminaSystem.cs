using System.Collections;
using UnityEngine;

[System.Serializable]
public struct StaminaSettings {
    public float maxStamina;
    public float runStaminaDrain;
    public float staminaRegen;
    public float staminaRegenDelay;
    public float exhaustedSpeedMultiplier;
    public bool infiniteStamina;
}
public class StaminaSystem : MonoBehaviour {
    [Header("Stamina Settings")]
    public StaminaSettings _settings = new StaminaSettings{
        maxStamina = 1000f,
        runStaminaDrain = 10f,
        staminaRegen = 8f,
        staminaRegenDelay = 0.25f,
        exhaustedSpeedMultiplier = 0.3f,
        infiniteStamina = false
    };
    
    [SerializeField] private float _currentStamina;
    private bool _isExhausted = false;
    private Coroutine _staminaCoroutine;
    private bool _isDraining = false;
    
    public event System.Action<float> OnStaminaChanged;
    public event System.Action<bool> OnExhaustedStateChanged;
    
    public float CurrentStamina => _currentStamina;
    public float MaxStamina => _settings.maxStamina;
    public bool IsExhausted => _isExhausted;
    public bool IsDraining => _isDraining;

    private void Start() {
        _currentStamina = _settings.maxStamina;
    }

    #region Stamina
        public void StartRunDrain() {
            if (_settings.infiniteStamina || _isExhausted) return;
            StartDraining(_settings.runStaminaDrain);
        }
        public void StopDrain() {
            if (_isDraining) {
                _isDraining = false;
                StartRegeneration();
            }
        }
        public void AddStamina(float amount) {
            _currentStamina = Mathf.Clamp(_currentStamina + amount, 0, _settings.maxStamina);
            OnStaminaChanged?.Invoke(_currentStamina / _settings.maxStamina);
        }
        public void SetInfiniteStamina(bool enabled) {
            _settings.infiniteStamina = enabled;
            if (enabled) {
                _currentStamina = _settings.maxStamina;
                _isExhausted = false;
                OnStaminaChanged?.Invoke(1f);
            }
        }
        private void StartDraining(float drainRate){
            _isDraining = true;
            
            if (_staminaCoroutine != null) StopCoroutine(_staminaCoroutine);
                
            _staminaCoroutine = StartCoroutine(DrainStaminaCoroutine(drainRate));
        }
        
        private void StartRegeneration() {
            if (_staminaCoroutine != null) StopCoroutine(_staminaCoroutine);
                
            _staminaCoroutine = StartCoroutine(RegenerateStaminaCoroutine());
        }
        private IEnumerator DrainStaminaCoroutine(float drainRate) {
            while (_isDraining && _currentStamina > 0 && !_settings.infiniteStamina) {
                _currentStamina -= drainRate * Time.deltaTime;
                _currentStamina = Mathf.Max(0, _currentStamina);
                
                OnStaminaChanged?.Invoke(_currentStamina / _settings.maxStamina);
                
                if (_currentStamina <= 0 && !_isExhausted) {
                    _isExhausted = true;
                    OnExhaustedStateChanged?.Invoke(true);
                }
                
                yield return null;
            }
            
            if (_currentStamina <= 0) {
                _isExhausted = true;
                OnExhaustedStateChanged?.Invoke(true);
            }
        }
        private IEnumerator RegenerateStaminaCoroutine() {
            yield return new WaitForSeconds(_settings.staminaRegenDelay);
            
            while (_currentStamina < _settings.maxStamina && !_isDraining) {
                _currentStamina += _settings.staminaRegen * Time.deltaTime;
                _currentStamina = Mathf.Min(_currentStamina, _settings.maxStamina);
                
                OnStaminaChanged?.Invoke(_currentStamina / _settings.maxStamina);
                
                if (_isExhausted && _currentStamina >= _settings.maxStamina * 0.3f) {
                    _isExhausted = false;
                    OnExhaustedStateChanged?.Invoke(false);
                }
                
                yield return null;
            }
        }
    #endregion
}