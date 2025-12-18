# Abstract

This work addresses the requirement for scalable and adaptable digital educational infrastructures through the development of Learnify, a distributed software system designed to ensure seamless content delivery and learning experience. 

The primary objective was the construction of a resilient, multi-server solution that leverages the distinct advantages of a polyglot microservices architecture to enhance availability, data integrity, and system responsiveness. The architectural design enforces strict interoperability between Java and C# components, utilizing gRPC for high-throughput intra-service communication and HTTP for external client accessibility. 

Data persistence is managed via a PostgreSQL database, while security concerns inherent to distributed systems are mitigated through the implementation of JWT authentication and Argon2 salted password hashing. 

The development process followed an iterative methodology, where functional requirements were derived from User Stories established through specialized analysis and stakeholder interviews. The resulting product successfully manages concurrent user sessions within a distributed environment. 

Verification procedures confirmed that the system meets essential requirements, demonstrating successful component interoperability, and robust data protection. The project concludes with a fully deployable distributed system, validating the architecture as a secure and stable foundation for the future scalability of educational technology.

\newpage

# Introduction

The acquisition of new knowledge is an essential part of human life and evolution. Functioning in society necessitates communication, which in turn requires a foundational level of knowledge (Habermas, 1984). Although mandatory education became a universal global standard during the late 20th and early 21st centuries (UNESCO, 2000), significant disparities remain - approximately 40% of the global population still lacks access to education in a language they understand (PTI, 2025).

The aim of this project is the development of a system which would be able to provide learning opportunities with a specific focus on accessibility, efficiency, and the optimization of learning processes. Beyond this, the system also aimed to ensure security, data integrity, and deployability of the solution among other things.

While the pursuit of knowledge has been a cornerstone of human development for millennia, the incorporation of digital technologies into education is in perpetual evolution (Siemens, 2005). A widely accepted model for learning with digital technologies has not been identified, mainly because the exponential increases in computing power and volumes of online information constantly redefine how users approach knowledge acquisition, processing, and retention (Haleem et al., 2022).

The approach of this project is to develop a distributed educational platform implemented using a polyglot architecture, utilizing a database for data persistence, and adapting a hybrid communication strategy that includes technologies such as gRPC and HTTP.

\newpage

# Main Section

The main section of this document is organized in the direction from the high-level analysis of the problem towards the specific implementation, testing and other relevant aspects of the solution created. This direction does not represent the chronological order of the project development and thus some aspects of the problem might not have a solution designed, implemented, tested, or deployed yet.

This overview represents merely a snapshot of the project development across each phase, and thus does not present a full solution or even analysis of the problem domain.

## Analysis

The fundamental domain knowledge was at first derived from the analyzed problem domain via literature review, and research. Because of the data-driven nature of this project, the analysis focused on stakeholder interactions since the beginning to ensure proper understanding and to test the assumptions made. With more progress made on the solution, the analysis was evolutionarily refined to reflect the new understanding of the problem domain in the specificities of the solution space created by Learnify.

The most abstract and crucial aspect of the analysis was defining the system actors. The actors were defined to be:

- Learners
- Teachers
- Admins

The most debated aspect of the definition was the relationship between the roles, and particularly how teachers and admins relate to it. It was established that teachers and admins are a type of a learner, and this was confirmed throughout the project most importantly because:

- both teachers and admins were expected to be skilled users of the platform (Appendix 10.1, Interview_261125.pdf) and were supposed to be educated on it (Appendix 10.1, Interview_121025.pdf)
- both teachers and admins were understood as learners within the leaderboard setting and were expected to be equal participants in it (Appendix 10.1, Interview_121025.pdf)

### Actor Descriptions

#### Learner

Learners strive for knowledge acquisition. They want to be motivated to learn and they should be allowed to have a structured way of learning new information. They want to be able to learn various different topics at their own pace. They are looking for a gamified experience.

#### Teacher

Teachers are trusted Learners, who also want to share their knowledge with others. They want to be able to manage courses easily and have a structured way of doing it.

#### Administrator (Admin)

Admins are trusted Learners, who have the right to manage the platform. They should be able to manage all learners, and platform settings.

### Requirements

#### Functional requirements

The functional requirements are structured as user stories to better capture the perspective of the actor and to clarify permissions and intentions behind each requirement. This way the user stories served as the fundamental source of truth and a guideline for understanding the problem and being able to design a solution that would address the problem preserving the idea behind the intention of the actor.

|  ID   | User Story                                                                                                                |
| :---: | :------------------------------------------------------------------------------------------------------------------------ |
| USL1  | As a Learner, I want to register for an account so that I can access the platform.                                        |
| USL2  | As a Learner, I want to log in so that I can access the platform from my account.                                         |
| USL3  | As a Learner, I want to see in which courses I am enrolled in, so that I can continue where I left off.                   |
| USL4  | As a Learner, I want to continue learning where I left off, so that I don't have to start over every time.                |
| USL5  | As a Learner, I want to see all available courses, so that I can explore and choose what I want to learn.                 |
| USL6  | As a Learner, I want to filter courses, so that I can find specific content quickly.                                      |
| USL7  | As a Learner, I want to unenroll from a course, so that I can stop learning a course I no longer want to finish.          |
| USL8  | As a Learner, I want to view the Leaderboard, so that I can compare my progress with other learners.                      |
| USL9  | As a Learner, I want to view my Profile, so that I can see my personal account details.                                   |
| USL10 | As a Learner, I want to test my knowledge within the course, so that I know I understood the topic and I am not bored.    |
| UST1  | As a Teacher, I want to submit a course draft, so that I can find out if my course idea is relevant for the platform.     |
| UST2  | As a Teacher, I want to manage course content, so that I can correct or improve previous work.                            |
| UST3  | As a Teacher, I want to edit course information, so that I can correct mistakes.                                          |
| USA1  | As an Admin, I want to see all drafts, so that I know what drafts are waiting for approval.                               |
| USA2  | As an Admin, I want to approve course drafts, so that the teacher knows they can work on such course.                     |
| USA3  | As an Admin, I want to add course categories and languages, so that the platform can easily adapt to new content.         |
| USA4  | As an Admin, I want to manage users’ roles, so that I can manage what access is given to the platform and to what degree. |
| USA5  | As an Admin, I want to disapprove course drafts, so that the teacher knows such course is not needed at the moment.       |

*Table 1: Functional Requirements (Appendix 2.1 Requirements)*

