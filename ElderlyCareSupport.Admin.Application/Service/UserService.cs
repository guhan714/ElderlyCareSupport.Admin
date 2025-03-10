﻿using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;
using ElderlyCareSupport.Admin.Logging.ILogger;

namespace ElderlyCareSupport.Admin.Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoggerFactory _loggerFactory;
    
    public UserService(IUserRepository userRepository, ILoggerFactory loggerFactory)
    {
        _userRepository = userRepository;
        _loggerFactory = loggerFactory;
    }

    public async Task<PagedResponse<User>> GetAllUsersAsync(PageQueryParameters? userQueryParameters)
    {
        _loggerFactory.LogInfo("GetAllUsersAsync process started");
        var users = await _userRepository.GetAllUsersAsync(userQueryParameters);
        return users;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        _loggerFactory.LogInfo("GetUserByIdAsync process started");
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user ?? new User();
    }

    public async Task<Tuple<User, bool>> AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<Tuple<User, bool>> UpdateUserAsync(User user)
    {
        _loggerFactory.LogInfo("UpdateUserAsync process started");
        var userUpdateResult = await _userRepository.UpdateUserAsync(user);
        return userUpdateResult;
    }

    public async Task<Tuple<string, bool>> DeleteUserAsync(string userId)
    {
        var result = await _userRepository.DeleteUserAsync(userId);
        return result;
    }
}