using UnityEngine;
using UnityEngine.UI;

namespace Complete {
    public class TankHealth : MonoBehaviour {
        public float m_StartingHealth = 100f;
        public Slider m_Slider;
        public GameObject m_ExplosionPrefab;
        public float m_CurrentHealth;
        public Vector3 positionOffset = new Vector3(0, 50, 0);

        private Camera targetCamera;
        private AudioSource m_ExplosionAudio;
        private ParticleSystem m_ExplosionParticles;

        private void Awake() {
            m_Slider.gameObject.transform.parent.gameObject.SetActive(true);
        }
        private void FixedUpdate() {
            Vector3 newPosition = targetCamera.WorldToScreenPoint(gameObject.transform.position) + positionOffset;
            m_Slider.transform.position = newPosition;
        }
        private void OnEnable() {
            targetCamera = Camera.main;
            m_CurrentHealth = m_StartingHealth;
            SetHealthUI();
        }
        public void TakeDamage(float amount) {
            m_CurrentHealth -= amount;
            SetHealthUI();
            if (m_CurrentHealth <= 0f) {
                OnDeath();
            }
        }
        private void SetHealthUI() {
            m_Slider.value = m_CurrentHealth;
        }
        private void OnDeath() {
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab, transform).GetComponent<ParticleSystem>();
            m_ExplosionParticles.Play();
            m_ExplosionParticles.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }
}