The user stories are sorted based on the actors to which they correspond, not according to the chronological order in which they were added/discovered. The chronological order is also not perfectly reflected on the IDs, as these were not always static for the same user story (when managing them, they would be adjusted)

#### Non-functional requirements

1. The system must be polyglot
2. User passwords must be securely stored at rest
3. The system must be deployable
4. The system must be color-blind friendly
5. The system must be distributed

### Use cases and their related requirements

In order to address the user stories, use cases of the system were developed, which further clarified the requirements and provided a basis for understanding the system behaviour (giving basis for dynamic rather than static analysis).

The use cases developed are shown in a table below:

| ID   | Use Case                   |
| :--- | :------------------------- |
| UC1  | Register                   |
| UC2  | Log in                     |
| UC3  | Manage Personal Learning   |
| UC4  | Browse and Search Catalog  |
| UC5  | Complete Learning Step     |
| UC6  | View User Profile          |
| UC7  | View Leaderboard           |
| UC8  | Create Course Draft        |
| UC9  | Edit Course Content        |
| UC10 | Manage System Settings     |
| UC11 | Review Course Drafts       |
| UC12 | Manage User Roles          |

*Table 2: Use Cases (Appendix 2.2 Use Cases)*


The table below shows how the use cases are related to the user stories

| User Story | Use Cases Addressing It |
| :--------- | :---------------------- |
| USL1       | UC1                     |
| USL2       | UC2                     |
| USL3       | UC3                     |
| USL4       | UC3, UC5                |
| USL5       | UC4                     |
| USL6       | UC4                     |
| USL7       | UC3                     |
| USL8       | UC7                     |
| USL9       | UC6                     |
| USL10      | UC5                     |
| UST1       | UC8                     |
| UST2       | UC9                     |
| UST3       | UC9                     |
| USA1       | UC11                    |
| USA2       | UC11                    |
| USA3       | UC10                    |
| USA4       | UC12                    |
| USA5       | UC11                    |

*Table 3: Use Cases and their related requirements (Appendix 2.2 Use Cases)*


### Use case diagram (UCD)

To depict how the use cases were related to the system actors, a use case diagram was created as shown below:

![Use Case Diagram (Appendix 2.2 Use Cases)](..\out\Analysis\UseCaseDiagram\UseCaseDiagram.svg){width=60%}

As can be seen, the UCD also introduced the internal boundary for teachers and admins - specifying that these actors are not simply learners with privileges but there is a boundary to be crossed when becoming a teacher or an admin. The UCD also specifies the system boundary, which in the case of Learnify covers all the use cases developed.

### Use case descriptions

In order to fully describe the use cases, use case descriptions were created; an example below shows such use case description, specifically for the UC5 - Complete Learning Step use case:

![Complete Learning Step Use Case Description (Appendix 2.2 Use Cases)](CompleteLearningStepUseCaseDescription.png)

As seen above, the use case descriptions provided a structured way of understanding how the system should behave and gave a strong basis for the test cases. 

All the use case descriptions were made in the same format with:

- Use Case ID and Name
- Summary
- Actor(s)
- Preconditions
- Postconditions
- Base Sequence
- Alternative Sequences

It was also determined that when the system fails to perform its action, the user should be notified of the error and the inability to proceed. This was noted on the side of the Use Case Descriptions as a general alternative sequence.

### System sequence diagrams (SSD)

System Sequence Diagrams (SSDs) were developed to illustrate the interaction between the system actors and the system as a black box. By focusing on the input and output events, the SSDs helped in identifying the necessary system operations and the data that needs to be exchanged to fulfill each use case.

An SSD was created for each of the 12 use cases, ensuring that the dynamic behavior of the system is fully captured from an external perspective.

The figure below shows the SSD for UC5 - Complete Learning Step, which highlights the iterative nature of the learning process and the system's role in providing feedback.

![Complete Learning Step SSD (Appendix 2.3.2 System Sequence Diagrams)](..\out\Analysis\SSDs\UC5_CompleteLearningActivity\UC5_CompleteLearningActivity.svg){width=60%}

The system sequence diagrams were kept simple and focused on the core interaction. Nevertheless, for UC9 and UC11 it was decided to include alternate paths while not including anything else than the main success scenarios. 

### Activity diagrams

The development of activity diagrams was crucial in understanding the dynamic behaviour and the interplay of several use cases and domain entities. While use case descriptions provide a structured textual representation, activity diagrams allow for a visual understanding of the logical flow, decision points, and the interaction between the user and the system's core components.

A set of activity diagrams was developed to cover the most critical workflows of the Learnify platform, including user onboarding, course discovery, content creation, and the learning process itself. 

The activity diagram below illustrates the core workflow of a Learner interacting with the platform. It demonstrates the interplay between UC3 (Manage Personal Learning), UC5 (Complete Learning Step), and UC7 (View Leaderboard). 

![Learning and Achievement Activity Diagram (Appendix 2.3.1 Activity Diagrams)](..\out\Analysis\ActivityDIagrams\LearningAndAchievement\LearningAndAchievement.svg){width=60%}

This diagram is arguably one of the most important aspects of the application as it demonstrates something similar to a core loop of the system - a typical path a user takes within a session.

By modeling these workflows, the analysis phase ensured that the system's dynamic behavior aligns with the identified user stories and the relationships defined in the domain model.

### Test Cases

Following the definition of the system’s dynamic behavior through use cases, SSDs and activity diagrams, a set of high-level test cases was derived to formalize the acceptance criteria for the Learnify system. These test cases were constructed directly from the use case descriptions, specifically targeting the preconditions, base sequences, and alternative sequences defined in the previous sections.

The objective of defining these test cases during the analysis phase - rather than the testing phase - was to ensure understanding of system's functionality and to aid the definition of done.

The table with all test cases can be found in Appendix 2.4 Tests - the table provided there also contains the test case results as these test cases were used during the testing phase as well.

### Domain model

The domain model was constructed for this project to better understand the problem domain and to aid communication among stakeholders. The crucial aspect of developing the domain model was identifying the relationships between different kinds of users, in particular the roles and responsibilities of Learners, Teachers, and Administrators; which had to be combined with the security aspect of the system as well as had to align with the shared understanding of the stakeholders.

![Domain Model (Appendix 2.3 Diagrams)](..\out\Analysis\DomainModel\DomainModel.svg){width=80%}

The figure above shows the domain model for Learnify. It can be seen that the crucial aspect of fully describing the different system roles is understanding what entities exist and how they relate to each other.

