using Unity.Burst.CompilerServices;
using Unity.Services.Analytics.Internal;
using UnityEngine;

namespace Complete {
    public class TankMovement : MonoBehaviour {
        public float movementStrength = 50f;
        public float lerpSpeed = 0.3f;
        public float aimLerpStrength = 1f;
        public float enginePitchRange = 0.2f;
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
        private void Start() {
            m_OriginalPitch = m_MovementAudio.pitch;
        }
        private void Update() {
            EngineAudio();
        }
        private void FixedUpdate() {
            orientToMovement();
        }
        public void onMoveInput(Vector3 input) {
            m_Rigidbody.AddForce(input.normalized * movementStrength);
        }
        public void aimAt(Vector3 point) {
            Vector3 aimDirection = (point - transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
            newRotation.x = 0;
            newRotation.z = 0;
            newRotation = Quaternion.Lerp(turretTransform.transform.rotation, newRotation, aimLerpStrength);
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
        private void EngineAudio() {
            if (m_Rigidbody.velocity.magnitude < 0.1f) {
                if (m_MovementAudio.clip == m_EngineDriving) {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - enginePitchRange, m_OriginalPitch + enginePitchRange);
                    m_MovementAudio.Play();
                }
            } else if (m_MovementAudio.clip == m_EngineIdling) {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - enginePitchRange, m_OriginalPitch + enginePitchRange);
                m_MovementAudio.Play();
            }
        }
    }
}