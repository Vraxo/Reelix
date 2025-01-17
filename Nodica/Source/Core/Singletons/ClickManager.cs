﻿using Raylib_cs;

namespace Nodica;

public class ClickManager
{
    public static ClickManager Instance => instance ??= new();
    private static ClickManager? instance;

    public int MinLayer = -1;

    private List<Clickable> clickables = [];

    private ClickManager() { }

    public void Register(Clickable clickable)
    {
        clickables.Add(clickable);
    }

    public void Unregister(Clickable clickable)
    {
        clickables.Remove(clickable);
    }

    public void Update()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            SignalClick(MouseButton.Left);
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            SignalClick(MouseButton.Right);
        }
    }

    public int GetHighestLayer()
    {
        int highestLayer = MinLayer;

        foreach (var clickable in clickables)
        {
            if (clickable.Layer > highestLayer)
            {
                highestLayer = clickable.Layer;
            }
        }

        return highestLayer;
    }

    private void SignalClick(MouseButton mouseButton)
    {
        List<Clickable> viableClickables = GetViableClickables();

        if (viableClickables.Count > 0)
        {
            Clickable? topClickable = GetTopClickable(viableClickables);

            if (topClickable != null)
            {
                if (mouseButton == MouseButton.Left)
                {
                    topClickable.OnTopLeft = true;
                    //Console.WriteLine("on top left set to true " + topClickable.Name);
                }
                else
                {
                    topClickable.OnTopRight = true;
                    //Console.WriteLine("on top right set to true");
                }
            }
        }
    }

    private List<Clickable> GetViableClickables()
    {
        List<Clickable> viableClickables = [];

        foreach (Clickable clickable in clickables)
        {
            if (clickable.IsMouseOver())
            {
                viableClickables.Add(clickable);
            }
        }

        //Console.WriteLine("clickables: " + viableClickables.Count);

        return viableClickables;
    }

    private Clickable? GetTopClickable(List<Clickable> viableClickables)
    {
        Clickable? topClickable = null;
        int highestLayer = MinLayer;

        foreach (Clickable clickable in viableClickables)
        {
            if (clickable.Layer >= highestLayer)
            {
                highestLayer = clickable.Layer;
                topClickable = clickable;
            }
        }

        //Console.WriteLine("highest layer: " + highestLayer);

        return topClickable;
    }
}