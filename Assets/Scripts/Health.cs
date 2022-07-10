#pragma warning disable  0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Health : MonoBehaviour
{
    [SerializeField] public int m_HP = 10;
    [SerializeField] GameObject m_DeathParticles;
    private SpriteRenderer m_SpriteRenderer;
    private bool m_isPlayer;
    private bool m_isDead;
    HP_Display m_hpdp;
    CinemachineImpulseSource m_CineIS;

    void Start()
    {
        m_isDead = false;
        if(gameObject.CompareTag("Player"))
        {
            m_isPlayer = true;
            m_hpdp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MenuManager>().m_hpD;
        }
        else
        {
            m_isPlayer = false;
        }
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_CineIS = Camera.main.GetComponent<CinemachineImpulseSource>();
    }

    public void TakeDmg(int dmg)
    {

        if(m_isPlayer)
        {
            m_CineIS.GenerateImpulse(Vector3.down * dmg);
            m_hpdp.RemoveHp(dmg);
        }

        if(m_HP-dmg<=0)
        {
            if(m_DeathParticles!=null)
            {
                Instantiate(m_DeathParticles, transform.position, transform.rotation);
            }

            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb) rb.constraints = RigidbodyConstraints2D.None;
            gameObject.GetComponent<Collider2D>().enabled = false;
            EnemyPath ep = gameObject.GetComponent<EnemyPath>();
            if (ep) ep.enabled = false;
            AudioSource aS = gameObject.GetComponent<AudioSource>();
            if (aS) aS.enabled = false;

            m_SpriteRenderer.color = Color.red;
            Destroy(gameObject,3f);

            if(m_isPlayer) Invoke("ReloadScene", 1.5f);

            m_isDead = true;
            return;
        }
        StartCoroutine(RedOnDmg(0.2f));
        m_HP -= dmg;
    }

    IEnumerator RedOnDmg(float t)
    {
        if(m_SpriteRenderer!=null)
        {
            m_SpriteRenderer.color = Color.red;
        }

        yield return new WaitForSeconds(t);

        if (m_SpriteRenderer!=null&&!m_isDead)
        {
            m_SpriteRenderer.color = Color.white;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
