﻿using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; protected set; }
    public string Token { get; protected set; }
    public DateTime ExpirationTime { get; protected set; }
    public bool IsExpired => DateTime.UtcNow >= ExpirationTime;



    public RefreshToken(Guid id, Guid userId, string token, DateTime expirationTime) : base(id)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpirationTime = expirationTime;
    }
}