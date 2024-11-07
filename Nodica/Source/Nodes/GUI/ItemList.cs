using Raylib_cs;

namespace Nodica;

public class ItemList : ClickableRectangle
{
    public static readonly Vector2 DefaultSize = new(250, 150);

    public List<Node2D> Items = [];
    public Vector2 ItemSize = new(100, 20);
    public int SliderButtonLayer = 0;
    public VerticalSlider Slider;
    public Action<ItemList> OnUpdate = (list) => { };
    public Action<ItemList> OnItemCountChanged = (list) => { };

    public event EventHandler<int>? StartingIndexChanged;

    private int maxItemsShownAtOnce = 0;
    private int updateCount = 0;

    private int _startingIndex = 0;
    public int StartingIndex
    {
        get => _startingIndex;

        set
        {
            if (value < 0)
            {
                _startingIndex = 0;
            }
            else if (value > Items.Count - maxItemsShownAtOnce)
            {
                _startingIndex = Math.Max(0, Items.Count - maxItemsShownAtOnce);
            }
            else
            {
                _startingIndex = value;
            }
        }
    }

    public ItemList()
    {
        Size = DefaultSize;
        OriginPreset = OriginPreset.TopLeft;
    }

    public override void Build()
    {
        AddChild(new VerticalSlider
        {
            OnUpdate = (slider) =>
            {
                float x = Size.X - Offset.X - slider.Size.X - slider.Offset.X;
                float y = -Offset.Y + slider.Grabber.Size.Y * 2.5F; // *4

                slider.Position = new(x, y);

                //slider.position.Y = slider.Grabber.Radius * 2.5F;

                slider.Size = new(slider.Size.X, Size.Y - slider.Grabber.Size.Y * 5); // * 8

                int numItemsBesidesThisPage = Items.Count - maxItemsShownAtOnce;

                slider.MaxExternalValue = numItemsBesidesThisPage > 0 ?
                                          numItemsBesidesThisPage :
                                          0;

                slider.ExternalValue = StartingIndex;
            }
        }, "Slider");
    }

    public override void Start()
    {
        Slider = GetNode<VerticalSlider>("Slider");
        Slider.PercentageChanged += OnSliderPercentageChanged;
        Slider.Layer = Layer + 1;
        Slider.GetNode<Button>("DecrementButton").Layer = Layer + 1;
        Slider.GetNode<Button>("IncrementButton").Layer = Layer + 1;

        UpdateList(StartingIndex);

        base.Start();
    }

    public override void Update()
    {
        OnUpdate(this);
        HandleScrolling();
        UpdateList(StartingIndex);
        base.Update();
    }

    public void AddItem(Node2D item)
    {
        item.InheritOrigin = true;
        Items.Add(item);
        AddChild(item);
        item.Layer = Layer;
        OnItemCountChanged(this);
    }

    public void RemoveItem(Node2D item)
    {
        Items.Remove(item);
        Children.Remove(item);
        item.Destroy();
        OnItemCountChanged(this);
    }

    public void Remove(int index)
    {
        Node2D item = Items[index];
        RemoveItem(item);
    }

    public void ClearItems()
    {
        while (Items.Count > 0)
        {
            Remove(0);
        }
    }

    private void OnSliderPercentageChanged(object? sender, float e)
    {
        //int newStartingIndex = GetStartingIndexBasedOnSliderValue(e);
        //int newStartingIndex = (int)(sender as VerticalSlider).Value;
        //StartingIndex = newStartingIndex;

        int numItemsBesidesThisPage = Items.Count - maxItemsShownAtOnce;
        StartingIndex = (int)(numItemsBesidesThisPage * e);
    }

    private void MinimizeStartingIndex()
    {
        while (StartingIndex > Math.Max(0, Items.Count - maxItemsShownAtOnce))
        {
            StartingIndex--;
        }
    }

    private void UpdateMaxItemsShownAtOnce()
    {
        maxItemsShownAtOnce = (int)(Size.Y / ItemSize.Y);
    }

    private int GetStartingIndexBasedOnSliderValue(float sliderValue)
    {
        int numItemsBesidesThisPage = Items.Count - maxItemsShownAtOnce;
        return (int)Math.Floor(sliderValue * numItemsBesidesThisPage);
    }

    private void UpdateItemsActivationAndPosition()
    {
        if (updateCount != 2)
        {
            updateCount++;
            return;
        }

        int j = 0;

        for (int i = 0; i < Items.Count; i++)
        {
            if (i >= StartingIndex && i < StartingIndex + maxItemsShownAtOnce)
            {
                Items[i].Activate();
                //Items[i].position.Y = ItemSize.Y * j;
                Items[i].Position = new(Items[i].Position.X, ItemSize.Y * j);
                j++;
            }
            else
            {
                Items[i].Position = new(Items[i].Position.X, -1000);
                //Items[i].position = new(Items[i].position.X, ItemSize.Y * j);
                Items[i].Deactivate();
            }
        }
    }

    private void HandleScrolling()
    {
        bool isOnTop = Layer >= ClickManager.Instance.MinLayer;

        if (!IsMouseOver() || !isOnTop)
        {
            return;
        }

        float mouseWheelMovement = Raylib.GetMouseWheelMove();

        if (mouseWheelMovement > 0)
        {
            UpdateList(StartingIndex - 1);
            Slider.MoveGrabber(-1);
        }
        else if (mouseWheelMovement < 0)
        {
            UpdateList(StartingIndex + 1);
            Slider.MoveGrabber(+1);
        }
    }

    private void UpdateList(int newStartingIndex)
    {
        MinimizeStartingIndex();
        UpdateMaxItemsShownAtOnce();

        StartingIndex = newStartingIndex;

        UpdateItemsActivationAndPosition();

        StartingIndexChanged?.Invoke(this, StartingIndex);
    }
}