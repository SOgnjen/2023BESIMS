package rs.ac.uns.ftn.grpcdemo.model;

import com.fasterxml.jackson.annotation.JsonFormat;

import javax.persistence.*;
import java.time.LocalDate;

@Entity
@Table(name = "bloodAppointments")
public class BloodAppointment {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    @JsonFormat(pattern = "yyyy-MM-dd", shape = JsonFormat.Shape.STRING)
    @Column(name = "dateOfAppointment")
    private LocalDate when;

    @Column(name = "isFree")
    private boolean isFree;

    @Column(name= "ownerJmbg")
    private Integer ownerJmbg;

    public BloodAppointment() {}

    public BloodAppointment(LocalDate when, boolean isFree, Integer ownerJmbg) {
        this.when = when;
        this.isFree = isFree;
        this.ownerJmbg = ownerJmbg;
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

    public boolean isFree() {
        return isFree;
    }

    public void setFree(boolean free) {
        isFree = free;
    }

    public Integer getOwnerJmbg() {
        return ownerJmbg;
    }

    public void setOwnerJmbg(Integer ownerJmbg) {
        this.ownerJmbg = ownerJmbg;
    }
}
