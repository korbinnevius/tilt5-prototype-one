using UnityEngine;

public static class Utility
{
	private static Vector3 AdjustInputAxisToAlignWithForward(Vector2 input, Vector3 facingDir)
	{
		float facing = Quaternion.Euler(facingDir).eulerAngles.y;
		return (Quaternion.Euler(0, facing, 0) * input);
	}

	public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
	{
		if (Quaternion.Dot(a, b) < 0)
		{
			return a * Quaternion.Inverse(Multiply(b, -1));
		}

		else return a * Quaternion.Inverse(b);
	}

	public static Quaternion Multiply(Quaternion input, float scalar)
	{
		return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
	}

	public static float RoundAndNormalizeDegrees360(this float input)
	{
		//normalize to range
		while (input < 0)
		{
			input += 360;
		}

		while (input >= 360)
		{
			input -= 360;
		}

		input = Mathf.Round(input);
		return input;
	}
}
