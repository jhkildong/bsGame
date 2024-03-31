using UnityEngine;
using UnityEngine.InputSystem;

public class MashInteraction : IInputInteraction
{
    // 각 탭의 최대 허용 시간 간격 [s]
    public float tapDelay;
    // 입력 판정의 최소값
    public float pressPoint;

    private float TapDelayOrDefault => tapDelay > 0 ? tapDelay : InputSystem.settings.multiTapDelayTime;
    
    //이전 입력 확인
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

        // 입력이 시작되었는지 확인
        if (context.ControlIsActuated())
        {
            if(!_prevRemainInput)
            {
                // Started 단계
                context.Started();
                _prevRemainInput = true;
            }
            // Performed 단계로 이행
            context.PerformedAndStayPerformed();
        }
        else if (!context.ControlIsActuated())
        {
            // 타임 아웃 설정
            context.SetTimeout(TapDelayOrDefault);
        }
    }

    public void Reset()
    {
        _prevRemainInput = false;
    }
}