package rs.ac.uns.ftn.grpcdemo;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;
import rs.ac.uns.ftn.grpcdemo.model.Information;
import rs.ac.uns.ftn.grpcdemo.model.InformationStatus;
import rs.ac.uns.ftn.grpcdemo.repository.InformationRepository;

import java.time.LocalDate;

@EnableScheduling
@SpringBootApplication
public class GrpcDemoApplication implements CommandLineRunner {
    @Autowired
    private InformationRepository informationRepository;

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
    }
}
