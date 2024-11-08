﻿using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void BlockUser(User user)
        {
            _userRepository.BlockUser(user);
        }

        public void Create(User user)
        {
            _userRepository.Create(user);
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }

        public User FindRequiredLoginUser(string email, string password)
        {
            return _userRepository.FindUserByEmailAndPassword(email, password);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public IEnumerable<User> GetAllBadUsers()
        {
            return _userRepository.GetAllBadUsers();
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User GetByJmbg(int userJmbg)
        {
            return _userRepository.GetByJmbg(userJmbg);
        }

        public IEnumerable<User> GetUsersBasedOnGuidance(GuidanceTo guidance)
        {
            return _userRepository.GetUsersBasedOnGuidance(guidance);
        }

        public void SetGuidanceToNone(User user)
        {
            _userRepository.SetGuidanceToNone(user);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}
