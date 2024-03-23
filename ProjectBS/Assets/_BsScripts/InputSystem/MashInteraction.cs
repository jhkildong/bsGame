using UnityEngine;
using UnityEngine.InputSystem;

public class MashInteraction : IInputInteraction
{
    // �� ���� �ִ� ��� �ð� ���� [s]
    public float tapDelay;
    // �Է� ������ �ּҰ�
    public float pressPoint;

    // ��Ÿ�� Ƚ��
    public int TapCount { get; private set; }

    // �������̳� �⺻���� �����ϴ� �ʵ�
    private float PressPointOrDefault => pressPoint > 0 ? pressPoint : InputSystem.settings.defaultButtonPressPoint;
    private float ReleasePointOrDefault => PressPointOrDefault * InputSystem.settings.buttonReleaseThreshold;
    private float TapDelayOrDefault => tapDelay > 0 ? tapDelay : InputSystem.settings.multiTapDelayTime;

    // �Է� �ܰ�
    private enum ButtonPhase
    {
        None,
        WaitingForNextRelease,
        WaitingForNextPress,
    }

    private ButtonPhase _currentButtonPhase;
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

        switch (_currentButtonPhase)
        {
            //�Է��� ���ٰ� ������ ���(started)
            case ButtonPhase.None:
                //�Է� ����
                if (context.ControlIsActuated(PressPointOrDefault))
                {
                    //Release ��� ���·� ��ȯ
                    _currentButtonPhase = ButtonPhase.WaitingForNextRelease;
                    //Started �ܰ�
                    context.Started();

                    //���� �Է��� ���� �����ִ� ���
                    if (_prevRemainInput)
                    {
                        //Performed �ܰ�� ����
                        context.PerformedAndStayPerformed();
                    }
                    _prevRemainInput = true;
                }
                break;

            case ButtonPhase.WaitingForNextRelease:
                if (!context.ControlIsActuated(ReleasePointOrDefault))
                {
                    //��ư�� ���� ������ ���
                    _currentButtonPhase = ButtonPhase.WaitingForNextPress;
                    //Ÿ�� �ƿ� ����
                    context.SetTimeout(TapDelayOrDefault);
                }
                break;

            case ButtonPhase.WaitingForNextPress:
                if (context.ControlIsActuated(PressPointOrDefault))
                {
                    //���߸� �� ������ ���
                    _currentButtonPhase = ButtonPhase.WaitingForNextRelease;

                    // �ʿ� �� ȸ�� �̻� �ǵǸ� ��Ÿ �������� �Ѵ�
                    if (_prevRemainInput)
                    {
                        // Performed �ܰ�� ����
                        context.PerformedAndStayPerformed();
                    }

                    // Ÿ�Ӿƿ� ����
                    context.SetTimeout(TapDelayOrDefault);
                }

                break;
        }
    }

    public void Reset()
    {
        _currentButtonPhase = ButtonPhase.None;
        _prevRemainInput = false;
    }
}