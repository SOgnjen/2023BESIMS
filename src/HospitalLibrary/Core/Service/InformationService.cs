using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public class InformationService : IInformationService
    {
        private readonly IInformationRepository _informationRepository;

        public InformationService(IInformationRepository informationRepository)
        {
            _informationRepository = informationRepository;
        }

        public void AcceptInformation(Information information)
        {
            _informationRepository.AcceptInformation(information);
        }

        public void Create(Information information)
        {
            _informationRepository.Create(information);
        }

        public void DeclineInformation(Information information)
        {
            _informationRepository.DeclineInformation(information);
        }

        public void Delete(Information information)
        {
            _informationRepository.Delete(information);
        }

        public IEnumerable<Information> GetAll()
        {
            return _informationRepository.GetAll();
        }

        public IEnumerable<Information> GetAllAccepted()
        {
            return _informationRepository.GetAllAccepted();
        }

        public IEnumerable<Information> GetAllWaiting()
        {
            return _informationRepository.GetAllWaiting();
        }

        public Information GetById(int id)
        {
            return _informationRepository.GetById(id);
        }

        public void Update(Information information)
        {
            _informationRepository.Update(information);
        }
    }
}
