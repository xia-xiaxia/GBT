using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TODO : MonoBehaviour
{
    //�ı�ע�����Ļ���Ӣ��ð��
    //��Ʒ�ѵ�
    //�ֵ����AText
    //���ز��µ��ֿ��Թ�����ʾ

    //UI��С���䲻ͬ��Ļ��С��ת�����Ի��򣬹��ܰ�
    //UI����

    //Ļ�У����鲥�ż��ת��
    //Help������ʾ����˵��
    //������
    //������������

    //�첽���س���(�����ÿճ�����ת��)(���м�����)
    //����UI����

    //�ؿ�����

    /*1.	ÿ���ؿ���������һ��UI���壺
    �ŵ㣺
    ��ֱ�ӣ�ÿ���ؿ����������ж�����UI�����ڹ�����޸ġ�
    UI��ؿ��������ܽ�ϣ��ʺ���Ҫ�ڲ�ͬ�ؿ�����ʾ��ͬUI�������
    ȱ�㣺
    ���ܻᵼ���ظ���UIԪ�أ�����ά���ɱ���
    �л�����ʱ��UIҲ�����¼��أ�����Ӱ�����ܡ�
    2.	��UI����һ�������ĳ����У���Ҫʱ��ʾ��
    �ŵ㣺
    UI�����ڶ���ؿ�֮�乲�������ظ��������Դ��
    �л��ؿ�ʱ��UI�������¼��أ�������ܡ�
    ���ڼ��й���͸���UIԪ�ء�
    ȱ�㣺
    ��Ҫ������߼�������UI����ʾ�����ء�
    ������Ҫ����UI��ؿ�����֮��Ľ�����
    ��ʵ�ʿ����У��ڶ��ַ�������UI����һ�������ĳ����У�ͨ����Ϊ����������������Ҫ����UIԪ�أ������˵�����ͣ�˵���HUD�ȣ�ʱ��������һ��ʾ����չʾ�����Unity��ʵ�����ַ�����
    1.	����һ��������UI������
    ����һ���µĳ���������Ϊ UIScene��
    �� UIScene �����������Ҫ��UIԪ�أ������˵�����ͣ�˵���HUD�ȣ���
    2.	���������м���UI������
    ���������Ľű��У�ʹ�� SceneManager.LoadSceneAsync �������� UIScene�������� LoadSceneMode.Additive ������
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            // ����UI����
            SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
        }
    }
    3.����UI����ʾ�����أ�
    �� UIScene �еĽű��У�����߼�������UI����ʾ�����ء����磬����ʹ�õ���ģʽ������UI����ʾ״̬��
    ʾ�����룺

    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject); // ȷ��UI�������л��ؿ�ʱ���ᱻ����
        }

        public void ShowUI()
        {
            // ��ʾUI
            gameObject.SetActive(true);
        }

        public void HideUI()
        {
            // ����UI
            gameObject.SetActive(false);
        }
    }
    ͨ�����ַ�����������ڶ���ؿ�֮�乲��UI��������ܲ������ظ����롣
    */
}