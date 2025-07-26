﻿using LiteDB;
using schedule_bot.Entities;

namespace schedule_bot.Services;

public record CreateDefaultUser(long UserId, bool IsAdmin);
public record UpsertUser(long UserId, string? MenuJsonString);
public interface IUserRepository
{
    User GetOrCreateDefault(CreateDefaultUser dto);
    void Upsert(UpsertUser dto);
}
public class UserRepository(LiteDatabase db) : IUserRepository
{
    private readonly Lock _lock = new();
    private readonly ILiteCollection<User> _users = db.GetCollection<User>();

    public User GetOrCreateDefault(CreateDefaultUser dto)
    {
        if (!db.BeginTrans())
            throw new Exception();
        lock (_lock)
        {
            try
            {
                var (userId, isAdmin) = dto;
                var user = _users.FindById(userId);
                if (user is null)
                {
                    user = new User()
                    {
                        IsAdmin = isAdmin,
                        Id = userId,
                        Settings = new()
                        {
                            BeginningEnabled = true,
                            DeadlineEnabled = true
                        },
                        Tasks = [],
                        MenuString = "[]"
                    };
                    _users.Insert(user);
                }
                db.Commit();
                return user;
            }
            catch
            {
                db.Rollback();
                throw;
            }
        }
    }

    public void Upsert(UpsertUser dto)
    {
        if (!db.BeginTrans())
            throw new Exception();
        lock (_lock)
        {
            try
            {
                var user = _users.FindById(dto.UserId);
                if (dto.MenuJsonString is not null)
                    user.MenuString = dto.MenuJsonString;
                _users.Update(user);
                db.Commit();
            }
            catch
            {
                db.Rollback();
                throw;
            }
        }
    }
}
