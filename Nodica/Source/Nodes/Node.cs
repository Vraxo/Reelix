namespace Nodica;

/// <summary>
/// Represents a basic node in a scene tree with support for children, activation, and destruction.
/// </summary>
public class Node
{
    public string Name { get; set; } = "";
    public Node? Parent { get; set; } = null;
    public List<Node> Children { get; set; } = [];
    public bool Active { get; private set; } = true;

    /// <summary>Gets the root node of the application scene tree.</summary>
    public Node RootNode => App.Instance.RootNode;

    /// <summary>
    /// Gets the absolute path to this node, starting with "/root/".
    /// </summary>
    public string AbsolutePath
    {
        get
        {
            string path = Name;
            Node? current = Parent;
            bool isRootNode = true;

            while (current != null)
            {
                if (!isRootNode)
                {
                    path = $"{current.Name}/{path}";
                }
                else
                {
                    isRootNode = false;
                }
                current = current.Parent;
            }

            return $"/root/{path}";
        }
    }

    private bool started = false;

    // Public

    /// <summary>Performs setup actions before starting the node.</summary>
    public virtual void Build() { }

    /// <summary>Initializes the node when added to the scene.</summary>
    public virtual void Start() { }

    /// <summary>Called once when the node becomes active.</summary>
    public virtual void Ready() { }

    /// <summary>Updates the node on each frame while active.</summary>
    public virtual void Update() { }

    /// <summary>Recursively destroys this node and its children, removing it from the parent's children.</summary>
    public virtual void Destroy()
    {
        List<Node> childrenToDestroy = new(Children);

        foreach (Node child in childrenToDestroy)
        {
            child.Destroy();
        }

        Parent?.Children.Remove(this);
    }

    /// <summary>Processes the node and its children, handling initialization and updating.</summary>
    public void Process()
    {
        if (!Active)
        {
            return;
        }

        if (!started)
        {
            Ready();
            started = true;
        }

        Update();

        for (int i = 0; i < Children.Count; i++)
        {
            Children[i].Process();
        }
    }

    /// <summary>Prints the node and all its children in a tree structure.</summary>
    public void PrintChildren(string indent = "")
    {
        //Console.WriteLine(indent + "-" + Name);

        //foreach (var child in Children)
        //{
        //    child.PrintChildren(indent + "-");
        //}

        Console.WriteLine(AbsolutePath);

        foreach (var child in Children)
        {
            child.PrintChildren();
        }
    }

    // (De)Activation

    /// <summary>Activates the node and recursively activates all its children.</summary>
    public virtual void Activate()
    {
        Active = true;

        foreach (Node child in Children)
        {
            child.Activate();
        }
    }

    /// <summary>Deactivates the node and recursively deactivates all its children.</summary>
    public virtual void Deactivate()
    {
        Active = false;

        foreach (Node child in Children)
        {
            child.Deactivate();
        }
    }

    // Get special nodes

    /// <summary>
    /// Returns the parent node cast to type <typeparamref name="T"/> if available, otherwise returns the current node.
    /// </summary>
    public T? GetParent<T>() where T : Node
    {
        if (Parent != null)
        {
            return (T)Parent;
        }

        return (T)this;
    }

    // Get node from the root

    /// <summary>
    /// Retrieves a node of type <typeparamref name="T"/> from the scene tree based on a specified path.
    /// </summary>
    /// <typeparam name="T">The expected type of the node to be returned.</typeparam>
    /// <param name="path">
    /// The path to the target node, which can be either absolute (starting with "/root") or relative.
    /// </param>
    public T? GetNode<T>(string path) where T : Node
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        if (path.StartsWith("/root"))
        {
            path = path.Substring("/root".Length);
            Node currentNode = App.Instance.RootNode;

            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            if (!string.IsNullOrEmpty(path))
            {
                string[] nodeNames = path.Split('/');
                foreach (var name in nodeNames)
                {
                    currentNode = currentNode.GetChild(name);

                    if (currentNode == null)
                    {
                        return null;
                    }
                }
            }

            return currentNode as T;
        }
        else
        {
            Node currentNode = this;
            string[] nodeNames = path.Split('/');
            foreach (var name in nodeNames)
            {
                if (name == "")
                {
                    return currentNode as T;
                }

                currentNode = currentNode.GetChild(name);
                if (currentNode == null)
                {
                    return null;
                }
            }

            return currentNode as T;
        }
    }


    /// <summary>
    /// Returns a child node by name if it exists, cast to <typeparamref name="T"/>.
    /// </summary>
    public T? GetChild<T>(string name) where T : Node
    {
        foreach (Node child in Children)
        {
            if (child.Name == name)
            {
                return (T)child;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the first child node matching type <typeparamref name="T"/>, if it exists.
    /// </summary>
    public T? GetChild<T>() where T : Node
    {
        foreach (Node child in Children)
        {
            if (child.GetType() == typeof(T))
            {
                return (T)child;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns a child node by name if it exists.
    /// </summary>
    public Node? GetChild(string name)
    {
        foreach (Node child in Children)
        {
            if (child.Name == name)
            {
                return child;
            }
        }

        return null;
    }

    // AddItem child

    /// <summary>
    /// Adds a child node with a specified name and optionally starts it.
    /// </summary>
    public Node AddChild(Node node, string name, bool start = true)
    {
        node.Name = name;
        node.Parent = this;

        node.Build();

        if (start)
        {
            node.Start();
        }

        Children.Add(node);

        return node;
    }

    /// <summary>
    /// Adds a child node using its type name and optionally starts it.
    /// </summary>
    public Node AddChild(Node node, bool start = true)
    {
        node.Name = node.GetType().Name;
        node.Parent = this;

        node.Build();

        if (start)
        {
            node.Start();
        }

        Children.Add(node);

        return node;
    }

    // Change scene

    /// <summary>
    /// Replaces the current scene with a new root node.
    /// </summary>
    public void ChangeScene(Node node)
    {
        //App.ResetView();
        App.Instance.RootNode.Destroy();
        App.Instance.RootNode = node;

        node.Name = node.GetType().Name;
    }
}