using Dapper;
using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;
using ElderlyCareSupport.Admin.Domain.ValueObjects;
using ElderlyCareSupport.Admin.SQL.SqlQueries;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence.Users;

public class TaskRepository : ITaskRepository
{
    private readonly IDbConnectionFactory _connection;
 
    public TaskRepository(IDbConnectionFactory connection)
    {
        _connection = connection;
    }

    public async Task<PagedResponse<TaskDetails>> GetAllTaskDetails(PageQueryParameters taskQueryParameters)
    {
        var connectionSession = _connection.GetConnection();
        var total = await connectionSession.ExecuteScalarAsync<int>("SELECT COUNT(TaskId) FROM Task;");
        var (configuration, parameters) = ConfigureQuery(taskQueryParameters);
        var result =
            await connectionSession.QueryAsync<TaskDetails, VolunteerDetails, TaskDetails>(
                configuration,
                (taskDetails, volunteerDetails) =>
                {
                    taskDetails.VolunteerAccount = volunteerDetails;
                    return taskDetails;
                },
                new { parameters }, splitOn: "VolunteerAccount");

        return new PagedResponse<TaskDetails>(result.ToList(), total, taskQueryParameters.pageNumber,
            taskQueryParameters.pageSize);
    }

    public async Task<TaskDetails?> GetTaskDetails(long id)
    {
        using var connection = _connection.GetConnection();
        var result =
            await connection.QueryAsync<TaskDetails>("SELECT * FROM Task WHERE TaskId = @Id;", new { Id = id });
        return result.FirstOrDefault();
    }


    private static Tuple<string, object> ConfigureQuery(PageQueryParameters pageQueryParameters)
    {
        var orderField = pageQueryParameters.SortBy;
        var orderBy = pageQueryParameters.Ascending ? "ASC" : "DESC";
        var offSet = pageQueryParameters.pageSize * (pageQueryParameters.pageNumber - 1);
        var taskQuery = string.Format(TaskQueries.GetAllTaskDetails, orderField, orderBy);

        var parameters = new
        {
            @SearchTerm = pageQueryParameters.SearchTerm,
            @offSet = offSet,
            @PageSize = pageQueryParameters.pageSize,
            @SearchValue = "Sample"
        };

        return Tuple.Create(taskQuery, parameters as object);
    }


    public async Task<Tuple<bool, long>> CancelTaskAsync(long id)
    {
        using var connection = _connection.GetConnection();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteScalarAsync<int>("UPDATE Task SET TaskStatusId = 1 WHERE TaskId = @Id;",
            new { Id = id });
        if (result == 0)
        {
            transaction.Rollback();
            return Tuple.Create(false, id);
        }
        
        transaction.Commit();
        return Tuple.Create(true, id);
    }
}