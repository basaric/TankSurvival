using UnityEngine;

namespace Complete {
    public class ShellExplosion : MonoBehaviour {
        public LayerMask m_TankMask;
        public ParticleSystem m_ExplosionParticles;
        public AudioSource m_ExplosionAudio;
        public float damageMultiplier = 100f;
        public float m_ExplosionForce = 1000f;
        public float m_MaxLifeTime = 2f;
        public float m_ExplosionRadius = 5f;
        public GameObject owner;

        private void Start() {
            Destroy(gameObject, m_MaxLifeTime);
        }

        private void OnTriggerEnter(Collider other) {
            /*
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
            for (int i = 0; i < colliders.Length; i++) 
            }
            */
            
            Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
            if (targetRigidbody) {
                //targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);
            }
                    
            TankHealth targetHealth = other.GetComponent<TankHealth>();
            if (targetHealth) {
                float damage = CalculateDamage(targetRigidbody.position);
                targetHealth.TakeDamage(damage);
            }

            Destroyable destroyable = other.GetComponent<Destroyable>();
            if (destroyable) {
                destroyable.destroy();
            }

            
            m_ExplosionParticles.transform.parent = null;
            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();
            ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
            Destroy(m_ExplosionParticles.gameObject, mainModule.duration);
            Destroy(gameObject);
        }

        private float CalculateDamage(Vector3 targetPosition) {
            Vector3 explosionToTarget = targetPosition - transform.position;
            float explosionDistance = explosionToTarget.magnitude;
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
            float damage = relativeDistance * damageMultiplier;
            damage = Mathf.Max(0f, damage);

            return damage;
        }
    }
}