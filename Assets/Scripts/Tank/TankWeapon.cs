using UnityEngine;
using UnityEngine.UI;

namespace Complete {
    public class TankWeapon : MonoBehaviour {
        public GameObject m_Shell;
        public Transform m_FireTransform;
        public AudioSource m_ShootingAudio;
        public AudioClip m_FireClip;
        public float m_MaxLaunchForce = 30f;

        public void Fire() {
            GameObject shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as GameObject;
            shellInstance.GetComponent<Rigidbody>().velocity = m_MaxLaunchForce * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().owner = gameObject;

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();
        }
    }
}