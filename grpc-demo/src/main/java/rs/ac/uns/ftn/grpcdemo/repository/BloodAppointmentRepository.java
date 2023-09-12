package rs.ac.uns.ftn.grpcdemo.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import rs.ac.uns.ftn.grpcdemo.model.BloodAppointment;

import java.util.List;

public interface BloodAppointmentRepository extends JpaRepository<BloodAppointment, Integer> {
    @Query("SELECT ba FROM BloodAppointment ba WHERE ba.isFree = :isFree")
    List<BloodAppointment> findAllByIsFree(@Param("isFree") boolean isFree);
}
