using UnityEngine;
using TowerDefense;

//������� ��������

namespace SpaceShooter
{

    public class EntitySpawner1 : MonoBehaviour
    {
        public enum SpawnMode //�������
        {
            Start, //��� ������
            Loop //������������
        }

        [SerializeField] private Path m_path;

        [SerializeField] private Entity[] m_EntityPrefabs; //������ ��������

        [SerializeField] private EnemyAsset[] m_enemySettings;

        [SerializeField] private CircleArea m_Area; //���� � ��������� �������

        [SerializeField] private SpawnMode m_SpawnMode; //������ ����� ��� ����� ��������

        [SerializeField] private int m_NumSpawns; //����������� ��������

        [SerializeField] private float m_RespawnTime; //��� ����� ����������� ������

        private float m_Timer; //������

        private void Start()
        {
            if(m_SpawnMode == SpawnMode.Start) //��� �� �������
            {
                SpawnEntities();  //����� ������ ��������
            }

            m_Timer = m_RespawnTime; //���������� ������

        }

        private void Update()
        {
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime; 

            if (m_SpawnMode == SpawnMode.Loop && m_Timer < 0)  //���� ����������� ������ � ��������� ������ ������
            {
                SpawnEntities();  //����� ������ ��������

                m_Timer = m_RespawnTime; //���������� ������
            }

        }

        private void SpawnEntities()   //����� ������ ��������
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                int index = Random.Range(0, m_EntityPrefabs.Length);

                GameObject e = Instantiate(m_EntityPrefabs[index].gameObject); //������� ��� ������ (e)

                e.transform.position = m_Area.RandomInsideZone; //������ ������� �������� � ��������� �����
                if (e.TryGetComponent<TDPatrolController> (out var ai))
                {
                    ai.SetPath(m_path);
                }
                if (e.TryGetComponent<Enemy>(out var en))
                {
                    en.Use(m_enemySettings[Random.Range( 0, 2 )]);
                }
            }
        }
    }
}