The inheritance from the User entity reflects the fact that all system users have some common basic attributes, rights and behaviours that can be generalized.

The philosophy behind the attribute selection and abstraction into separate entities was to provide full flexibility for future development of the system, and ensuring that no restrictions are imposed for no reason. For example, Learning Steps are abstracted into three different entities to fully support any kind of idea of a learning step as seen both in the domain and from stakeholder interactions.

At the same time, the domain model was kept in its simplest form possible in terms of more abstract entity concepts. As can be seen on the domain model, the core aspects of the system are:

- Users
- Courses
- Learning Steps

And it could be further argued that Learning Steps only exist as a part of Courses, therefore the Domain Model is centered around the idea of Users learning from Courses, which did not change from the initial vision of the system.

The inclusion of stakeholders as entities within the domain model mostly arose from the need of defining and understanding attributes and relationships of Users.

### Security Requirements

The security requirements for the system were developed as a part of the threat modelling process. The security objectives were as follows:

- **Confidentiality:** Protect user passwords and personal data from unauthorized disclosure.
- **Integrity:** Ensuring that data can not be altered or tampered with by unauthorized parties.
- **Availability:** Ensure the system remains accessible during high traffic or denial-of-service attempts.
- **Accountability:** Actions must be uniquely traceable to a specific entity.
- **Authenticity:** Verify that data inputs and users are genuine.

These objectives were developed based on the CIA triad and expanded to fit the needs of the system (Appendix 7.1 Threat Model).

Similarly to other parts of the analysis phase, stakeholder interactions shaped the system's security requirements. In particular, even testing the prototype (before having the core system functional) showed concerns about the authority of Administrators and Teachers, and needs for accountability for actions.

## Design

### System design

The aim of the designing phase was to establish a clear vision and guide for implementing the solution. Design phase created the largest gap between the initial problem definition and the approach taken to solve it by transforming certain concepts into an implementable or more flexible form (e.g. user roles).

#### Wireframes

Wireframes were used along other rough sketches to design the components of the user interface without the hassle of dealing with the final styling. Because of the chosen methodologies, wireframes provided a strong basis for creating HTML skeletons of various razor pages.

![All Courses Page Wireframe (Appendix 9.1 Wireframes)](..\out\Analysis\Wireframes\Wireframes-2.png){width=80%}

The figure above depicts one of the wireframes created for the system - all courses page or as described in the analysis - "The Course Catalogue".

The interface also includes features which are showed only to users with specific roles (Teacher, Admin); it can be seen that there are two action buttons named "Create Draft" which appears only for users with a Teacher role and "Waiting Drafts" which appears only for users who are an Administrator. These buttons give Teachers and Administrators quick access to content creation and moderation tool while keeping the learner view free from unnecessary elements.

The fidelity of the wireframes was kept low but the transformation into the chosen technology PlantUML (Roques, n.d.) provided a more visually accurate representation with less details provided from the creation inputs; ultimately resulting in wireframes that appear of higher fidelity.

### Architectural overview

The architecture is a distributed, three-tier solution that aims for strict separation of concerns, scalability, and polyglot interoperability. The architecture facilitates interaction between a C# frontend, a C# logic middleware, and a Java data persistence layer.

#### Client

The client-side application is built using Blazor in C# .NET. This layer hosts the web interface accessible to all system actors: learners, teachers, and admins. It is responsible solely for UI rendering and user input handling, delegating all business logic to the backend services.

The solution utilizes Blazor Server, which decouples the UI rendering from the client. This means that a significant part of the interactions is done on the blazor server rather than directly on the client machine. 

#### Logic Tier

The Logic Server, implemented in C# (ASP.NET Core), and acts as the central orchestrator. It exposes a RESTful API via HTTP(s) to the client, ensuring broad compatibility and standard web communication. This layer handles authentication, authorization (RBAC), and feature-specific business rules. It serves as a protocol bridge, translating external HTTP requests into internal gRPC calls for the data layer.

#### Data Tier

The Data Server is implemented in Java (Spring Boot), fulfilling the project's polyglot requirement. Communication between the Logic and Data servers is conducted via gRPC - Google's Remote Procedure Call framework. This choice leverages Protocol Buffers (Protobuf) for binary serialization, resulting in lower latency and higher throughput compared to text-based JSON over HTTP.

#### Data Persistence

At the foundation of the architecture is a PostgreSQL database. The Java Data Server manages all database interactions, ensuring that the Logic and Client layers remain agnostic to the underlying storage mechanics.

#### Architectural Diagram and Justification

The figure below depicts the final architecture of the system as described above:

![Architectural Overview (Appendix 11.1 Architecture)](ArchitecturalOverview.png){width=60%}

The mentioned technologies, frameworks, and tools were chosen both to fulfill the requirements but also prioritized flexibility and ease of development. 

### Class diagram design

There is a class diagram for each of the servers, demonstrating their independence. These are the Client App, Logic Server and Data Server.

#### Client App Class Diagram

This server is responsible for displaying the system to the client and to help the client navigate through our system, giving freedom to the user to use the system as they please, using high-level methods.

![Client App Class Diagram](../out/Implementation/ClientAppClass/ClientAppClass.svg)

#### Logic Server Class Diagram

In this server the logic of the system is defined through the controllers, allowing the client server to perform the actions requested by the client.

![Logic Server Class Diagram](../out/Implementation/LogicServerClass/LogicServerClass.svg)

The diagram bellow shows the different entities needed in the logic server. All entities implement the IIdentifiable interface, achieving an abstract system where entities are easily replaceable.

![Entities Logic Server](../out/Implementation/EntitiesLogicServer/EntitiesLogicServer.svg)

The following diagram is a summarized easy-to-read Logic Server Class Diagram.

![Summary Logic Server Class Diagram](../out/Implementation/SummaryLogicServer/SummaryLogicServerClass.svg)

#### Data Server Class Diagram

This server main responsibility is to manage the database by adding, fetching, modifying and deleting the entities, ensuring that the logic server requests are completed successfully.

![Data Server Class Diagram](../out/Implementation/DataServerClass/DataServerClass.svg)

In the next diagram the entities of the data server are exposed. It looks similar to the GRD since the system uses JPA, ensuring that the relationships in the data server's entities are transfered to the database,

![Entities Data Server](../out/Implementation/EntitiesDataServerClass/EntitiesDataServerClass.svg)

The following diagram displays a summary of the data server class diagram. Services and repositories can be seen on it.

