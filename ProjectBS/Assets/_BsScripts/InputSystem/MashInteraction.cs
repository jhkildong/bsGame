using UnityEngine;
using UnityEngine.InputSystem;

public class MashInteraction : IInputInteraction
{
    // �� ���� �ִ� ��� �ð� ���� [s]
    public float tapDelay;
    // �Է� ������ �ּҰ�
    public float pressPoint;

    private float TapDelayOrDefault => tapDelay > 0 ? tapDelay : InputSystem.settings.multiTapDelayTime;
    
    //���� �Է� Ȯ��
    private bool _prevRemainInput;

    /// /// <summary>
    /// �ʱ�ȭ
    /// /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void Initialize()
    {
        InputSystem.RegisterInteraction<MashInteraction>();
    }

    public void Process(ref InputInteractionContext context)
    {
        // Ÿ�Ӿƿ� üũ
        if (context.timerHasExpired)
        {
            // �ִ� ��� �ð� �̻� �Է� ��ȭ�� ������ ����
            context.Canceled();
            return;
        }

        // �Է��� ���۵Ǿ����� Ȯ��
        if (context.ControlIsActuated())
        {
            if(!_prevRemainInput)
            {
                // Started �ܰ�
                context.Started();
                _prevRemainInput = true;
            }
            // Performed �ܰ�� ����
            context.PerformedAndStayPerformed();
        }
        else if (!context.ControlIsActuated())
        {
            // Ÿ�� �ƿ� ����
            context.SetTimeout(TapDelayOrDefault);
        }
    }

    public void Reset()
    {
        _prevRemainInput = false;
    }
}