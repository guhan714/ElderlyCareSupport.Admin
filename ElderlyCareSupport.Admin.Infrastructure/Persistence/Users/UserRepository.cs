﻿using Dapper;
using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;
using ElderlyCareSupport.Admin.SQL.SqlQueries;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence.Users;

public class UserRepository : IUserRepository
{
    
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<PagedResoonse<User>> GetAllUsersAsync(UserQueryParameters userQueryParameters)
    {
        using var connection = _dbConnectionFactory.GetConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM ElderCareAccount;");
        var userQuery = GetConfigurationForUserQuery(userQueryParameters);
        var users = await connection.QueryAsync<User>(userQuery.Item1, userQuery.Item2);
        return new PagedResoonse<User>(users.ToList(), totalCount, userQueryParameters.pageNumber,  userQueryParameters.pageSize);
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        using var connection = _dbConnectionFactory.GetConnection();
        var userDetails = await connection.QuerySingleOrDefaultAsync<User>(UserQueries.GetUserById, new {Email = userId});
        return userDetails;
    }

    public async Task<Tuple<User, bool>> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> UpdateUserAsync(string userId, User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User> AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    private Tuple<string, object> GetConfigurationForUserQuery(UserQueryParameters userQueryParameters)
    {
        var sanitizedSortBy = userQueryParameters.SortBy switch
        {
            "FirstName" => "FirstName",
            "LastName" => "LastName",
            "Email" => "Email",
            _ => "FirstName" 
        };
        var sortOrder = userQueryParameters.Ascending ? "ASC" : "DESC";
        var offSet = userQueryParameters.pageNumber * (userQueryParameters.pageNumber - 1);
        var query = string.Format(UserQueries.GetAllUsersWithPaging, sanitizedSortBy, sortOrder);
        var parameters = new  { Search = userQueryParameters.SearchTerm, offSet = offSet, pageSize = userQueryParameters.pageSize };
        
        return Tuple.Create(query, parameters as object);
    }
}