// Simple generic proprotional-integral-derivative (PID) controller behavior

using UnityEngine;

public class PID : MonoBehaviour
{
    [Tooltip("String identifier to disambiguate from other PID components")]
    public string id = "";

    [Tooltip("Proportional error coefficient")]
    public float kP = 1.0F;

    [Tooltip("Integral error coefficient")]
    public float kI = 0.1F;

    [Tooltip("Differential error coefficient")]
    public float kD = 0.2F;

    [Tooltip("Integral clamping magnitude")]
    public float integralClamp = 10.0F;

    private float _errorIntegral = 0.0F;

    private float _errorPrevious = 0.0F;
    private bool _errorPreviousInit = false;

    // Update the PID error and get the new control output
    public float updateError(float error, float deltaTime)
    {
        // Integrate error and clamp if needed
        _errorIntegral += (error * deltaTime);
        float errorSign = Mathf.Sign(error); 
        if (Mathf.Abs(_errorIntegral) > Mathf.Abs(integralClamp)) {
            _errorIntegral = (integralClamp * errorSign);
        }

        // Compute simple error delta if a previous error was provided
        float errorDelta = 0.0F;
        if (_errorPreviousInit) {
            errorDelta = (error - _errorPrevious) / deltaTime;
        } else {
            _errorPreviousInit = true;
        }

        _errorPrevious = error;

        return (error * kP) + (_errorIntegral * kI) + (errorDelta * kD);
    }

    // Reset the error integral and clear previous init flag
    public void resetError()
    {
        _errorIntegral = 0.0F;
        _errorPreviousInit = false;
    }

    // Reset the error integral and set the previous error to the provided value
    public void resetError(float error)
    {
        _errorIntegral = 0.0F;
        _errorPreviousInit = true;
        _errorPrevious = error;
    }
}
