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

  private static volatile io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.LearningStep,
      via.sep3.dataserver.grpc.LearningStep> getAddLearningStepMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "AddLearningStep",
      requestType = via.sep3.dataserver.grpc.LearningStep.class,
      responseType = via.sep3.dataserver.grpc.LearningStep.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.LearningStep,
      via.sep3.dataserver.grpc.LearningStep> getAddLearningStepMethod() {
    io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.LearningStep, via.sep3.dataserver.grpc.LearningStep> getAddLearningStepMethod;
    if ((getAddLearningStepMethod = DataRetrievalServiceGrpc.getAddLearningStepMethod) == null) {
      synchronized (DataRetrievalServiceGrpc.class) {
        if ((getAddLearningStepMethod = DataRetrievalServiceGrpc.getAddLearningStepMethod) == null) {
          DataRetrievalServiceGrpc.getAddLearningStepMethod = getAddLearningStepMethod =
              io.grpc.MethodDescriptor.<via.sep3.dataserver.grpc.LearningStep, via.sep3.dataserver.grpc.LearningStep>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "AddLearningStep"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.LearningStep.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.LearningStep.getDefaultInstance()))
              .setSchemaDescriptor(new DataRetrievalServiceMethodDescriptorSupplier("AddLearningStep"))
              .build();
        }
      }
    }
    return getAddLearningStepMethod;
  }

  private static volatile io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.GetLearningStepsRequest,
      via.sep3.dataserver.grpc.GetLearningStepsResponse> getGetLearningStepsMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "GetLearningSteps",
      requestType = via.sep3.dataserver.grpc.GetLearningStepsRequest.class,
      responseType = via.sep3.dataserver.grpc.GetLearningStepsResponse.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.GetLearningStepsRequest,
      via.sep3.dataserver.grpc.GetLearningStepsResponse> getGetLearningStepsMethod() {
    io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.GetLearningStepsRequest, via.sep3.dataserver.grpc.GetLearningStepsResponse> getGetLearningStepsMethod;
    if ((getGetLearningStepsMethod = DataRetrievalServiceGrpc.getGetLearningStepsMethod) == null) {
      synchronized (DataRetrievalServiceGrpc.class) {
        if ((getGetLearningStepsMethod = DataRetrievalServiceGrpc.getGetLearningStepsMethod) == null) {
          DataRetrievalServiceGrpc.getGetLearningStepsMethod = getGetLearningStepsMethod =
              io.grpc.MethodDescriptor.<via.sep3.dataserver.grpc.GetLearningStepsRequest, via.sep3.dataserver.grpc.GetLearningStepsResponse>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "GetLearningSteps"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.GetLearningStepsRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.GetLearningStepsResponse.getDefaultInstance()))
              .setSchemaDescriptor(new DataRetrievalServiceMethodDescriptorSupplier("GetLearningSteps"))
              .build();
        }
      }
    }
    return getGetLearningStepsMethod;
  }

  private static volatile io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.IdRequest,
      via.sep3.dataserver.grpc.LearningStep> getGetLearningStepMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "GetLearningStep",
      requestType = via.sep3.dataserver.grpc.IdRequest.class,
      responseType = via.sep3.dataserver.grpc.LearningStep.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.IdRequest,
      via.sep3.dataserver.grpc.LearningStep> getGetLearningStepMethod() {
    io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.IdRequest, via.sep3.dataserver.grpc.LearningStep> getGetLearningStepMethod;
    if ((getGetLearningStepMethod = DataRetrievalServiceGrpc.getGetLearningStepMethod) == null) {
      synchronized (DataRetrievalServiceGrpc.class) {
        if ((getGetLearningStepMethod = DataRetrievalServiceGrpc.getGetLearningStepMethod) == null) {
          DataRetrievalServiceGrpc.getGetLearningStepMethod = getGetLearningStepMethod =
              io.grpc.MethodDescriptor.<via.sep3.dataserver.grpc.IdRequest, via.sep3.dataserver.grpc.LearningStep>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "GetLearningStep"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.IdRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.LearningStep.getDefaultInstance()))
              .setSchemaDescriptor(new DataRetrievalServiceMethodDescriptorSupplier("GetLearningStep"))
              .build();
        }
      }
    }
    return getGetLearningStepMethod;
  }

  private static volatile io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.LearningStep,
      via.sep3.dataserver.grpc.LearningStep> getUpdateLearningStepMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "UpdateLearningStep",
      requestType = via.sep3.dataserver.grpc.LearningStep.class,
      responseType = via.sep3.dataserver.grpc.LearningStep.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.LearningStep,
      via.sep3.dataserver.grpc.LearningStep> getUpdateLearningStepMethod() {
    io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.LearningStep, via.sep3.dataserver.grpc.LearningStep> getUpdateLearningStepMethod;
    if ((getUpdateLearningStepMethod = DataRetrievalServiceGrpc.getUpdateLearningStepMethod) == null) {
      synchronized (DataRetrievalServiceGrpc.class) {
        if ((getUpdateLearningStepMethod = DataRetrievalServiceGrpc.getUpdateLearningStepMethod) == null) {
          DataRetrievalServiceGrpc.getUpdateLearningStepMethod = getUpdateLearningStepMethod =
              io.grpc.MethodDescriptor.<via.sep3.dataserver.grpc.LearningStep, via.sep3.dataserver.grpc.LearningStep>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "UpdateLearningStep"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.LearningStep.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.LearningStep.getDefaultInstance()))
              .setSchemaDescriptor(new DataRetrievalServiceMethodDescriptorSupplier("UpdateLearningStep"))
              .build();
        }
      }
    }
    return getUpdateLearningStepMethod;
  }

  private static volatile io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.IdRequest,
      via.sep3.dataserver.grpc.Empty> getDeleteLearningStepMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "DeleteLearningStep",
      requestType = via.sep3.dataserver.grpc.IdRequest.class,
      responseType = via.sep3.dataserver.grpc.Empty.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.IdRequest,
      via.sep3.dataserver.grpc.Empty> getDeleteLearningStepMethod() {
    io.grpc.MethodDescriptor<via.sep3.dataserver.grpc.IdRequest, via.sep3.dataserver.grpc.Empty> getDeleteLearningStepMethod;
    if ((getDeleteLearningStepMethod = DataRetrievalServiceGrpc.getDeleteLearningStepMethod) == null) {
      synchronized (DataRetrievalServiceGrpc.class) {
        if ((getDeleteLearningStepMethod = DataRetrievalServiceGrpc.getDeleteLearningStepMethod) == null) {
          DataRetrievalServiceGrpc.getDeleteLearningStepMethod = getDeleteLearningStepMethod =
              io.grpc.MethodDescriptor.<via.sep3.dataserver.grpc.IdRequest, via.sep3.dataserver.grpc.Empty>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "DeleteLearningStep"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.IdRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  via.sep3.dataserver.grpc.Empty.getDefaultInstance()))
              .setSchemaDescriptor(new DataRetrievalServiceMethodDescriptorSupplier("DeleteLearningStep"))
              .build();
        }
      }
    }
    return getDeleteLearningStepMethod;
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
     * <pre>
     * --- Courses ---
     * </pre>
     */
    default void getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetCoursesResponse> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getGetCoursesMethod(), responseObserver);
    }

    /**
     * <pre>
     * --- Learning Steps ---
     * Full CRUD definitions to match Logic Tier Repository
     * </pre>
     */
    default void addLearningStep(via.sep3.dataserver.grpc.LearningStep request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getAddLearningStepMethod(), responseObserver);
    }

    /**
     */
    default void getLearningSteps(via.sep3.dataserver.grpc.GetLearningStepsRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetLearningStepsResponse> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getGetLearningStepsMethod(), responseObserver);
    }

    /**
     */
    default void getLearningStep(via.sep3.dataserver.grpc.IdRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getGetLearningStepMethod(), responseObserver);
    }

    /**
     */
    default void updateLearningStep(via.sep3.dataserver.grpc.LearningStep request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getUpdateLearningStepMethod(), responseObserver);
    }

    /**
     */
    default void deleteLearningStep(via.sep3.dataserver.grpc.IdRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.Empty> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getDeleteLearningStepMethod(), responseObserver);
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
     * <pre>
     * --- Courses ---
     * </pre>
     */
    public void getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetCoursesResponse> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getGetCoursesMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     * <pre>
     * --- Learning Steps ---
     * Full CRUD definitions to match Logic Tier Repository
     * </pre>
     */
    public void addLearningStep(via.sep3.dataserver.grpc.LearningStep request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getAddLearningStepMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     */
    public void getLearningSteps(via.sep3.dataserver.grpc.GetLearningStepsRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetLearningStepsResponse> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getGetLearningStepsMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     */
    public void getLearningStep(via.sep3.dataserver.grpc.IdRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getGetLearningStepMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     */
    public void updateLearningStep(via.sep3.dataserver.grpc.LearningStep request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getUpdateLearningStepMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     */
    public void deleteLearningStep(via.sep3.dataserver.grpc.IdRequest request,
        io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.Empty> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getDeleteLearningStepMethod(), getCallOptions()), request, responseObserver);
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
     * <pre>
     * --- Courses ---
     * </pre>
     */
    public via.sep3.dataserver.grpc.GetCoursesResponse getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getGetCoursesMethod(), getCallOptions(), request);
    }

    /**
     * <pre>
     * --- Learning Steps ---
     * Full CRUD definitions to match Logic Tier Repository
     * </pre>
     */
    public via.sep3.dataserver.grpc.LearningStep addLearningStep(via.sep3.dataserver.grpc.LearningStep request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getAddLearningStepMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.GetLearningStepsResponse getLearningSteps(via.sep3.dataserver.grpc.GetLearningStepsRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getGetLearningStepsMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.LearningStep getLearningStep(via.sep3.dataserver.grpc.IdRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getGetLearningStepMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.LearningStep updateLearningStep(via.sep3.dataserver.grpc.LearningStep request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getUpdateLearningStepMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.Empty deleteLearningStep(via.sep3.dataserver.grpc.IdRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getDeleteLearningStepMethod(), getCallOptions(), request);
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
     * <pre>
     * --- Courses ---
     * </pre>
     */
    public via.sep3.dataserver.grpc.GetCoursesResponse getCourses(via.sep3.dataserver.grpc.GetCoursesRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getGetCoursesMethod(), getCallOptions(), request);
    }

    /**
     * <pre>
     * --- Learning Steps ---
     * Full CRUD definitions to match Logic Tier Repository
     * </pre>
     */
    public via.sep3.dataserver.grpc.LearningStep addLearningStep(via.sep3.dataserver.grpc.LearningStep request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getAddLearningStepMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.GetLearningStepsResponse getLearningSteps(via.sep3.dataserver.grpc.GetLearningStepsRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getGetLearningStepsMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.LearningStep getLearningStep(via.sep3.dataserver.grpc.IdRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getGetLearningStepMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.LearningStep updateLearningStep(via.sep3.dataserver.grpc.LearningStep request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getUpdateLearningStepMethod(), getCallOptions(), request);
    }

    /**
     */
    public via.sep3.dataserver.grpc.Empty deleteLearningStep(via.sep3.dataserver.grpc.IdRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getDeleteLearningStepMethod(), getCallOptions(), request);
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
     * <pre>
     * --- Courses ---
     * </pre>
     */
    public com.google.common.util.concurrent.ListenableFuture<via.sep3.dataserver.grpc.GetCoursesResponse> getCourses(
        via.sep3.dataserver.grpc.GetCoursesRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getGetCoursesMethod(), getCallOptions()), request);
    }

    /**
     * <pre>
     * --- Learning Steps ---
     * Full CRUD definitions to match Logic Tier Repository
     * </pre>
     */
    public com.google.common.util.concurrent.ListenableFuture<via.sep3.dataserver.grpc.LearningStep> addLearningStep(
        via.sep3.dataserver.grpc.LearningStep request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getAddLearningStepMethod(), getCallOptions()), request);
    }

    /**
     */
    public com.google.common.util.concurrent.ListenableFuture<via.sep3.dataserver.grpc.GetLearningStepsResponse> getLearningSteps(
        via.sep3.dataserver.grpc.GetLearningStepsRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getGetLearningStepsMethod(), getCallOptions()), request);
    }

    /**
     */
    public com.google.common.util.concurrent.ListenableFuture<via.sep3.dataserver.grpc.LearningStep> getLearningStep(
        via.sep3.dataserver.grpc.IdRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getGetLearningStepMethod(), getCallOptions()), request);
    }

    /**
     */
    public com.google.common.util.concurrent.ListenableFuture<via.sep3.dataserver.grpc.LearningStep> updateLearningStep(
        via.sep3.dataserver.grpc.LearningStep request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getUpdateLearningStepMethod(), getCallOptions()), request);
    }

    /**
     */
    public com.google.common.util.concurrent.ListenableFuture<via.sep3.dataserver.grpc.Empty> deleteLearningStep(
        via.sep3.dataserver.grpc.IdRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getDeleteLearningStepMethod(), getCallOptions()), request);
    }
  }

  private static final int METHODID_GET_COURSES = 0;
  private static final int METHODID_ADD_LEARNING_STEP = 1;
  private static final int METHODID_GET_LEARNING_STEPS = 2;
  private static final int METHODID_GET_LEARNING_STEP = 3;
  private static final int METHODID_UPDATE_LEARNING_STEP = 4;
  private static final int METHODID_DELETE_LEARNING_STEP = 5;

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
        case METHODID_ADD_LEARNING_STEP:
          serviceImpl.addLearningStep((via.sep3.dataserver.grpc.LearningStep) request,
              (io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep>) responseObserver);
          break;
        case METHODID_GET_LEARNING_STEPS:
          serviceImpl.getLearningSteps((via.sep3.dataserver.grpc.GetLearningStepsRequest) request,
              (io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.GetLearningStepsResponse>) responseObserver);
          break;
        case METHODID_GET_LEARNING_STEP:
          serviceImpl.getLearningStep((via.sep3.dataserver.grpc.IdRequest) request,
              (io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep>) responseObserver);
          break;
        case METHODID_UPDATE_LEARNING_STEP:
          serviceImpl.updateLearningStep((via.sep3.dataserver.grpc.LearningStep) request,
              (io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.LearningStep>) responseObserver);
          break;
        case METHODID_DELETE_LEARNING_STEP:
          serviceImpl.deleteLearningStep((via.sep3.dataserver.grpc.IdRequest) request,
              (io.grpc.stub.StreamObserver<via.sep3.dataserver.grpc.Empty>) responseObserver);
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
        .addMethod(
          getAddLearningStepMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              via.sep3.dataserver.grpc.LearningStep,
              via.sep3.dataserver.grpc.LearningStep>(
                service, METHODID_ADD_LEARNING_STEP)))
        .addMethod(
          getGetLearningStepsMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              via.sep3.dataserver.grpc.GetLearningStepsRequest,
              via.sep3.dataserver.grpc.GetLearningStepsResponse>(
                service, METHODID_GET_LEARNING_STEPS)))
        .addMethod(
          getGetLearningStepMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              via.sep3.dataserver.grpc.IdRequest,
              via.sep3.dataserver.grpc.LearningStep>(
                service, METHODID_GET_LEARNING_STEP)))
        .addMethod(
          getUpdateLearningStepMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              via.sep3.dataserver.grpc.LearningStep,
              via.sep3.dataserver.grpc.LearningStep>(
                service, METHODID_UPDATE_LEARNING_STEP)))
        .addMethod(
          getDeleteLearningStepMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              via.sep3.dataserver.grpc.IdRequest,
              via.sep3.dataserver.grpc.Empty>(
                service, METHODID_DELETE_LEARNING_STEP)))
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
              .addMethod(getAddLearningStepMethod())
              .addMethod(getGetLearningStepsMethod())
              .addMethod(getGetLearningStepMethod())
              .addMethod(getUpdateLearningStepMethod())
              .addMethod(getDeleteLearningStepMethod())
              .build();
        }
      }
    }
    return result;
  }
}
