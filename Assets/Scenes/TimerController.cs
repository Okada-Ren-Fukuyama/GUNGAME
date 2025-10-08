using UnityEngine;
using UnityEngine.UI; // Text (Legacy) �̏ꍇ
// using TMPro; // Text Mesh Pro �̏ꍇ

public class TimerController : MonoBehaviour
{
    // �������Ԃ�Inspector����ݒ� (��: 120.0f �� 2��)
    public float timeLimit = 60.0f;

    // ���݂̎c�莞��
    private float currentTime;

    // ��ʂɎ��Ԃ�\�����邽�߂�UI�R���|�[�l���g
    public Text timerText; // TextMeshPro�̏ꍇ�� public TMPro.TextMeshProUGUI timerText;

    private bool isTimeUp = false; // ���Ԑ؂�t���O

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerDisplay();
    }

    void Update()
    {
        // ���Ԑ؂�Ȃ�X�V�������X�L�b�v
        if (isTimeUp)
        {
            return;
        }

        // **Time.deltaTime**�i�O�t���[������̌o�ߎ��ԁj���g���Đ��m�Ɏ��Ԃ����炷
        currentTime -= Time.deltaTime;

        UpdateTimerDisplay();

        // �c�莞�Ԃ�0�ȉ��ɂȂ�����
        if (currentTime <= 0)
        {
            currentTime = 0;
            isTimeUp = true;
            TimeUpAction(); // ���Ԑ؂ꎞ�̏��������s
        }
    }

    // UI�̕\�����X�V���鏈��
    void UpdateTimerDisplay()
    {
        // �c�莞�Ԃ�b�P�ʂ̐����ɕϊ����A"�c�莞��: 59" �̌`���ŕ\��
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = "�c�莞��: " + seconds.ToString("D2"); // D2 ��2���\���i��: 05�j
    }

    // ���Ԑ؂ꎞ�̏����i�Q�[���I�[�o�[�A�V�[���ړ��Ȃǁj
    void TimeUpAction()
    {
        Debug.Log("�������ԏI���I�Q�[���I�[�o�[�ł��B");
        // ��: SceneManager.LoadScene("GameOverScene");
    }
}