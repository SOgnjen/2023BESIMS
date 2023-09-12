package rs.ac.uns.ftn.grpcdemo;

import net.devh.boot.grpc.client.inject.GrpcClient;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import rs.ac.uns.ftn.grpc.MessageProto;
import rs.ac.uns.ftn.grpc.MessageResponseProto;
import rs.ac.uns.ftn.grpc.NetGrpcServiceGrpc;

import java.util.UUID;
import java.util.concurrent.ThreadLocalRandom;

@Component
public class ScheduledTasks {

    @GrpcClient("netgrpcserver")
    private NetGrpcServiceGrpc.NetGrpcServiceBlockingStub stub;

    @Scheduled(fixedRate = 3000)
    public void sendMessageToServer() {
       MessageProto message = MessageProto.newBuilder().setMessage("Random message from Java client: " +
                UUID.randomUUID().toString()).setRandomInteger(ThreadLocalRandom.current().nextInt(1, 101)).build();
        final MessageResponseProto response = this.stub.transfer(message);
        System.out.println("Response: " + response.getResponse() + "; " + response.getStatus());
    }
}
