using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public interface IInformationService
    {
        IEnumerable<Information> GetAll();
        Information GetById(int id);
        void Create(Information information);
        void Update(Information information);
        void Delete(Information information);
        IEnumerable<Information> GetAllAccepted();
        void AcceptInformation(Information information);
        void DeclineInformation(Information information);
        IEnumerable<Information> GetAllWaiting();
    }
}
