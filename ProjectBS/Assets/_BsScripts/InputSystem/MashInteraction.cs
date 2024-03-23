using UnityEngine;
using UnityEngine.InputSystem;

public class MashInteraction : IInputInteraction
{
    // 각 탭의 최대 허용 시간 간격 [s]
    public float tapDelay;
    // 입력 판정의 최소값
    public float pressPoint;

    // 연타된 횟수
    public int TapCount { get; private set; }

    // 설정값이나 기본값을 저장하는 필드
    private float PressPointOrDefault => pressPoint > 0 ? pressPoint : InputSystem.settings.defaultButtonPressPoint;
    private float ReleasePointOrDefault => PressPointOrDefault * InputSystem.settings.buttonReleaseThreshold;
    private float TapDelayOrDefault => tapDelay > 0 ? tapDelay : InputSystem.settings.multiTapDelayTime;

    // 입력 단계
    private enum ButtonPhase
    {
        None,
        WaitingForNextRelease,
        WaitingForNextPress,
    }

    private ButtonPhase _currentButtonPhase;
    private bool _prevRemainInput;

    /// /// <summary>
    /// 초기화
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
        // 타임아웃 체크
        if (context.timerHasExpired)
        {
            // 최대 허용 시간 이상 입력 변화가 없으면 종료
            context.Canceled();
            return;
        }

        switch (_currentButtonPhase)
        {
            //입력이 없다가 들어오는 경우(started)
            case ButtonPhase.None:
                //입력 시작
                if (context.ControlIsActuated(PressPointOrDefault))
                {
                    //Release 대기 상태로 변환
                    _currentButtonPhase = ButtonPhase.WaitingForNextRelease;
                    //Started 단계
                    context.Started();

                    //이전 입력이 아직 남아있는 경우
                    if (_prevRemainInput)
                    {
                        //Performed 단계로 이행
                        context.PerformedAndStayPerformed();
                    }
                    _prevRemainInput = true;
                }
                break;

            case ButtonPhase.WaitingForNextRelease:
                if (!context.ControlIsActuated(ReleasePointOrDefault))
                {
                    //버튼을 누를 때까지 대기
                    _currentButtonPhase = ButtonPhase.WaitingForNextPress;
                    //타임 아웃 설정
                    context.SetTimeout(TapDelayOrDefault);
                }
                break;

            case ButtonPhase.WaitingForNextPress:
                if (context.ControlIsActuated(PressPointOrDefault))
                {
                    //단추를 땔 때까지 대기
                    _currentButtonPhase = ButtonPhase.WaitingForNextRelease;

                    // 필요 탭 회수 이상 탭되면 연타 판정으로 한다
                    if (_prevRemainInput)
                    {
                        // Performed 단계로 이행
                        context.PerformedAndStayPerformed();
                    }

                    // 타임아웃 설정
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