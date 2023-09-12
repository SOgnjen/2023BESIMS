package rs.ac.uns.ftn.grpcdemo;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;
import rs.ac.uns.ftn.grpcdemo.model.BloodAppointment;
import rs.ac.uns.ftn.grpcdemo.model.Information;
import rs.ac.uns.ftn.grpcdemo.model.InformationStatus;
import rs.ac.uns.ftn.grpcdemo.repository.BloodAppointmentRepository;
import rs.ac.uns.ftn.grpcdemo.repository.InformationRepository;

import java.time.LocalDate;

@EnableScheduling
@SpringBootApplication
public class GrpcDemoApplication implements CommandLineRunner {
    @Autowired
    private InformationRepository informationRepository;

    @Autowired
    private BloodAppointmentRepository bloodAppointmentRepository;

    public static void main(String[] args) {
        SpringApplication.run(GrpcDemoApplication.class, args);
    }

    @Override
    public void run(String... args) throws Exception {
        Information first = new Information(LocalDate.of(2023, 9, 15), InformationStatus.Accepted);
        Information second = new Information(LocalDate.of(2023, 9, 20), InformationStatus.Accepted);
        Information third = new Information(LocalDate.of(2023, 9, 25), InformationStatus.Waiting);
        Information forth = new Information(LocalDate.of(2023, 10, 5), InformationStatus.Waiting);
        Information fifth = new Information(LocalDate.of(2023, 10, 10), InformationStatus.Waiting);
        Information sixth = new Information(LocalDate.of(2023, 10, 15), InformationStatus.Waiting);

        informationRepository.save(first);
        informationRepository.save(second);
        informationRepository.save(third);
        informationRepository.save(forth);
        informationRepository.save(fifth);
        informationRepository.save(sixth);

        BloodAppointment bloodAppointment1 = new BloodAppointment(LocalDate.of(2023, 9, 16), false, 1234567890);
        BloodAppointment bloodAppointment2 = new BloodAppointment(LocalDate.of(2023, 9, 22), false, 1234567890);
        BloodAppointment bloodAppointment3 = new BloodAppointment(LocalDate.of(2023, 9, 26), false, 1234567890);
        BloodAppointment bloodAppointment4 = new BloodAppointment(LocalDate.of(2023, 10, 6), true, 0);
        BloodAppointment bloodAppointment5 = new BloodAppointment(LocalDate.of(2023, 10, 11), true, 0);
        BloodAppointment bloodAppointment6 = new BloodAppointment(LocalDate.of(2023, 10, 16), true, 0);

        bloodAppointmentRepository.save(bloodAppointment1);
        bloodAppointmentRepository.save(bloodAppointment2);
        bloodAppointmentRepository.save(bloodAppointment3);
        bloodAppointmentRepository.save(bloodAppointment4);
        bloodAppointmentRepository.save(bloodAppointment5);
        bloodAppointmentRepository.save(bloodAppointment6);
    }
}
