using Unity.Burst.CompilerServices;
using Unity.Services.Analytics.Internal;
using UnityEngine;

namespace Complete {
    public class TankMovement : MonoBehaviour {
        public float m_Speed = 12f;
        public float m_TurnSpeed = 180f;
        public float m_PitchRange = 0.2f;
        public float movementStrength = 50f;
        public float lerpSpeed = 0.3f;
        public AudioSource m_MovementAudio;
        public AudioClip m_EngineIdling;
        public AudioClip m_EngineDriving;
        public Transform turretTransform;

        private Rigidbody m_Rigidbody;
        private float m_OriginalPitch;
        private ParticleSystem[] m_particleSystems;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            m_Rigidbody.isKinematic = false;
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i) {
                m_particleSystems[i].Play();
            }
        }

        private void OnDisable() {
            m_Rigidbody.isKinematic = true;
            for (int i = 0; i < m_particleSystems.Length; ++i) {
                m_particleSystems[i].Stop();
            }
        }

        private void Start() {
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void Update() {
            EngineAudio();
        }

        public void onMoveInput(Vector3 input) {
            m_Rigidbody.AddForce(input.normalized * movementStrength);
            orientToMovement();
        }
        public void aimAt(Vector3 point) {
            Vector3 aimDirection = (point - transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
            newRotation.x = 0;
            newRotation.z = 0;
            turretTransform.transform.rotation = newRotation;
        }
        public void orientToMovement() {
            Vector3 XZVelocity = m_Rigidbody.velocity;
            XZVelocity.y = 0;
            if (XZVelocity.magnitude > 0.1f) {
                Quaternion newRotation = Quaternion.LookRotation(XZVelocity.normalized, Vector3.up);
                newRotation.x = 0;
                newRotation.z = 0;
                float _lerpSpeed = 0.3f + (Vector3.Dot(transform.forward, XZVelocity.normalized) - 1f) * 0.5f * 0.2f;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, _lerpSpeed);
            }
        }

        //TODO
        private Vector3 movementInput = Vector3.zero;
        private void EngineAudio() {
            if (Mathf.Abs(movementInput.x) < 0.1f && Mathf.Abs(movementInput.y) < 0.1f) {
                if (m_MovementAudio.clip == m_EngineDriving) {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else {
                if (m_MovementAudio.clip == m_EngineIdling) {
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }
    }
}