![Summary Data Server](../out/Implementation/SummaryDataServerClass/SummaryDataServerClass.svg)

### Communication protocol design

The system implements a multi-tiered architecture that utilizes distinct communication protocols for external and internal interactions. The sequence diagram below illustrates the end-to-end communication flow, demonstrating how the Client, Logic Server, and Data Server interact to process a request.

![Application Layer Sequence Diagram](Application-LayerSD.png){width=60%}

### Security Design 

Taking into consideration the earlier defined threat analysis and risk assessment, the security design focuses on the technologies used to defend against identified threats. System maintains various strategies of protecting the data at rest and while being transmitted. 

##### Authentication and Authorization 

To address the threats of Spoofing and Elevation of Privilege, the System is based on stateless authentication architecture. 

- Authentication: The System delegates auth operations to an AuthController. Once credentials are successfully validated, the System sends a JWT to the client. The System relies on JwtSecurityTokenHandler to sign the token using the HmacSha256Signature algorithm and a pre-configured secret key loaded though server level application settings.

 - Authorization (RBAC): Access control is granted via ADDJwtBearer authentication method. The System inspects the claims of incoming requests to restrict access based on user roles (Learner, Teacher, Admin), effectively mitigating threats and unauthorized access.
 
  ##### Data Protection and Integrity 
  
  To address Confidentiality and Integrity requirements, following strategies have been adopted to protect data throughout its lifecycle. 
  
  - Data at Rest: The system follows the principle of data minimization, ensuring no personal information is stored beyond the necessary authentication credentials (usernames and passwords). To reduce the impact of potential database leaks, passwords are never stored as plain text. The system utilizes the Argon2 algorithm. This is hash algorithm provide resistance against brute-force attacks. 

  - Data during Transit: To protect data against Man-in-the-Middle Attacks, protection strategies differ based on network exposure: Client to Server: The Logic Server enforces Transport Layer Security (TLS) via the app.UseHttpsRedirection() middleware. This ensures that user credentials are encrypted when being transferred over the public internet. Logic to Data: Communication between the Logic and Data servers occurs via gRPC. While this traffic remains unencrypted, the data is serialized in binary Protobuf format. This unencrypted state is considered acceptable for the current project scope as it assumes strict network isolation.

##### Input Validation

To reduce the possibility of injection attacks the system validates users input.
- The System's SecureAuthService is designed to handle user inputs and throw errors when invalid inputs  such as mismatched passwords are detected, so that bad data is rejected before entering database.

- The system uses strict gRPC message typing (e.g., the AddUserRequest) to guarantee data structure. This ensures that injection attacks relying on malformed data structures or unexpected fields are impossible, as they are rejected by the protocol's strict binary validation before reaching the application logic.

#### Interface Definition (gRPC & Protobuf)

Internal communication between the Logic Server and the Data Server is managed via gRPC. The data structures and service contracts are defined using Protocol Buffers (Protobuf), ensuring strict typing.

The next code snippet demonstrates the definition of the message structures (Requests and Responses) used within the system.

```Protobuf
message User {
  int32 id = 1;
  string username = 2;
  string password = 3;
  repeated Role roles = 4;
}

message Role {
  string role = 1;
}

message GetUsersRequest {}

message GetUsersResponse {
  repeated User users = 1;
}
```

The next code snippet illustrates the service definition, detailing the RPC methods, their required parameters, and return types.

```Protobuf
service UserService {
  rpc GetUsers(GetUsersRequest) returns (GetUsersResponse);
  rpc GetUser(GetUserRequest) returns (User);
  rpc AddUser(AddUserRequest) returns (AddUserResponse);
  rpc UpdateUser(UpdateUserRequest) returns (User);
}
```

#### API Specification

The Logic Server exposes a RESTful API to external clients using standard HTTP/1.1 protocols. This design streamlines client integration by using standard HTTP verbs (GET, POST, PUT, DELETE) and status codes.

For example, authentication is handled via the /auth/login endpoint. By sending a POST request to http://localhost:9090/auth/login with the correct credentials, a client can successfully authenticate and connect to the system.

#### Protocol Justification

A hybrid protocol approach was chosen to balance user experience with system performance, in specific, HTTP was chosen for the logic server and gRPC for the data server:

· External Communication (HTTP/JSON): HTTP was used with JSON for client-server interaction because of its universality and readability. JSON is natively supported by web browsers and mobile clients, making the system easy to debug and integrate. While the text-based format introduces some overhead, the trade-off favors the ease of development and broad compatibility required at the client layer.

· Internal Communication (gRPC/Protobuf): For communication between the Logic and Data servers, gRPC was selected over REST. Unlike the text-based JSON, gRPC uses Protocol Buffers to serialize data into a binary format. This results in significantly smaller payload sizes and faster serialization/deserialization times. Furthermore, gRPC operates over HTTP/2, allowing for multiplexing and lower latency, which is critical for high-throughput internal traffic.

### Database design

#### Enhanced Entity Relationship Diagram

As means of bridging the gap from the problem domain in general and the Learnify system in specific, an EER diagram was created as can be seen on the figure below:

![Enhanced Entity Relationship Diagram (Appendix 2.3 Diagrams)](..\out\Implementation\EER\EER.png)

The EER developed does not significantly differ from the domain model as both diagrams are conceptual and could in theory be used interchangeably. However, the EER diagram further reflects the decisions made during analysis, which most notably reflected on the way how roles are handled.

```plantuml {caption="SystemUser to Role Relationship"}
@startuml
class SystemUser {
    id
}
class Role {
    role
}
SystemUser "*" -right- "*" Role : has
@enduml
```

The figure above is a part of the domain model (Appendix 2.3 Diagrams) and focuses on the relationship between the User and their Roles. This relationship in contrast to inheritance based models provides a flexible and strict way of handling user roles - their permissions and access to the system. Most importantly it does not hide the complexities of inheritance into a seemingly simple abstraction and prevents the potential issues that could arise from mindless inheritance hierarchies.

Certain relationships could in theory be kept as many-to-many, however the need to attach attributes to these relationships led to the preference of a separate entity to hold the attribute thus separating the concerns and providing a more flexible design. This can be seen on, for example, the UserCourseProgress entity as shown below:

```plantuml {caption="SystemUser to Role Relationship"}
@startuml
class SystemUser
{
    id
    username
    password_hash
}
class UserCourseProgress {
    current_step
}
class Course {
    id
    title
    description
    total_steps
}
SystemUser "1" -right- "*" UserCourseProgress : has progress
Course "1" -left- "*" UserCourseProgress : tracked in
@enduml
```

