package rs.ac.uns.ftn.grpcdemo.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import rs.ac.uns.ftn.grpcdemo.model.Information;
import rs.ac.uns.ftn.grpcdemo.model.InformationStatus;

import java.util.List;

public interface InformationRepository extends JpaRepository<Information, Integer> {

    @Query("SELECT i FROM Information i WHERE i.status = :status")
    List<Information> findByStatus(@Param("status") InformationStatus status);
}
