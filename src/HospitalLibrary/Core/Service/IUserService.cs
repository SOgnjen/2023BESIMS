﻿using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int it);
        void Create(User user);
        void Update(User user);
        void Delete(User user);
        User FindRequiredLoginUser(string email, string password);
        IEnumerable<User> GetUsersBasedOnGuidance(GuidanceTo guidance);
        IEnumerable<User> GetAllBadUsers();
        void BlockUser(User user);
        void SetGuidanceToNone(User user);
        User GetByJmbg(int userJmbg);
    }
}
