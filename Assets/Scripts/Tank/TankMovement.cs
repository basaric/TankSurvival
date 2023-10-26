using Unity.Services.Analytics.Internal;
using UnityEngine;

namespace Complete {
    public class TankMovement : MonoBehaviour {
        public float m_Speed = 12f;
        public float m_TurnSpeed = 180f;

        public AudioSource m_MovementAudio;
        public AudioClip m_EngineIdling;
        public AudioClip m_EngineDriving;
        public float m_PitchRange = 0.2f;

        public float movementStrength = 50f;
        public float lerpSpeed = 0.3f;

        public Transform turretTransform;

        private string horizontalAxisName;
        private string verticalAxisName;
        private Rigidbody m_Rigidbody;
        private float inputHorizontal;
        private float inputVertical;
        private float m_OriginalPitch;
        private ParticleSystem[] m_particleSystems;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            m_Rigidbody.isKinematic = false;
            inputHorizontal = 0f;
            inputVertical = 0f;
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
            horizontalAxisName = "Vertical1";
            verticalAxisName = "Horizontal1";
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void Update() {
            inputHorizontal = Input.GetAxis(horizontalAxisName);
            inputVertical = Input.GetAxis(verticalAxisName);
            EngineAudio();
        }

        private void EngineAudio() {
            if (Mathf.Abs(inputHorizontal) < 0.1f && Mathf.Abs(inputVertical) < 0.1f) {
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

        private void FixedUpdate() {
            Vector3 moveInput = Camera.main.transform.forward * inputHorizontal + Camera.main.transform.right * inputVertical;
            moveInput.y = 0;
            m_Rigidbody.AddForce(moveInput.normalized * movementStrength);
            Vector3 XZVelocity = m_Rigidbody.velocity;
            XZVelocity.y = 0;
            if (XZVelocity.magnitude > 0.1f) {
                Quaternion newRotation = Quaternion.LookRotation(XZVelocity.normalized, Vector3.up);
                newRotation.x = 0;
                newRotation.z = 0;
                float _lerpSpeed = 0.3f + (Vector3.Dot(transform.forward, XZVelocity.normalized) - 1f) * 0.5f * 0.2f;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpSpeed);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Vector3 aimDirection = (hit.point - transform.position).normalized;
                Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
                newRotation.x = 0;
                newRotation.z = 0;
                turretTransform.transform.rotation = newRotation;
            }
        }
    }
}