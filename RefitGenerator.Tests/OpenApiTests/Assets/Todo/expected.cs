using System;
using Refit;

public interface ITodoApiService
{
    [Get("/todos")]
    public Task<ListTodosResponse> ListTodos(ListTodosQueries queries);
}


public enum Status
{
    DONE,
    WAITING,
    WORKING,
    ALL
}

public class ListTodosQueries
{
    public Status Status { get; set; }
}

public class ListTodosResponse
{
    public List<Todo> Items { get; set; }
}

public class Todo
{
    /// <summary>
    /// The todo identifier
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The todo title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The todo creation date
    /// </summary>
    public DateTime Create_date { get; set; }

    /// <summary>
    /// The todo resolution date
    /// </summary>
    public DateTime Done_date { get; set; }

    /// <summary>
    /// The todo state
    /// </summary>
    public Status Status { get; set; }
}