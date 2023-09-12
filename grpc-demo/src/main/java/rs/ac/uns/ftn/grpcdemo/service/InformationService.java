package rs.ac.uns.ftn.grpcdemo.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import rs.ac.uns.ftn.grpcdemo.model.Information;
import rs.ac.uns.ftn.grpcdemo.model.InformationStatus;
import rs.ac.uns.ftn.grpcdemo.repository.InformationRepository;

import java.util.List;

@Service
public class InformationService {
    @Autowired
    private final InformationRepository informationRepository;

    public InformationService(InformationRepository informationRepository)
    {
        this.informationRepository = informationRepository;
    }

    public List<Information> getAllAcceptedInformations() {
        return informationRepository.findByStatus(InformationStatus.Accepted);
    }

    public List<Information> getAllDeclinedInformations() {
        return informationRepository.findByStatus(InformationStatus.Declined);
    }

    public Information acceptInformation(Integer informationId) {
        Information information = informationRepository.findById(informationId).orElse(null);
        if (information != null && information.getStatus() == InformationStatus.Waiting) {
            information.setStatus(InformationStatus.Accepted);
            return informationRepository.save(information);
        }
        return null;
    }

    public Information declineInformation(Integer informationId) {
        Information information = informationRepository.findById(informationId).orElse(null);
        if (information != null && information.getStatus() == InformationStatus.Waiting) {
            information.setStatus(InformationStatus.Declined);
            return informationRepository.save(information);
        }
        return null;
    }
}