#### Relational Schema

Contrary to the conceptual modelling, the logical modelling required adherence to the rules and specificities of the relational model.
Because of the designed use of standard relational database (PostgreSQL), the mapping of the EER needed to determine all the necessary resolutions of relationships, strong and weak entities, and the establishment of integrity keys.

The mapping of the EER diagram resulted in a relational schema and the to it related global relations diagram.

The mapping resulted in the relation schema as shown on the figure below:

![Relational Schema (Appendix 4.1 Relation Schema)](..\out\Implementation\RelationalSchema\RelationalSchema.png)

As can be seen below on the part of Global Relational Diagram (Appendix 2.3 Diagrams), the many-to-many relationships got resolved into new relations - SystemUserRole, and UserCourseProgress.

```plantuml {caption="Caption"}
@startuml
class SystemUser {
    id
}
class Role {
    role
}
class SystemUserRole {
    systemUserId
    roleId
}
SystemUser "1" -right- "*" SystemUserRole
Role "1" -left- "*" SystemUserRole
@enduml
```

#### Global Relations Diagram (GR/GRD)

Relational schema and Global Relations Diagram are technically identical. In the project Learnify, the GRD was the main source of truth for later implementation of the database structure and discussing the data persistence design.

The final GR diagram is shown on the figure below:

![Global Relational Diagram (Appendix 2.3 Diagrams)](..\out\Implementation\GR\GR.png)

It can be seen that the GRD reflects the same concepts as the relational schema, however, in this case the positioning of the elements provides a more natural step going from the EER and the domain model; although, the positioning did not fully preserve the conceptual relationships as seen in the EER diagram.

## Implementation

The implementation of the system followed the designed architecture and communication protocols with a focus on setting up the core of the architecture first as one continuous vertical slice.

At first, a database schema was created in PostgreSQL, a Springboot project was created for the Data Server, and two .NET solutions were created for the Logic Server and the Client Application.

This stage did not include any actual logic but rather provided a skeleton of the system. One of the decisions taken at this stage was to maintain separate solutions for the Logic Server and Client Application. Despite the initial idea of implementing a shared solution, it was decided that the feature of C# anonymous types would suffice for most of the purposes of data transfer objects (DTOs) and that the added complexity of a shared solution would not be justified.

The implementation of the skeleton was followed by the implementation of a vertical slice which focused on fetching all courses from the database.

The individual components of the vertical slice can be seen below on the part of Domain Model (Appendix 2.3 Diagrams):

```plantuml {caption="Caption"}
@startuml
class Course
{
    id
    language
    title
    description
}
@enduml
```

At a later stage, the original database setup was split into pure DDL script and a dummy data initial setup crucial for testing stages mentioned later in this document.

### Methods and tools

Going further into implementation details, Java and C# programming languages were chosen to meet the requirements of multilanguage system. For storing information PostgreSQL database has been implemented. 
The development team chose C# Blazor .NET for the frontend because its component-based system gave a possibility for fast GUI development without needing an additional JavaScript code. On the Logic Server, ASP.NET Core provided an intuitive environment for managing REST endpoints and gRPC services, facilitating high-speed communication. It was achieved by using shared Data Transfer Objects (DTOs) and validation logic which kept data model changes synchronized between client and server systems. 
Java programming language was selected to run the Data Server because it met this semester's polyglot requirements and demonstrated how .NET and Java systems can work together using gRPC protocol, which is explained further in the integration logic paragraph below.

### Servers Implementation

Data Server implemented using Java was intended to not have any code related to the main logic of the system. Its main responsibility was to handle operations of the services which were either taking data from the database or updating the database. The framework that was used in order to make the implementation process cleaner and more efficient was Spring boot. The team chose Spring Data JPA when it comes to handling data persistent with PostgreSQL due to the possibility of working with Java objects instead of raw SQL queries.

The Logic Server operated as the system's Web API which functioned as the core processing unit of the platform by using the C# ASP.NET Core framework. The Java server took care of database management while the Logic Server executed all business operations which enable the system to function properly. The system operated as a middleman between the Blazor client and the Data Server because it handled requests which followed system rules before sending data to the Data Server. It is worth mentioning that security took a big part on this server. The system used JWT (JSON Web Tokens) to handle user authentication which restricted access to particular features based on user authorization. All the main logic was kept here which made it simple to control features such as course enrollments and leaderboard system or course draft approval workflow. Using C# for this layer was a great fit because it worked perfectly with the Blazor client on another server, allowing the team to keep the code organized and easy to build on.

When it comes to Client Server, as mentioned before, Blazor C# was chosen. It gave the team a structured template to work on the frontend using reusable components and integrating logic using C# programming language instead of JavaScript. Client side was responsible for sending HTTP request to the WebApi through user friendly, GUI.

### Integration Logic:
To showcase the path from GUI through the servers, the database, and back as well as communication between the servers, the following figures demonstrate the necessary logs and implementations of such functionality. The code can be found in (Appendix 3.1 Source Code).

#### Login Feature

When a user who already has an existing account tries to log in, they input their credentials into the text field and clicks the login button.

![Click login as teacher](clicklogin.png)

After clicking the login button, the client side server sends a request to the Logic Server. The following json represents the HTTP request sent:

```json
[Client] Sending HTTP POST /auth/login:
{
  "username": "teacher1",
  "password": "password123"
}
```

The Logic Server receives this request and initiates the validation process.

**Logic Server (AuthController.cs):**

```csharp
[HttpPost("login")]
public async Task<ActionResult> Login([FromBody] LoginRequest request)
{
    try
    {
        User foundUser = await authService.ValidateUser(request.Username, request.Password);
        string token = GenerateJwt(foundUser);
        return Ok(token);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}
```
*Code Snippet 1: AuthController (Appendix 3.1 Source Code)*


The `AuthController` delegates the validation to the `SecureAuthService`.

**Logic Server (SecureAuthService.cs):**

```csharp
public async Task<User> ValidateUser(string username, string password)
{
    User? existingUser = await GetUserByUsernameAsync(username) ?? throw new Exception("User not found");

    if (!Argon2.Verify(existingUser.Password, password))
        throw new Exception("Password mismatch");

    return existingUser;
}

private async Task<User?> GetUserByUsernameAsync(string userName)
{
    IEnumerable<User> users = await Task.Run(() => userRepository.GetMany());
    foreach (User user in users)
    {
        if (user.Username.Equals(userName))
        {
            return user;
        }
    }
    return null;
}
```
*Code Snippet 2: SecureAuthService (Appendix 3.1 Source Code)*

