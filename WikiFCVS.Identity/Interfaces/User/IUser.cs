﻿using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace WikiFCVS.Identity.Interfaces.User
{
    public interface IUser
    {
        string Name { get;}

        Guid GetUserId();

        string GetUserEmail();

        string GetUserName();

        bool IsAuthenticated();

        bool IsInRole(string role);

        IEnumerable<Claim> GetClaimsIdentity();

    }
}
