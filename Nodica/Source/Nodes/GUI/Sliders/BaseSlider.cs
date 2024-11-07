namespace Nodica;

public abstract partial class BaseSlider : ClickableRectangle
{
    #region [ - - - Properties & Fields - - - ]

    public float InitialPercentage { get; set; } = -1;
    public float DefaultPercentage { get; set; } = 0;
    public float MaxExternalValue { get; set; } = 0;
    public bool HasButtons { get; set; } = true;
    public bool ResetOnRitghtClick { get; set; } = true;
    public BoxTheme FilledStyle { get; set; } = new();
    public BoxTheme EmptyStyle { get; set; } = new();

    public BaseGrabber Grabber;

    public Action<BaseSlider> OnUpdate = (slider) => { };

    public event EventHandler<float>? PercentageChanged;
    public event EventHandler<float>? Released;

    protected bool wasPressed = false;

    private bool initialPercentageSet = false;

    public float Value => MathF.Ceiling(Percentage * MaxExternalValue);

    private float _percentage = 0;
    public float Percentage
    {
        get => _percentage;

        set
        {
            _percentage = Math.Clamp(value, 0, 1);
            MoveGrabberToPercentage(_percentage);
        }
    }

    private float _externalValue = 0;
    public float ExternalValue
    {
        get => _externalValue;

        set
        {
            if (_externalValue != value)
            {
                _externalValue = value;

                //float minPos = GlobalPosition.Y - Offset.Y;
                //float maxPos = minPos + Size.Y;

                //float x = Grabber.GlobalPosition.X;
                //float y = ExternalValue / MaxExternalValue * maxPos;

                //Grabber.GlobalPosition = new(x, y);
            }
        }
    }

    #endregion

    public BaseSlider()
    {
        EmptyStyle.FillColor = DefaultTheme.SliderEmptyFill;
        FilledStyle.FillColor = DefaultTheme.Accent;
    }

    public override void Start()
    {
        Grabber = GetNode<BaseGrabber>("Grabber");
        Grabber.Layer = Layer;

        var decrementButton = GetNode<Button>("DecrementButton");
        var incrementButton = GetNode<Button>("IncrementButton");

        if (!HasButtons)
        {
            decrementButton.Deactivate();
            incrementButton.Deactivate();
        }
        else
        {
            decrementButton.LeftClicked += OnDecrementButtonLeftClicked;
            incrementButton.LeftClicked += OnIncrementButtonLeftClicked;

            decrementButton.Layer = Layer;
            incrementButton.Layer = Layer;
        }

        SizeChanged += OnSizeChanged;

        base.Start();
    }

    private void OnSizeChanged(object? sender, Vector2 e)
    {
        MoveGrabberToPercentage(Percentage);
    }

    public override void Update()
    {
        OnUpdate(this);
        UpdatePercentage();
        HandleClicks();
        Draw();
        SetInitialPercentage();
        base.Update();
    }

    public void UpdatePercentageBasedOnGrabber()
    {
        float previousValue = Percentage;

        UpdatePercentage();

        if (Percentage != previousValue)
        {
            OnPercentageChanged();
        }

        if (wasPressed && !Grabber.Pressed)
        {
            OnReleased();
        }

        wasPressed = Grabber.Pressed;
    }

    public abstract void MoveGrabber(int direction);

    private void OnDecrementButtonLeftClicked(object? sender, EventArgs e)
    {
        MoveGrabber(-1);
    }

    private void OnIncrementButtonLeftClicked(object? sender, EventArgs e)
    {
        MoveGrabber(1);
    }

    protected abstract void UpdatePercentage();

    protected abstract void HandleClicks();

    protected abstract void MoveGrabberToPercentage(float percentage);

    protected void OnPercentageChanged()
    {
        PercentageChanged?.Invoke(this, Percentage);
    }

    protected void OnReleased()
    {
        Released?.Invoke(this, Percentage);
    }

    protected void RevertToDefaultPercentage()
    {
        Percentage = DefaultPercentage;
    }

    private void SetInitialPercentage()
    {
        if (initialPercentageSet)
        {
            return;
        }

        if (InitialPercentage < 0)
        {
            Percentage = Percentage;
        }

        Percentage = InitialPercentage;
        initialPercentageSet = true;
    }
}