The `SecureAuthService` retrieves users via the `gRPCUserRepository`, which communicates with the Data Server.

**Logic Server (gRPCUserRepository.cs):**

```csharp
public override IQueryable<User> GetMany()
{
    var resp = UserServiceClient.GetUsers(new GetUsersRequest());
    var users = resp.Users.Select(c => new User
    {
        Id = c.Id,
        Username = c.Username,
        Password = c.Password,
        Roles = c.Roles.Select(r => new Entities.Role { RoleName = r.Role_ }).ToList(),
    }).ToList();

    return users.AsQueryable();
}
```
*Code Snippet 3: gRPCUserRepository (Appendix 3.1 Source Code)*

The Protobuf message structure used for this communication is:

```protobuf
message User {
  int32 id = 1;
  string username = 2;
  string password = 3;
  repeated Role roles = 4;
}

message Role {
  string role = 1;
}

message GetUsersRequest {}

message GetUsersResponse {
  repeated User users = 1;
}
```
*Code Snippet 4: Protocol (Appendix 3.1 Source Code)*

Finally, the Data Server handles the gRPC request and retrieves the users from the database.

**Data Server (UserServiceImpl.java):**

```java
@Override
public void getUsers(GetUsersRequest request, StreamObserver<GetUsersResponse> responseObserver) {
    try {
        List<SystemUser> users = userRepository.findAll();
        List<via.sep3.dataserver.grpc.User> grpcUsers = new ArrayList<>();

        for (SystemUser user : users) {
            grpcUsers.add(convertToGrpcUser(user));
        }
        GetUsersResponse response = GetUsersResponse.newBuilder()
                .addAllUsers(grpcUsers)
                .build();

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    } catch (Exception e) {
        responseObserver.onError(e);
    }
}
```
*Code Snippet 5: UserServiceImpl (Appendix 3.1 Source Code)*

#### Create Draft Feature

Once logged in, the teacher can create a course draft. This process involves the Client sending data to the Logic Server, which then forwards it to the Data Server.

![Click Create Draft](createdraft.png)

**Client App (HttpCourseService.cs):**

```csharp
public async Task<Optional<Draft>> CreateDraft(CreateDraftDto dto) 
    => await httpCrudService.CreateAsync<Draft, CreateDraftDto>("drafts", dto);
```
*Code Snippet 6: HttpCourseService (Appendix 3.1 Source Code)*

The Logic Server handles the request at the `/drafts` endpoint:

**Logic Server (CoursesController.cs):**

```csharp
[HttpPost("/drafts")]
[Authorize("MustBeTeacher")]
public async Task<ActionResult<Course>> HttpCreateAsync([FromBody] CreateCourseDto entity) 
    => await CreateAsync(entity);
```
*Code Snippet 7: CoursesController (Appendix 3.1 Source Code)*

The controller uses the `gRPCCourseRepository` to send the data to the Data Server.

**Logic Server (gRPCCourseRepository.cs):**

```csharp
public override async Task<Course> AddAsync(CreateCourseDto entity)
{
    var request = new AddCourseRequest
    {
        Title = entity.Title ?? "",
        Description = entity.Description ?? "",
        Language = entity.Language ?? "",
        Category = entity.Category ?? "",
        AuthorId = entity.AuthorId ?? -1
    };

    var response = await CourseServiceClient.AddCourseAsync(request);

    return new Course
    {
        Id = response.Course.Id,
        Title = response.Course.Title,
        // ... mapping other fields ...
    };
}
```
*Code Snippet 8: gRPCCourseRepository (Appendix 3.1 Source Code)*

The gRPC messages for creating a course are defined as:

```protobuf
message AddCourseRequest {
  string title = 1;
  string description = 2;
  string language = 3;
  string category = 4;
  int32 authorId = 5;
}

message AddCourseResponse {
  Course course = 1;
}
```
*Code Snippet 9: Protocol (Appendix 3.1 Source Code)*

Finally, the Data Server persists the new course draft:

**Data Server (CourseServiceImpl.java):**

```java
@Override
public void addCourse(AddCourseRequest request, StreamObserver<AddCourseResponse> responseObserver) {
    try {
        Course course = new Course();
        course.setTitle(request.getTitle());
        course.setDescription(request.getDescription());
        // ... additional field setting ...
        course = courseRepository.save(course);

        AddCourseResponse response = AddCourseResponse.newBuilder()
                .setCourse(convertToGrpcCourse(course))
                .build();

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    } catch (Exception e) {
        responseObserver.onError(e);
    }
}
```
*Code Snippet 10: CourseServiceImpl (Appendix 3.1 Source Code)*


### Security Implementation

Learnify bases its entire system security approach on the foundation of its Security Policy (Appendix 7.2 Security Policy). The document describes vital security measures which organizations employ to protect their information from unauthorized access and security breaches. The System depends on three critical security principles which include confidentiality, integrity and availability that protect its core functions through multiple essential security measures. The process needs each user to enter their personal login details for authentication. The policy specifies that users needs to create passwords which contain at least eight characters to safeguard their accounts. Role-Based Access Control (RBAC) provides additional protection through its access management system which grants specific permissions to Learners and Teachers and Administrators based on their designated roles. The Logic Server performs user role verification through generated claims to authorize only permitted actions before processing any requests. The endpoints of WebApi logic server have been secured. The team strived to achieve data security by using a well-planned system which organizes information through classification and protects it with encryption methods. The system contains three types of data which include public information that users can access through registration and login pages, internal information that requires authentication to view course catalogs and content and sensitive information which needs encryption for user passwords. The system uses Argon2 as a secure password hashing function which includes salting to protect user information. The system operates with continuous security measures and regular software updates to maintain its network security. It functions through a firewall which grants access to particular ports that are essential for operation.

### Deployment

A deployment diagram was created to visualize the physical deployment of the system components. The diagram illustrates how the system components are dependent on each other and how they interact once deployed. The diagram can be found as Appendix 13.1 Deployment Diagram or seen on the figure below:

![Deployment Diagram (Appendix 13.1 Deployment Diagram)](../out/Deployment/DeploymentDiagram/DeploymentDiagram.svg){width=60%}

## Testing

### Testing Approach

Testing in this project was structured around the V-Model (Sommerville, 2016), where testing activities are planned in parallel with developments phases. Instead of treating testings as a final step, test considerations were introduced early, starting at the requirement analyses, and refined as the system design evolved. This approach ensured that development decision had a corresponding verification strategy.

