using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete {
    public class TankWeapon : MonoBehaviour {
        [Header("Tank")]
        public Transform m_FireTransform;
        public AudioSource m_ShootingAudio;

        [Header("Weapon")]
        public GameObject m_Shell;
        public AudioClip m_FireClip;
        public float launchVelocity = 30f;
        public float recoilStrength = 0;
        public bool autoFire = true;
        public float fireRate = 0.3f;
        public ParticleSystem particleFX;

        private bool isTriggered = false;
        private Coroutine fireCoroutine;

        public void fire() {
            GameObject shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as GameObject;
            shellInstance.GetComponent<Rigidbody>().velocity = launchVelocity * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().owner = gameObject;
            gameObject.GetComponent<Rigidbody>().AddForce(-recoilStrength * m_FireTransform.forward);

            particleFX.Play();
            //GameObject particles = Instantiate(particleFX, m_FireTransform.position, m_FireTransform.rotation);

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();
        }
        public void triggerOn() {
            if (autoFire) {
                isTriggered = true;
                fireCoroutine = StartCoroutine(fireLoop());
            } else {
                fire();
            }
        }
        public void triggerOff() {
            isTriggered = false;
            if (fireCoroutine != null) {
                StopCoroutine(fireCoroutine);
            }
        }
        public IEnumerator fireLoop() {
            while (isTriggered) {
                fire();
                yield return new WaitForSeconds(fireRate);
            }
        }
    }
}