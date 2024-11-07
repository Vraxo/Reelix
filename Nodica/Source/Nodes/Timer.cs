namespace Nodica;

public class Timer : Node
{
    public float TimePassed { get; private set; } = 0;
    public bool AutoStart { get; set; } = false;
    public bool Loop { get; set; } = false;

    private float waitTime = 1.0f;
    private bool fired = false;

    public event EventHandler? TimedOut;

    public override void Start()
    {
        if (AutoStart)
        {
            Fire(waitTime);
        }
    }

    public override void Update()
    {
        if (fired)
        {
            TimePassed += Time.DeltaTime;

            if (TimePassed >= waitTime)
            {
                fired = false;
                TimePassed = 0;

                TimedOut?.Invoke(this, EventArgs.Empty);

                if (Loop)
                {
                    Fire(waitTime);
                }
            }
        }
    }

    public void Fire(float waitTime)
    {
        this.waitTime = waitTime;
        fired = true;
        TimePassed = 0;
    }

    public void Stop()
    {
        fired = false;
    }

    public void Reset()
    {
        TimePassed = 0;
        fired = false;
    }
}