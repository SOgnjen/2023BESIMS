package rs.ac.uns.ftn.grpcdemo.model;

import com.fasterxml.jackson.annotation.JsonFormat;

import javax.persistence.*;
import java.time.LocalDate;

@Entity
@Table(name = "informations")
public class Information {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    @JsonFormat(pattern = "yyyy-MM-dd", shape = JsonFormat.Shape.STRING)
    @Column(name = "dateOfAppointment")
    private LocalDate when;

    @Enumerated(EnumType.ORDINAL)
    @Column(length = 20)
    private InformationStatus status;

    public Information() {}

    public Information(LocalDate when, InformationStatus status) {
        this.when = when;
        this.status = status;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public LocalDate getWhen() {
        return when;
    }

    public void setWhen(LocalDate when) {
        this.when = when;
    }

    public InformationStatus getStatus() {
        return status;
    }

    public void setStatus(InformationStatus status) {
        this.status = status;
    }
}
