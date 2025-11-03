package via.sep3.dataserver.grpc;

import static io.grpc.MethodDescriptor.generateFullMethodName;

/**
 */
@io.grpc.stub.annotations.GrpcGenerated
public final class DataRetrievalServiceGrpc {

  private DataRetrievalServiceGrpc() {}

  public static final java.lang.String SERVICE_NAME = "DataRetrievalService";

  // Static method descriptors that strictly reflect the proto.
  private static volatile io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.GetCoursesRequest,
      via.sep3.dataserver.grpc.GetCoursesResponse> getGetCoursesMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "GetCourses",
      requestType = via.sep3.dataserver.grpc.GetCoursesRequest.class,
      responseType = via.sep3.dataserver.grpc.GetCoursesResponse.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.GetCoursesRequest,
      via.sep3.dataserver.grpc.GetCoursesResponse> getGetCoursesMethod() {
    io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.GetCoursesRequest, via.sep3.dataserver.grpc.GetCoursesResponse> getGetCoursesMethod;
    if ((getGetCoursesMethod = DataRetrievalServiceGrpc.getGetCoursesMethod) == null) {
      synchronized (DataRetrievalServiceGrpc.class) {
        if ((getGetCoursesMethod = DataRetrievalServiceGrpc.getGetCoursesMethod) == null) {
          DataRetrievalServiceGrpc.getGetCoursesMethod = getGetCoursesMethod =
              io.grpc.MethodDescriptor.<via.sep3.dataserver.grpc.GetCoursesRequest, via.sep3.dataserver.grpc.GetCoursesResponse>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "GetCourses"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.GetCoursesRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.GetCoursesResponse.getDefaultInstance()))
              .setSchemaDescriptor(new DataRetrievalServiceMethodDescriptorSupplier("GetCourses"))
              .build();
        }
      }
    }
    return getGetCoursesMethod;
  }

  /**
   * Creates a new async stub that supports all call types for the service
   */
  public static DataRetrievalServiceStub newStub(io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceStub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceStub>() {
        @java.lang.Override
        public DataRetrievalServiceStub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new DataRetrievalServiceStub(channel, callOptions);
        }
      };
    return DataRetrievalServiceStub.newStub(factory, channel);
  }

  /**
   * Creates a new blocking-style stub that supports all types of calls on the service
   */
  public static DataRetrievalServiceBlockingV2Stub newBlockingV2Stub(
      io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceBlockingV2Stub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceBlockingV2Stub>() {
        @java.lang.Override
        public DataRetrievalServiceBlockingV2Stub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new DataRetrievalServiceBlockingV2Stub(channel, callOptions);
        }
      };
    return DataRetrievalServiceBlockingV2Stub.newStub(factory, channel);
  }

  /**
   * Creates a new blocking-style stub that supports unary and streaming output calls on the service
   */
  public static DataRetrievalServiceBlockingStub newBlockingStub(
      io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceBlockingStub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceBlockingStub>() {
        @java.lang.Override
        public DataRetrievalServiceBlockingStub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new DataRetrievalServiceBlockingStub(channel, callOptions);
        }
      };
    return DataRetrievalServiceBlockingStub.newStub(factory, channel);
  }

  /**
   * Creates a new ListenableFuture-style stub that supports unary calls on the service
   */
  public static DataRetrievalServiceFutureStub newFutureStub(
      io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceFutureStub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<DataRetrievalServiceFutureStub>() {
        @java.lang.Override
        public DataRetrievalServiceFutureStub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new DataRetrievalServiceFutureStub(channel, callOptions);
        }
      };
    return DataRetrievalServiceFutureStub.newStub(factory, channel);
  }

  /**
   */
  public interface AsyncService {

    /**
     */
    default void getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetCoursesResponse> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getGetCoursesMethod(), responseObserver);
    }
  }

  /**
   * Base class for the server implementation of the service DataRetrievalService.
   */
  public static abstract class DataRetrievalServiceImplBase
      implements io.grpc.BindableService, AsyncService {

    @java.lang.Override public final io.grpc.ServerServiceDefinition bindService() {
      return DataRetrievalServiceGrpc.bindService(this);
    }
  }

  /**
   * A stub to allow clients to do asynchronous rpc calls to service DataRetrievalService.
   */
  public static final class DataRetrievalServiceStub
      extends io.grpc.stub.AbstractAsyncStub<DataRetrievalServiceStub> {
    private DataRetrievalServiceStub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected DataRetrievalServiceStub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new DataRetrievalServiceStub(channel, callOptions);
    }

    /**
     */
    public void getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetCoursesResponse> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getGetCoursesMethod(), getCallOptions()), request, responseObserver);
    }
  }

  /**
   * A stub to allow clients to do synchronous rpc calls to service DataRetrievalService.
   */
  public static final class DataRetrievalServiceBlockingV2Stub
      extends io.grpc.stub.AbstractBlockingStub<DataRetrievalServiceBlockingV2Stub> {
    private DataRetrievalServiceBlockingV2Stub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected DataRetrievalServiceBlockingV2Stub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new DataRetrievalServiceBlockingV2Stub(channel, callOptions);
    }

    /**
     */
    public via.sep3.dataserver.grpc.GetCoursesResponse getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getGetCoursesMethod(), getCallOptions(), request);
    }
  }

  /**
   * A stub to allow clients to do limited synchronous rpc calls to service DataRetrievalService.
   */
  public static final class DataRetrievalServiceBlockingStub
      extends io.grpc.stub.AbstractBlockingStub<DataRetrievalServiceBlockingStub> {
    private DataRetrievalServiceBlockingStub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected DataRetrievalServiceBlockingStub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new DataRetrievalServiceBlockingStub(channel, callOptions);
    }

    /**
     */
    public via.sep3.dataserver.grpc.GetCoursesResponse getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getGetCoursesMethod(), getCallOptions(), request);
    }
  }

  /**
   * A stub to allow clients to do ListenableFuture-style rpc calls to service DataRetrievalService.
   */
  public static final class DataRetrievalServiceFutureStub
      extends io.grpc.stub.AbstractFutureStub<DataRetrievalServiceFutureStub> {
    private DataRetrievalServiceFutureStub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected DataRetrievalServiceFutureStub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new DataRetrievalServiceFutureStub(channel, callOptions);
    }

    /**
     */
    public com.google.common.util.concurrent.ListenableFuture<via.sep3.dataserver.grpc.GetCoursesResponse> getCourses(
        via.sep3.dataserver.grpc.GetCoursesRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getGetCoursesMethod(), getCallOptions()), request);
    }
  }

  private static final int METHODID_GET_COURSES = 0;

  private static final class MethodHandlers<Req, Resp> implements
      io.grpc.stub.ServerCalls.UnaryMethod<Req, Resp>,
      io.grpc.stub.ServerCalls.ServerStreamingMethod<Req, Resp>,
      io.grpc.stub.ServerCalls.ClientStreamingMethod<Req, Resp>,
      io.grpc.stub.ServerCalls.BidiStreamingMethod<Req, Resp> {
    private final AsyncService serviceImpl;
    private final int methodId;

    MethodHandlers(AsyncService serviceImpl, int methodId) {
      this.serviceImpl = serviceImpl;
      this.methodId = methodId;
    }

    @java.lang.Override
    @java.lang.SuppressWarnings("unchecked")
    public void invoke(Req request, io.grpc.stub.StreamObserver<Resp> responseObserver) {
      switch (methodId) {
        case METHODID_GET_COURSES:
          serviceImpl.getCourses((via.sep3.dataserver.grpc.GetCoursesRequest) request,
              (io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetCoursesResponse>) responseObserver);
          break;
        default:
          throw new AssertionError();
      }
    }

    @java.lang.Override
    @java.lang.SuppressWarnings("unchecked")
    public io.grpc.stub.StreamObserver<Req> invoke(
        io.grpc.stub.StreamObserver<Resp> responseObserver) {
      switch (methodId) {
        default:
          throw new AssertionError();
      }
    }
  }

  public static final io.grpc.ServerServiceDefinition bindService(AsyncService service) {
    return io.grpc.ServerServiceDefinition.builder(getServiceDescriptor())
        .addMethod(
          getGetCoursesMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              via.sep3.dataserver.grpc.GetCoursesRequest,
              via.sep3.dataserver.grpc.GetCoursesResponse>(
                service, METHODID_GET_COURSES)))
        .build();
  }

  private static abstract class DataRetrievalServiceBaseDescriptorSupplier
      implements io.grpc.protobuf.ProtoFileDescriptorSupplier, io.grpc.protobuf.ProtoServiceDescriptorSupplier {
    DataRetrievalServiceBaseDescriptorSupplier() {}

    @java.lang.Override
    public com.google.protobuf.Descriptors.FileDescriptor getFileDescriptor() {
      return via.sep3.dataserver.grpc.DataProtocol.getDescriptor();
    }

    @java.lang.Override
    public com.google.protobuf.Descriptors.ServiceDescriptor getServiceDescriptor() {
      return getFileDescriptor().findServiceByName("DataRetrievalService");
    }
  }

  private static final class DataRetrievalServiceFileDescriptorSupplier
      extends DataRetrievalServiceBaseDescriptorSupplier {
    DataRetrievalServiceFileDescriptorSupplier() {}
  }

  private static final class DataRetrievalServiceMethodDescriptorSupplier
      extends DataRetrievalServiceBaseDescriptorSupplier
      implements io.grpc.protobuf.ProtoMethodDescriptorSupplier {
    private final java.lang.String methodName;

    DataRetrievalServiceMethodDescriptorSupplier(java.lang.String methodName) {
      this.methodName = methodName;
    }

    @java.lang.Override
    public com.google.protobuf.Descriptors.MethodDescriptor getMethodDescriptor() {
      return getServiceDescriptor().findMethodByName(methodName);
    }
  }

  private static volatile io.grpc.ServiceDescriptor serviceDescriptor;

  public static io.grpc.ServiceDescriptor getServiceDescriptor() {
    io.grpc.ServiceDescriptor result = serviceDescriptor;
    if (result == null) {
      synchronized (DataRetrievalServiceGrpc.class) {
        result = serviceDescriptor;
        if (result == null) {
          serviceDescriptor = result = io.grpc.ServiceDescriptor.newBuilder(SERVICE_NAME)
              .setSchemaDescriptor(new DataRetrievalServiceFileDescriptorSupplier())
              .addMethod(getGetCoursesMethod())
              .build();
        }
      }
    }
    return result;
  }
}