The first step of the V-Model was requirement analysis, which directly maps to acceptance testing.

At this stage, testing focused on answering a simple but critical question:

How can it be determined whether the system fulfills the user requirements?

Rather then writing code-level tests, the team defined high-level acceptance criteria for each use case. These criteria described:

- What user expects to achieve
- Under which conditions the system should allow or deny actions
- What outcome confirms that the requirement is fulfilled

These acceptance-oriented test ideas were later used to validate the system both manually and through automated test. In this way, test cases were already embedded in the analysis phase.

In practice, different types of tests naturally aligned with different parts of the V-Model:

- High-level test cases and use-case validation corresponded to the upper part of the V (requirements and acceptance testing).
- Automated tests focused mainly on the lower part of the V, especially unit and integration testing.

### Tools and frameworks

A combination of automated and manual testing tools was used throughout the project:

- xUnit / JUnit – Used for writing automated unit and integration tests.
- Mockito / Mocking frameworks – Used to isolate logic and mock external dependencies.
- HTTP client–based tests – Used for endpoint-level testing.
- .http files – Used early in development for manual REST endpoint testing.
- BloomRPC – Used for manually testing gRPC endpoints.
- In-memory repositories – Used to test logic without external dependencies.
- IDE tooling (Visual Studio / IntelliJ) – Used for writing, executing, and debugging tests.

### What was tested

Testing focused primarily on core logic and system-critical behavior, rather than attempting full coverage of all UI components.

The following aspects of the system were tested:

- Business logic in the logic server
- REST and gRPC endpoints
- Authentication and authorization behavior
- Role-based access control
- Course and draft management workflows
- Integration between services

The team aimed for high endpoint coverage, testing endpoints under different scenarios such as:

- Different user roles and credentials
- Authorized vs. unauthorized access
- Expected success paths

However, testing of faulty or malformed input data was less comprehensive. While access control and permissions were thoroughly tested, negative scenarios involving invalid data were sometimes under-tested. This limitation is acknowledged as an area for improvement.

### Method-level test case documentation

Method-level testing focused on logic-heavy and high-impact methods, following a white-box testing approach. Instead of testing every method, priority was given to areas where errors would have the greatest impact on system behavior.

For critical methods, tests were designed to cover:

- Normal execution paths
- Boundary conditions
- Exceptional cases
- Interface correctness

This approach ensured that the internal behavior of key methods was thoroughly validated, not just their external outputs. Test cases were documented to show clear intent and traceability between requirements, logic, and expected outcomes.

The table of all test case results can be found in (Appendix 2.4 Tests). 

### Benefits and bug detection

Testing played an important role in maintaining system stability throughout development and gave the team confidence when introducing changes.

One of the main benefits of automated testing was that it enabled safe refactoring. Because core logic and most endpoints were covered by tests, the team could restructure and improve the codebase without the constant risk of breaking existing functionality. This was especially valuable in later stages of the project, where refactoring became more frequent as the system matured.

In several cases, features appeared to work correctly when tested manually through the user interface, but automated tests revealed issues that were not immediately visible. These included:

- Missing validation for null or unexpected input
- Edge cases and boundary values not being handled correctly
- Operations failing silently without clear feedback

The team followed a continuous test–fix–verify cycle during development. When a problem was discovered, it was corrected, committed through version control, and often accompanied by additional tests to prevent similar issues in the future. Although the project did not strictly follow Test-Driven Development from the start, the gradual shift toward automated testing significantly improved reliability, reduced regressions, and supported ongoing changes as the project evolved.

## Result

Based on the defined use cases and their corresponding test scenarios, the system was evaluated through a combination of automated tests and manual verification. The implemented functionality covers the main workflows for learners, teachers, and administrators that were within the project scope.

The results show that all implemented use cases behave according to their expected outcomes. Core features such as user registration and login, course participation, learning activities, course creation, and administrative management were verified to function correctly.

Regarding security-related aspects, the system communicates over HTTP during development and testing. While the application configuration includes redirection to HTTPS outside of development mode, a full HTTPS setup with proper certificate handling was not established as part of this project. Additionally, no explicit enforcement of a minimum password length (such as an 8-character requirement) is implemented in the current system.

The table with all test cases can be found as (Appendix 2.4 Tests) - the table provided there also contains the test case results as these test cases were used during the testing phase as well.

# Discussion

#### What Has Been Accomplished

The main purpose of this project has been achieved through the creation of the “Learnify” distributed heterogeneous e-learning system which targets the problem of educational inequality. The system operates through a three-tier architecture which combines a C# Blazor client application with a Logic Server built in C# .NET and a Data Server developed in Java. The PostgreSQL database functions as the storage system which maintains all relevant information for every service. The system architecture uses gRPC protocol to establish fast communication between services while it employs HTTP protocol to enable client application interaction. The system has achieved full integration of 21 specific user stories which support the complete learning process. The system enables users to create new accounts through its registration feature. Users can browse course catalogs. Users can take part in courses. The system provides users with suitable interactive learning activities that deliver instant feedback. The system enforces a rigid role-based structure which applies to all users including Learners and Teachers and Administrators. The role hierarchy establishes a governance process which requires Teachers to obtain administrator approval before their content can be published. The security implementation utilizes industrial standards with Argon2 for Password Hashing with salting and JWT for Stateless Authentication. In addition, the User Experience has been strengthened with the incorporation of leaderboards for gamification and a Color Validated User Interface for color-deficient users. The overall requirements have been designed in accordance with the primary values laid down in the project and potential customers' feedback.

#### What Can Be Improved

Even with the successful implementation of the fundamental system, there are some aspects that need optimization towards the reduction of the identified security risks and the improvement of scalability. The basic application needs additional security measures to protect against advanced attacks even though it uses Argon2 password hashing and JSON Web Tokens. The Critical risk of unpatched software vulnerability exploitation requires automated scanning and effective patch management for future developments. The protection of high-risk systems against Man-in-the-Middle attacks and credential exploitation needs Transport Layer Security/Secure Sockets Layer combined with HTTP Strict Transport Security and Content Security Policies to prevent Cross-Site Scripting attacks and Multi-Factor Authentication. The system has passed normal functional tests to meet operational requirements but its performance under severe stress remains untested through extreme load testing. The risk assessment shows that Distributed Denial of Service requires future research to simulate high user volumes for identifying system bottlenecks.
Concerning user experience, there is still a lot of room for improving the inclusivity of the platform. Although the support for color vision disabilities is a good starting point, the support needs to be extended to implement full compatibility with the Web Content Accessibility Guidelines and screen readers for users with motor disabilities. In addition to this, the format of the learning model needs to change from the linear pattern that it currently supports to an adaptive path and also an advanced gamification system incorporating features like badges and streaks. Lastly, to avoid administrative delays that could arise in the future due to the expansion of users, it is advised that the manual content reviewing system should be integrated with an AI-powered content review system.

