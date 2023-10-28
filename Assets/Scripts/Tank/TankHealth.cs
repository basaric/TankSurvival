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
        private bool m_Dead;

        private void Awake() {
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
            m_ExplosionParticles.gameObject.SetActive(false);
            m_Slider.gameObject.transform.parent.gameObject.SetActive(true);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(1)) {
                TakeDamage(m_StartingHealth / 6);
            }
        }
        private void FixedUpdate() {
            Vector3 newPosition = targetCamera.WorldToScreenPoint(gameObject.transform.position) + positionOffset;
            m_Slider.transform.position = newPosition;
        }
        private void OnEnable() {
            targetCamera = Camera.main;
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;
            SetHealthUI();
        }

        public void TakeDamage(float amount) {
            m_CurrentHealth -= amount;
            SetHealthUI();
            if (m_CurrentHealth <= 0f && !m_Dead) {
                OnDeath();
            }
        }

        private void SetHealthUI() {
            m_Slider.value = m_CurrentHealth;
        }

        private void OnDeath() {
            m_Dead = true;
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive(true);
            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();
            gameObject.SetActive(false);
        }
    }
}