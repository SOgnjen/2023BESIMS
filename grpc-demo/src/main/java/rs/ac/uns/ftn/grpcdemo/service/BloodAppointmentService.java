package rs.ac.uns.ftn.grpcdemo.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import rs.ac.uns.ftn.grpcdemo.model.BloodAppointment;
import rs.ac.uns.ftn.grpcdemo.repository.BloodAppointmentRepository;

import java.util.List;

@Service
public class BloodAppointmentService {
    @Autowired
    private final BloodAppointmentRepository bloodAppointmentRepository;

    public BloodAppointmentService(BloodAppointmentRepository bloodAppointmentRepository){
        this.bloodAppointmentRepository = bloodAppointmentRepository;
    }

    public List<BloodAppointment> getAllFree() {
        return bloodAppointmentRepository.findAllByIsFree(true);
    }
}