---

# Conclusion and Recommendations

The main goal of this study was to develop a scalable heterogeneous distributed e-learning system which tackles educational inequalities through superior performance and system integrity, while delivering an excellent user experience. The system development process required a three-tier architectural design which implemented a multi programming language approach by combining C#, Java, and the PostgreSQL database. The tests showed that the system operates effectively when handling multiple sessions and processing data.

The system delivered its core objectives but failed to achieve complete functionality because content moderation features remained unimplemented within the project timeframe. The system establishes a core structure which enables the creation of a distributed system.

Future research should direct its attention toward creating strong security systems which include Multi-Factor Authentication and advanced permission frameworks because the existing basic systems would need to evolve for commercial deployment. The learning platform will become better after the implementation of full accessibility features which include screen reader support and adaptive layout functionality. The platform requires testing for production-level usability and maximum load capacity to achieve production readiness and acceptable latency levels.

The project achieved success through its development of a distributed learning system which met all essential criteria established during the planning phases. However, there are still some areas which require improvement. The results of this study could possibly help educational institutions improve their technology systems which currently exist in schools. Digital learning systems produce various results which need to be evaluated during the evaluation process. The development team needs to resolve privacy issues and content recommendation problems and digital divide challenges for future Learnify learning platform versions.

---

# References

- Haleem, A., Javaid, M., Qadri, M. A., & Suman, R. (2022). Understanding the role of digital technologies in education: A review. Sustainable Operations and Computers, 3(1), 275–285. https://doi.org/10.1016/j.susoc.2022.05.004
- Project Description.
- PTI. (2025, March 2). 40% global population doesn’t have access to education in language they understand: UNESCO. Deccan Herald. https://www.deccanherald.com/world/40-global-population-doesnt-have-access-to-education-in-language-they-understand-unesco-3428194
- Samonas, S., & Coss, D. (2014). The Cia Strikes Back: Redefining Confidentiality, Integrity and Availability in Security. In Journal of Information System Security (Vol. 10, Issue 3). https://www.proso.com/dl/Samonas.pdf
- Siemens, G. (n.d.). Connectivism: A Learning Theory for the Digital Age. https://static1.squarespace.com/static/6820668911e3e5617c36c48c/t/682dadc9690ec5749004d96d/1747824073835/connectivism.pdf
- Habermas, J. (1984). The theory of communicative action: Vol. 1. Reason and the rationalization of society (T. McCarthy, Trans.). https://teddykw2.wordpress.com/wp-content/uploads/2012/07/jurgen-habermas-theory-of-communicative-action-volume-1.pdf
- UNESCO. (2000). The Dakar framework for action: Education for all: Meeting our collective commitments. UNESCO Digital Library. https://unesdoc.unesco.org/ark:/48223/pf0000121147
- Sommerville, I. (2016). Software Engineering Tenth Edition. https://dn790001.ca.archive.org/0/items/bme-vik-konyvek/Software%20Engineering%20-%20Ian%20Sommerville.pdf
- Roques, A. (n.d.). Open-source tool that uses simple textual descriptions to draw beautiful UML diagrams. PlantUML.com. https://plantuml.com/

# Appendices

## Appendix 2.1 Requirements
### Requirements
Can be found as requirements.png

## Appendix 2.2 Use Cases
### Use Case Diagram
Can be found as UseCaseDiagram.svg
### Complete Learning Step Use Case Description
Can be found as CompleteLearningStepUseCaseDescription.png

## Appendix 2.3 Diagrams
### 2.3.1 Activity Diagrams
Can be found in the Activity Diagrams folder
### 2.3.2 System Sequence Diagrams
Can be found in the System Sequence Diagrams folder
### Domain Model
Can be found as DomainModel.png
### Enhanced Entity Relationship Diagram
Can be found as EER.png
### Global Relational Diagram
Can be found as GR.png
### Application Layer Sequence Diagram
Can be found as Application-LayerSD.png

## Appendix 2.4 Tests
### Test Cases
Can be found as Tests.xlsx

## Appendix 3.1 Source Code
### AuthController.cs
Can be found as System/LogicServer/RESTAPI/Controllers/AuthController.cs
### SecureAuthService.cs
Can be found as System/LogicServer/RESTAPI/Services/SecureAuthService.cs
### gRPCUserRepository.cs
Can be found as System/LogicServer/gRPCRepo/gRPCUserRepository.cs
### data_protocol.proto
Can be found as System/DataServer/DataServer/src/main/proto/data_protocol.proto
### UserServiceImpl.java
Can be found as System/DataServer/DataServer/src/main/java/via/sep3/dataserver/service/UserServiceImpl.java
### HttpCourseService.cs
Can be found as System/ClientApp/BlazorApp/Services/HttpCourseService.cs
### CoursesController.cs
Can be found as System/LogicServer/RESTAPI/Controllers/CoursesController.cs
### gRPCCourseRepository.cs
Can be found as System/LogicServer/gRPCRepo/gRPCCourseRepository.cs
### CourseServiceImpl.java
Can be found as System/DataServer/DataServer/src/main/java/via/sep3/dataserver/service/CourseServiceImpl.java

## Appendix 4.1 Relation Schema
### Relational Schema
Can be found as RelationalSchema.png

## Appendix 7.1 Threat Model
### Threat model
Can be found as ThreatModel.pdf

## Appendix 7.2 Security Policy
### Security Policy
Can be found as SecurityPolicy.pdf

## Appendix 9.1 Wireframes
### All Courses Page Wireframe
Can be found as Wireframes-2.png

## Appendix 10.1: Stakeholder Interviews
### Interview David
Can be found as Interview_261125_1.pdf
### Interview Andrej
Can be found as Interview_121025.pdf

## Appendix 11.1 Architecture
### Architectural Overview 
Can be found as ArchitecturalOverview.png

## Appendix 13.1 Deployment Diagram
### Deployment Diagram
Can be found as DeploymentDiagram.svg

